using Microsoft.EntityFrameworkCore;
using SupportTicket.Application.Interfaces;
using SupportTicket.Domain.Entities;
using SupportTicket.Domain.Enums;
using SupportTicket.Infrastructure.Persistence;

namespace SupportTicket.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ticket>> GetAllAsync(
        string? search,
        TicketStatus? status,
        TicketPriority? priority)
    {
        var query = _context.Tickets
            .Include(x => x.StatusHistories)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(x =>
                x.Title.Contains(search) ||
                x.RequesterName.Contains(search));
        }

        if (status.HasValue)
        {
            query = query.Where(x =>
                x.Status == status.Value);
        }

        if (priority.HasValue)
        {
            query = query.Where(x =>
                x.Priority == priority.Value);
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<Ticket?> GetByIdAsync(int id)
    {
        return await _context.Tickets
            .Include(x => x.StatusHistories)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Ticket ticket)
    {
        await _context.Tickets.AddAsync(ticket);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}