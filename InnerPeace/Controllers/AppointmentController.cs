using System.Security.Claims;
using InnerPeace.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace InnerPeace.Controllers;
[Authorize(Roles = "User")]
[Route("appointment")]
public class AppointmentController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        Guid.TryParse(userIdString, out Guid userId);

        var session = await context.Sessions.Where(s => s.UserId == userId)
            .Include(s => s.Doctor)
            .Include(s => s.Duration)
            .Include(s => s.Rating)
            .ToListAsync();

        return View(session);
    }
    
    [HttpPost]
    public async Task<IActionResult> RateSession(Guid sessionId,
        RatingEnum communication,
        RatingEnum understandingOfTheSituation,
        RatingEnum providingEffectiveSolution,
        RatingEnum commitmentToStartAndEndTimes,
        Guid doctorId,
        string? comment = null)
    {
        
        string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        Guid.TryParse(userIdString, out Guid userId);
        Rating rating = new()
        {
            Communication = communication,
            CommitmentToStartAndEndTimes = commitmentToStartAndEndTimes,
            ProvidingEffectiveSolution = providingEffectiveSolution,
            UnderstandingOfTheSituation = understandingOfTheSituation,
            Comment = comment,
            SessionId = sessionId,
            DoctorId = doctorId,
            UserId = userId,
            Id = Guid.NewGuid()
        };
        
        await context.Ratings.AddAsync(rating);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}