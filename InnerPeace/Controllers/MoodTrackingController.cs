using System.Security.Claims;
using InnerPeace.Entities;
using Microsoft.AspNetCore.Authorization;

namespace InnerPeace.Controllers;
[Authorize(Roles = "User")]

[Route("mood-tracking")]
public class MoodTrackingController(ApplicationDbContext context) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateMood(MoodEnum mood,
        string? note)
    {
        string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        Guid.TryParse(userIdString, out Guid userId);
        
        MoodTracking model = new()
        {
            UserId = userId,
            Mood = mood,
            Id = Guid.NewGuid(),
            Notes = note
        };
        
        await context.MoodsTracking.AddAsync(model);

        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    } 
}