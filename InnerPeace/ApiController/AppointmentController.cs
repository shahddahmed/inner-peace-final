using InnerPeace.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace InnerPeace.ApiController;
[Authorize]

[ApiController]
[Route("api/[controller]")]
public class AppointmentController(ApplicationDbContext context) : ControllerBase
{
    [HttpPost("GetAvailableSlots")]
    public async Task<IActionResult> GetAvailableSlots([FromBody] AvailabilityRequestModel request)
    {
        var doctor = await context.Doctors
            .Include(d => d.Sessions)!
            .ThenInclude(d=>d.Duration)
            .FirstOrDefaultAsync(d => d.Id == request.DoctorId);

        if (doctor == null)
            return new JsonResult(new List<AvailabilityHour>());

        var available = GetAvailable(
            request.Date,
            doctor.Sessions?.ToList() ?? [],
            doctor.SessionSettings ?? new DoctorSessionSettings(),
            request.Duration
        );
        
        return new JsonResult(available);
    }

    private List<AvailabilityHour> GetAvailable(DateOnly date,
        List<Session> sessions,
        DoctorSessionSettings settings,
        int duration)
    {
        var day = DaysEnum.Saturday;

        if (DateOnly.FromDateTime(DateTime.Now) > date)
            return [];

        day = (DaysEnum)Enum.Parse(typeof(DaysEnum), date.DayOfWeek.ToString());

        var timesFromDb = settings?.AvailabilityTimes;

        if (!settings?.IsAvailable ?? false)
            return [];

        var availabilityHoursFromDb = timesFromDb?.Where(e => e.Day == day)
            .SelectMany(e => e.AvailabilityHours ?? [])
            .ToList();

        if (availabilityHoursFromDb == null || !availabilityHoursFromDb.Any())
            return [];

        var availableSlots = SplitAvailabilityHours(availabilityHoursFromDb,
            duration, date == DateOnly.FromDateTime(DateTime.UtcNow));

        var freeSlots = ExcludeReservedTimes(availableSlots,
            sessions.Where(s => DateOnly.FromDateTime(s.DateTime.Date) == date).ToList());

        return freeSlots;
    }

    private List<AvailabilityHour> SplitAvailabilityHours(List<AvailabilityHour> availabilityHours, int sessionDuration, bool isToday)
    {
        var now = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(3));

        var result = availabilityHours
            .SelectMany(availability =>
            {
                var start = availability.AvailableFrom;
                var end = availability.AvailableTo;

                return Enumerable.Range(0, (int)(end - start).TotalMinutes / sessionDuration)
                    .Select(i => new AvailabilityHour
                    {
                        AvailableFrom = start.AddMinutes(i * sessionDuration),
                        AvailableTo = start.AddMinutes((i + 1) * sessionDuration)
                    })
                    .Where(slot => !isToday || slot.AvailableFrom > now);
            })
            .ToList();

        return result;
    }

    private List<AvailabilityHour> ExcludeReservedTimes(List<AvailabilityHour> availableSlots, List<Session> sessions)
    {
        foreach (var session in sessions)
        {
            var reservedStart = TimeOnly.FromDateTime(session.DateTime);
            var reservedEnd = reservedStart.AddMinutes(session.Duration.Duration);

            availableSlots.RemoveAll(slot =>
                (slot.AvailableFrom >= reservedStart && slot.AvailableFrom < reservedEnd) ||
                (slot.AvailableTo > reservedStart && slot.AvailableTo <= reservedEnd) ||
                (slot.AvailableFrom <= reservedStart && slot.AvailableTo >= reservedEnd));
        }

        return availableSlots;
    }
}