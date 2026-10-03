using SupportTicket.Domain.Enums;

namespace SupportTicket.Domain.Entities;

public class TicketStatusHistory
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public TicketStatus Status { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? EndedAt { get; set; }

    public Ticket Ticket { get; set; } = null!;
}