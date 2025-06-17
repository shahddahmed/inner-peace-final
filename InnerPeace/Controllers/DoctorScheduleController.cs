using System.Security.Claims;
using InnerPeace.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace InnerPeace.Controllers;

[Authorize(AuthenticationSchemes = "DoctorScheme", Roles = "Doctor")]
[Route("doctor/schedule")]
public class DoctorScheduleController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        string doctorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        Guid.TryParse(doctorIdString, out Guid doctorId);

        var settings = (await context.Doctors.FirstOrDefaultAsync(d => d.Id == doctorId))?.SessionSettings ?? new();
        
        return View(settings);
    }
    
    [HttpPost("schedule-toggle")]
    public async Task<IActionResult> ChangeAvailability(bool isAvailable)
    {
        string doctorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        Guid.TryParse(doctorIdString, out Guid doctorId);

        var doctor = await context.Doctors.FirstOrDefaultAsync(d => d.Id == doctorId);
        if (doctor == null)
            return NotFound();

        doctor.SessionSettings ??= new ();
            
        doctor.SessionSettings.IsAvailable = isAvailable;
            
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    [HttpPost("schedule-time")]
    public async Task<IActionResult> AddAvailabilityTimes(DaysEnum day, TimeOnly startTime, TimeOnly endTime)
    {
        string doctorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        Guid.TryParse(doctorIdString, out Guid doctorId);

        var doctor = await context.Doctors.FirstOrDefaultAsync(d => d.Id == doctorId);
        if (doctor == null)
            return NotFound();

        doctor.SessionSettings ??= new ();
            
        doctor.SessionSettings.IsAvailable = true;

        var time = doctor.SessionSettings.AvailabilityTimes.FirstOrDefault(s => s.Day == day);
        time ??= new()
        {
            Day = day,
            AvailabilityHours =
            [
                new()
                {
                    AvailableFrom = startTime,
                    AvailableTo = endTime
                }
            ]
        };

        doctor.SessionSettings.AvailabilityTimes.RemoveAll(s => s.Day == day);
        doctor.SessionSettings.AvailabilityTimes.Add(time);
        
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}