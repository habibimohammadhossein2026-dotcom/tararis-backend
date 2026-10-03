using SupportTicket.Domain.Enums;

namespace SupportTicket.Domain.Entities;

public class Ticket
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string RequesterName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TicketPriority Priority { get; set; }

    public TicketStatus Status { get; set; } = TicketStatus.Open;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<TicketStatusHistory> StatusHistories { get; set; }
        = new List<TicketStatusHistory>();
}