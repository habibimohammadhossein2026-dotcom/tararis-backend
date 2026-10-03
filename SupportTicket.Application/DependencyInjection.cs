using Microsoft.Extensions.DependencyInjection;
using SupportTicket.Application.Interfaces;
using SupportTicket.Application.Services;

namespace SupportTicket.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<ITicketService, TicketService>();

        return services;
    }
}