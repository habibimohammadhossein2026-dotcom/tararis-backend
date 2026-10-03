using SupportTicket.Application.DTOs;
using SupportTicket.Application.Interfaces;
using SupportTicket.Domain.Entities;
using SupportTicket.Domain.Enums;

namespace SupportTicket.Application.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _repository;

    public TicketService(ITicketRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TicketDto>> GetTicketsAsync(
        string? search,
        TicketStatus? status,
        TicketPriority? priority)
    {
        var tickets = await _repository.GetAllAsync(
            search,
            status,
            priority);

        return tickets
            .Select(MapToDto)
            .ToList();
    }

    public async Task<TicketDto?> GetByIdAsync(int id)
    {
        var ticket = await _repository.GetByIdAsync(id);

        if (ticket == null)
            return null;

        return MapToDto(ticket);
    }

    public async Task<TicketDto> CreateAsync(
        CreateTicketDto dto)
    {
        var now = DateTime.UtcNow;

        var ticket = new Ticket
        {
            Title = dto.Title,
            RequesterName = dto.RequesterName,
            Description = dto.Description,
            Priority = dto.Priority,
            Status = TicketStatus.Open,
            CreatedAt = now
        };

        ticket.StatusHistories.Add(
            new TicketStatusHistory
            {
                Status = TicketStatus.Open,
                StartedAt = now
            });

        await _repository.AddAsync(ticket);
        await _repository.SaveChangesAsync();

        return MapToDto(ticket);
    }

    public async Task<bool> ChangeStatusAsync(
        int id,
        UpdateTicketStatusDto dto)
    {
        var ticket = await _repository.GetByIdAsync(id);

        if (ticket == null)
            return false;

        if (ticket.Status == dto.Status)
            return true;

        var now = DateTime.UtcNow;

        var currentHistory = ticket.StatusHistories
            .Where(x => x.EndedAt == null)
            .OrderByDescending(x => x.StartedAt)
            .FirstOrDefault();

        if (currentHistory != null)
        {
            currentHistory.EndedAt = now;
        }

        ticket.Status = dto.Status;

        ticket.StatusHistories.Add(
            new TicketStatusHistory
            {
                TicketId = ticket.Id,
                Status = dto.Status,
                StartedAt = now
            });

        await _repository.SaveChangesAsync();

        return true;
    }

    private static TicketDto MapToDto(Ticket ticket)
    {
        var openDuration = CalculateOpenDuration(ticket);

        return new TicketDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            RequesterName = ticket.RequesterName,
            Description = ticket.Description,
            Priority = ticket.Priority,
            Status = ticket.Status,
            CreatedAt = ticket.CreatedAt,

            TotalOpenHours = openDuration.TotalHours,

            IsOverdue =
                ticket.Priority == TicketPriority.High &&
                ticket.Status == TicketStatus.Open &&
                openDuration > TimeSpan.FromHours(24)
        };
    }

    private static TimeSpan CalculateOpenDuration(
        Ticket ticket)
    {
        var now = DateTime.UtcNow;

        var total = TimeSpan.Zero;

        foreach (var history in ticket.StatusHistories)
        {
            if (history.Status != TicketStatus.Open)
                continue;

            var end = history.EndedAt ?? now;

            total += end - history.StartedAt;
        }

        return total;
    }
}