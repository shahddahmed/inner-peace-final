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

        doctor.SessionSettings ??= new();

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

        doctor.SessionSettings ??= new DoctorSessionSettings();

        // Ensure AvailabilityTimes is initialized as an empty list if it is null
        doctor.SessionSettings.AvailabilityTimes ??= new List<AvailabilityTime>();

        doctor.SessionSettings.IsAvailable = true;

        // Retrieve the existing availability time for the specified day or create a new one
        var time = doctor.SessionSettings.AvailabilityTimes.FirstOrDefault(s => s.Day == day);

        // If no existing availability time is found, create a new one
        if (time == null)
        {
            time = new AvailabilityTime
            {
                Day = day,
                AvailabilityHours = new List<AvailabilityHour>
            {
                new AvailabilityHour
                {
                    AvailableFrom = startTime,
                    AvailableTo = endTime
                }
            }
            };

            // Add the newly created availability time
            doctor.SessionSettings.AvailabilityTimes.Add(time);
        }
        else
        {
            // If the availability time exists, update the existing availability hours
            time?.AvailabilityHours?.Clear(); // Clear any existing availability hours
            time?.AvailabilityHours?.Add(new AvailabilityHour
            {
                AvailableFrom = startTime,
                AvailableTo = endTime
            });
        }

        // Save changes to the database
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

}