using System.Security.Claims;
using InnerPeace.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace InnerPeace.Controllers;

[Authorize(AuthenticationSchemes = "DoctorScheme", Roles = "Doctor")]
[Route("doctor")]
public class DoctorAppointmentController(ApplicationDbContext context) : Controller
{

    public async Task<IActionResult> Index()
    {
        string doctorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        Guid.TryParse(doctorIdString, out Guid doctorId);

        var session = await context.Sessions.Where(s => s.DoctorId == doctorId)
            .Include(s => s.User)
            .Include(s => s.Duration)
            .ToListAsync();

        return View(session);
    }
    
    [HttpPost]
    public async Task<IActionResult> WriteTreatment(Guid sessionId,
        string prescription,
        string? note)
    {

        var session = await context.Sessions.FirstOrDefaultAsync(s => s.Id == sessionId);
        if (session == null)
            return NotFound();
        
        session.Prescription = prescription;
        session.Note = note;
        
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}