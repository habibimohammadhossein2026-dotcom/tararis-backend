using SupportTicket.Domain.Entities;
using SupportTicket.Domain.Enums;

namespace SupportTicket.Application.Interfaces;

public interface ITicketRepository
{
    Task<List<Ticket>> GetAllAsync(
        string? search,
        TicketStatus? status,
        TicketPriority? priority);

    Task<Ticket?> GetByIdAsync(int id);

    Task AddAsync(Ticket ticket);

    Task SaveChangesAsync();
}