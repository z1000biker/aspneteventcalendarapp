using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventCalendarApp.Data;
using EventCalendarApp.Models;
using FluentValidation;

namespace EventCalendarApp.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class EventsApiController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IValidator<Event> _validator;
    private readonly ILogger<EventsApiController> _logger;

    public EventsApiController(
        AppDbContext context,
        IValidator<Event> validator,
        ILogger<EventsApiController> logger)
    {
        _context = context;
        _validator = validator;
        _logger = logger;
    }

    /// <summary>
    /// Get all events, optionally filtered by date range
    /// </summary>
    /// <param name="start">Optional start date filter</param>
    /// <param name="end">Optional end date filter</param>
    /// <returns>List of events</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<Event>>>> GetEvents(
        [FromQuery] DateTime? start = null,
        [FromQuery] DateTime? end = null)
    {
        try
        {
            var query = _context.Events.AsQueryable();

            if (start.HasValue)
            {
                query = query.Where(e => e.EndDate >= start.Value);
            }

            if (end.HasValue)
            {
                query = query.Where(e => e.StartDate <= end.Value);
            }

            var events = await query.OrderBy(e => e.StartDate).ToListAsync();

            return Ok(ApiResponse<List<Event>>.SuccessResponse(
                events,
                $"Retrieved {events.Count} event(s)"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving events");
            return StatusCode(500, ApiResponse<List<Event>>.ErrorResponse(
                "An error occurred while retrieving events"));
        }
    }

    /// <summary>
    /// Get a single event by ID
    /// </summary>
    /// <param name="id">Event ID</param>
    /// <returns>Event details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<Event>>> GetEvent(int id)
    {
        try
        {
            var evt = await _context.Events.FindAsync(id);

            if (evt == null)
            {
                return NotFound(ApiResponse<Event>.ErrorResponse(
                    $"Event with ID {id} not found"));
            }

            return Ok(ApiResponse<Event>.SuccessResponse(evt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event {EventId}", id);
            return StatusCode(500, ApiResponse<Event>.ErrorResponse(
                "An error occurred while retrieving the event"));
        }
    }

    /// <summary>
    /// Create a new event
    /// </summary>
    /// <param name="evt">Event data</param>
    /// <returns>Created event</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<Event>>> CreateEvent([FromBody] Event evt)
    {
        try
        {
            // Validate using FluentValidation
            var validationResult = await _validator.ValidateAsync(evt);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<Event>.ErrorResponse(
                    "Validation failed", errors));
            }

            _context.Events.Add(evt);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = evt.Id },
                ApiResponse<Event>.SuccessResponse(evt, "Event created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event");
            return StatusCode(500, ApiResponse<Event>.ErrorResponse(
                "An error occurred while creating the event"));
        }
    }

    /// <summary>
    /// Update an existing event
    /// </summary>
    /// <param name="id">Event ID</param>
    /// <param name="evt">Updated event data</param>
    /// <returns>Updated event</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<Event>>> UpdateEvent(int id, [FromBody] Event evt)
    {
        try
        {
            if (id != evt.Id)
            {
                return BadRequest(ApiResponse<Event>.ErrorResponse(
                    "Event ID mismatch"));
            }

            var existingEvent = await _context.Events.FindAsync(id);
            if (existingEvent == null)
            {
                return NotFound(ApiResponse<Event>.ErrorResponse(
                    $"Event with ID {id} not found"));
            }

            // Validate using FluentValidation
            var validationResult = await _validator.ValidateAsync(evt);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<Event>.ErrorResponse(
                    "Validation failed", errors));
            }

            // Update properties
            existingEvent.Title = evt.Title;
            existingEvent.Description = evt.Description;
            existingEvent.StartDate = evt.StartDate;
            existingEvent.EndDate = evt.EndDate;
            existingEvent.Location = evt.Location;
            existingEvent.Category = evt.Category;
            existingEvent.IsAllDay = evt.IsAllDay;

            await _context.SaveChangesAsync();

            return Ok(ApiResponse<Event>.SuccessResponse(
                existingEvent, "Event updated successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event {EventId}", id);
            return StatusCode(500, ApiResponse<Event>.ErrorResponse(
                "An error occurred while updating the event"));
        }
    }

    /// <summary>
    /// Delete an event
    /// </summary>
    /// <param name="id">Event ID</param>
    /// <returns>Success message</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> DeleteEvent(int id)
    {
        try
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(
                    $"Event with ID {id} not found"));
            }

            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<object>.SuccessResponse(
                new { id }, "Event deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting event {EventId}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "An error occurred while deleting the event"));
        }
    }
}
