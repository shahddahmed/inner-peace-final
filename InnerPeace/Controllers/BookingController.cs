using System.Security.Claims;
using InnerPeace.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace InnerPeace.Controllers;

[Authorize]
[Authorize(Roles = "User")]
public class BookingController(ApplicationDbContext context) : Controller
{
    [Route("{id:guid}")]
    public async Task<IActionResult> Index(Guid id)
    {
        var doctor = await context.Doctors
            .Include(d => d.Sessions.Where(s => s.DateTime >= DateTime.Now))
            .FirstOrDefaultAsync(d => d.Id == id);
        
        return View(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> ReserveSession(DateTime time, double price, Guid doctorId, int duration)
    {
        string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        Guid.TryParse(userIdString, out Guid userId);
        
        SessionDuration? sesstionDuration =
            context.SessionDurations.FirstOrDefault(d => d.Duration == duration);

        var endtime = time.AddMinutes(duration);

        var endtimeUtc = endtime.ToUniversalTime();

        VideoCallMeetingModel? meeting = await endtimeUtc.GenerateConsultingMeetingUrl();
        
        if (meeting == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to generate meeting URL.");
        }
        
        Session session = new()
        {
            DateTime = time,
            Price = price,
            DoctorId = doctorId,
            DurationId = sesstionDuration?.Id ?? Guid.NewGuid(),
            UserId = userId,
            Id = Guid.NewGuid(),
            MeetingId = meeting.MeetingId,
            HostingUrl = meeting.HostRoomUrl!.ToString(),
            ViewerUrl = meeting.ViewerRoomUrl!.ToString()
        };
        
        await context.Sessions.AddAsync(session);
        await context.SaveChangesAsync();

        return RedirectToAction("Index", "Appointment");
    }
    
}