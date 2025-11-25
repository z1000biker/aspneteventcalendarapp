using FluentValidation;
using EventCalendarApp.Models;

namespace EventCalendarApp.Validators;

public class EventValidator : AbstractValidator<Event>
{
    private static readonly string[] ValidCategories = { "Work", "Personal", "Meeting", "Holiday", "General" };

    public EventValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters");

        RuleFor(e => e.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(e => e.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .LessThan(e => e.EndDate).WithMessage("Start date must be before end date");

        RuleFor(e => e.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(e => e.StartDate).WithMessage("End date must be after start date");

        RuleFor(e => e.Location)
            .MaximumLength(300).WithMessage("Location cannot exceed 300 characters");

        RuleFor(e => e.Category)
            .NotEmpty().WithMessage("Category is required")
            .Must(c => ValidCategories.Contains(c))
            .WithMessage($"Category must be one of: {string.Join(", ", ValidCategories)}");

        // Custom validation: Event duration should not exceed 24 hours for non-all-day events
        When(e => !e.IsAllDay, () =>
        {
            RuleFor(e => e)
                .Must(e => (e.EndDate - e.StartDate).TotalHours <= 24)
                .WithMessage("Non-all-day events cannot exceed 24 hours duration");
        });

        // Custom validation: All-day events should have time set to midnight
        When(e => e.IsAllDay, () =>
        {
            RuleFor(e => e.StartDate)
                .Must(d => d.TimeOfDay == TimeSpan.Zero)
                .WithMessage("All-day events should start at midnight");
        });
    }
}

public class EventViewModelValidator : AbstractValidator<EventViewModel>
{
    private static readonly string[] ValidCategories = { "Work", "Personal", "Meeting", "Holiday", "General" };

    public EventViewModelValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters");

        RuleFor(e => e.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(e => e.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .LessThan(e => e.EndDate).WithMessage("Start date must be before end date");

        RuleFor(e => e.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(e => e.StartDate).WithMessage("End date must be after start date");

        RuleFor(e => e.Location)
            .MaximumLength(300).WithMessage("Location cannot exceed 300 characters");

        RuleFor(e => e.Category)
            .NotEmpty().WithMessage("Category is required")
            .Must(c => ValidCategories.Contains(c))
            .WithMessage($"Category must be one of: {string.Join(", ", ValidCategories)}");
    }
}
