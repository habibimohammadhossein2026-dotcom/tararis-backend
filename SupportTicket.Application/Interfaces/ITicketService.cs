using SupportTicket.Application.DTOs;
using SupportTicket.Domain.Enums;

namespace SupportTicket.Application.Interfaces;

public interface ITicketService
{
    Task<List<TicketDto>> GetTicketsAsync(
        string? search,
        TicketStatus? status,
        TicketPriority? priority);

    Task<TicketDto?> GetByIdAsync(int id);

    Task<TicketDto> CreateAsync(CreateTicketDto dto);

    Task<bool> ChangeStatusAsync(
        int id,
        UpdateTicketStatusDto dto);
}