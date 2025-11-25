using System.ComponentModel.DataAnnotations;

namespace EventCalendarApp.Models;

public class EventViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [Display(Name = "Event Title")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Description")]
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    [Display(Name = "Start Date & Time")]
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "End date is required")]
    [Display(Name = "End Date & Time")]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; } = DateTime.Now.AddHours(1);

    [Display(Name = "Location")]
    [StringLength(300, ErrorMessage = "Location cannot exceed 300 characters")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [Display(Name = "Category")]
    public string Category { get; set; } = "General";

    [Display(Name = "All Day Event")]
    public bool IsAllDay { get; set; }

    // Helper method to convert to Event entity
    public Event ToEvent()
    {
        return new Event
        {
            Id = Id,
            Title = Title,
            Description = Description,
            StartDate = StartDate,
            EndDate = EndDate,
            Location = Location,
            Category = Category,
            IsAllDay = IsAllDay
        };
    }

    // Helper method to create from Event entity
    public static EventViewModel FromEvent(Event evt)
    {
        return new EventViewModel
        {
            Id = evt.Id,
            Title = evt.Title,
            Description = evt.Description,
            StartDate = evt.StartDate,
            EndDate = evt.EndDate,
            Location = evt.Location,
            Category = evt.Category,
            IsAllDay = evt.IsAllDay
        };
    }
}
