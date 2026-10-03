using SupportTicket.Domain.Enums;

namespace SupportTicket.Application.DTOs;

public class TicketDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string RequesterName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TicketPriority Priority { get; set; }

    public TicketStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsOverdue { get; set; }

    public double TotalOpenHours { get; set; }
}