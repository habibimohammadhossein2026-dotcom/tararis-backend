using System.ComponentModel.DataAnnotations;
using SupportTicket.Domain.Enums;

namespace SupportTicket.Application.DTOs;

public class CreateTicketDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string RequesterName { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public TicketPriority Priority { get; set; }
}