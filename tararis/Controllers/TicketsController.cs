using Microsoft.AspNetCore.Mvc;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Interfaces;
using SupportTicket.Domain.Enums;

namespace SupportTicket.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    // GET: api/tickets
    // GET: api/tickets?search=ali
    // GET: api/tickets?status=Open
    // GET: api/tickets?priority=High
    // GET: api/tickets?search=ali&status=Open&priority=High

    [HttpGet]
    public async Task<ActionResult<List<TicketDto>>> GetTickets(
        [FromQuery] string? search,
        [FromQuery] TicketStatus? status,
        [FromQuery] TicketPriority? priority)
    {
        var tickets = await _ticketService.GetTicketsAsync(
            search,
            status,
            priority);

        return Ok(tickets);
    }

    // GET: api/tickets/5

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TicketDto>> GetById(int id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);

        if (ticket == null)
        {
            return NotFound(new
            {
                message = "Ticket not found."
            });
        }

        return Ok(ticket);
    }

    // POST: api/tickets

    [HttpPost]
    public async Task<ActionResult<TicketDto>> Create(
        [FromBody] CreateTicketDto dto)
    {
        var ticket = await _ticketService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = ticket.Id },
            ticket);
    }

    // PATCH: api/tickets/5/status

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> ChangeStatus(
        int id,
        [FromBody] UpdateTicketStatusDto dto)
    {
        var result = await _ticketService.ChangeStatusAsync(
            id,
            dto);

        if (!result)
        {
            return NotFound(new
            {
                message = "Ticket not found."
            });
        }

        return NoContent();
    }
}