using System.ComponentModel.DataAnnotations;
using SupportTicket.Domain.Enums;

namespace SupportTicket.Application.DTOs;

public class UpdateTicketStatusDto
{
    [Required]
    public TicketStatus Status { get; set; }
}