using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportTicket.Application.Interfaces;
using SupportTicket.Infrastructure.Persistence;
using SupportTicket.Infrastructure.Repositories;

namespace SupportTicket.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString(
                    "DefaultConnection")));

        services.AddScoped<ITicketRepository, TicketRepository>();

        return services;
    }
}