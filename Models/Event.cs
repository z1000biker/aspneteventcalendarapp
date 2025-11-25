using System.ComponentModel.DataAnnotations;

namespace EventCalendarApp.Models;

public class Event
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date is required")]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }

    [StringLength(300, ErrorMessage = "Location cannot exceed 300 characters")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [StringLength(50)]
    public string Category { get; set; } = "General";

    public bool IsAllDay { get; set; }

    // Computed property for FullCalendar
    public string Color => Category switch
    {
        "Work" => "#3b82f6",
        "Personal" => "#10b981",
        "Meeting" => "#f59e0b",
        "Holiday" => "#ef4444",
        _ => "#6366f1"
    };
}
