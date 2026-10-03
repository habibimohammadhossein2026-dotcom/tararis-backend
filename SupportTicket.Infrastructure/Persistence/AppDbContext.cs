using Microsoft.EntityFrameworkCore;
using SupportTicket.Domain.Entities;
using System.Net.Sockets;

namespace SupportTicket.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ticket> Tickets => Set<Ticket>();

    public DbSet<TicketStatusHistory> TicketStatusHistories
        => Set<TicketStatusHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.RequesterName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Description)
                .IsRequired();

            entity.Property(x => x.Priority)
                .IsRequired();

            entity.Property(x => x.Status)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .IsRequired();
        });

        modelBuilder.Entity<TicketStatusHistory>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Status)
                .IsRequired();

            entity.Property(x => x.StartedAt)
                .IsRequired();

            entity.HasOne(x => x.Ticket)
                .WithMany(x => x.StatusHistories)
                .HasForeignKey(x => x.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}