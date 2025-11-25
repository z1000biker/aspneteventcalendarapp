using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventCalendarApp.Data;
using EventCalendarApp.Models;
using FluentValidation;

namespace EventCalendarApp.Controllers;

public class EventsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IValidator<EventViewModel> _validator;
    private readonly ILogger<EventsController> _logger;

    public EventsController(
        AppDbContext context,
        IValidator<EventViewModel> validator,
        ILogger<EventsController> logger)
    {
        _context = context;
        _validator = validator;
        _logger = logger;
    }

    // GET: Events
    public async Task<IActionResult> Index()
    {
        var events = await _context.Events.OrderBy(e => e.StartDate).ToListAsync();
        return View(events);
    }

    // GET: Events/Create
    public IActionResult Create()
    {
        var model = new EventViewModel
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddHours(1)
        };
        return View(model);
    }

    // POST: Events/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventViewModel model)
    {
        // Validate using FluentValidation
        var validationResult = await _validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(model);
        }

        if (ModelState.IsValid)
        {
            try
            {
                var evt = model.ToEvent();
                _context.Events.Add(evt);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Event created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event");
                ModelState.AddModelError("", "An error occurred while creating the event.");
            }
        }

        return View(model);
    }

    // GET: Events/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var evt = await _context.Events.FindAsync(id);
        if (evt == null)
        {
            return NotFound();
        }

        var model = EventViewModel.FromEvent(evt);
        return View(model);
    }

    // POST: Events/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EventViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        // Validate using FluentValidation
        var validationResult = await _validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(model);
        }

        if (ModelState.IsValid)
        {
            try
            {
                var evt = await _context.Events.FindAsync(id);
                if (evt == null)
                {
                    return NotFound();
                }

                evt.Title = model.Title;
                evt.Description = model.Description;
                evt.StartDate = model.StartDate;
                evt.EndDate = model.EndDate;
                evt.Location = model.Location;
                evt.Category = model.Category;
                evt.IsAllDay = model.IsAllDay;

                _context.Update(evt);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Event updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EventExists(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating event {EventId}", id);
                ModelState.AddModelError("", "An error occurred while updating the event.");
            }
        }

        return View(model);
    }

    // POST: Events/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt == null)
            {
                return NotFound();
            }

            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Event deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting event {EventId}", id);
            TempData["ErrorMessage"] = "An error occurred while deleting the event.";
            return RedirectToAction(nameof(Index));
        }
    }

    private async Task<bool> EventExists(int id)
    {
        return await _context.Events.AnyAsync(e => e.Id == id);
    }
}
