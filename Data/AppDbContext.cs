using Microsoft.EntityFrameworkCore;
using EventCalendarApp.Models;

namespace EventCalendarApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Event> Events { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Event entity
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Location).HasMaxLength(300);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
            entity.Property(e => e.StartDate).IsRequired();
            entity.Property(e => e.EndDate).IsRequired();

            // Add index for better query performance
            entity.HasIndex(e => e.StartDate);
            entity.HasIndex(e => e.Category);
        });

        // Seed initial data
        modelBuilder.Entity<Event>().HasData(
            new Event
            {
                Id = 1,
                Title = "Project Kickoff Meeting",
                Description = "Initial meeting to discuss project scope and timeline",
                StartDate = DateTime.Now.Date.AddDays(1).AddHours(10),
                EndDate = DateTime.Now.Date.AddDays(1).AddHours(11),
                Location = "Conference Room A",
                Category = "Meeting",
                IsAllDay = false
            },
            new Event
            {
                Id = 2,
                Title = "Code Review Session",
                Description = "Review pull requests and discuss code quality improvements",
                StartDate = DateTime.Now.Date.AddDays(2).AddHours(14),
                EndDate = DateTime.Now.Date.AddDays(2).AddHours(15).AddMinutes(30),
                Location = "Virtual - Teams",
                Category = "Work",
                IsAllDay = false
            },
            new Event
            {
                Id = 3,
                Title = "Team Building Event",
                Description = "Annual team building activities and lunch",
                StartDate = DateTime.Now.Date.AddDays(5),
                EndDate = DateTime.Now.Date.AddDays(5),
                Location = "City Park",
                Category = "Personal",
                IsAllDay = true
            },
            new Event
            {
                Id = 4,
                Title = "Christmas Holiday",
                Description = "Office closed for Christmas celebration",
                StartDate = new DateTime(2025, 12, 25),
                EndDate = new DateTime(2025, 12, 25),
                Location = "N/A",
                Category = "Holiday",
                IsAllDay = true
            }
        );
    }
}
