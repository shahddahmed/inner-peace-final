using Microsoft.EntityFrameworkCore;

namespace InnerPeace.Controllers;

[Route("therapist-profile")]
public class TherapistProfileController(ApplicationDbContext context) : Controller
{
    [Route("{id:guid}")]
    public IActionResult Index(Guid id)
    {
        var doctor = context.Doctors.Include(d => d.DoctorSpecializations)
            .ThenInclude(c => c.Specialization)
            .Include(d => d.Ratings)
            .ThenInclude(r => r.User)
            .Include(d => d.Educations)
            .ThenInclude(d => d.EducationDegree)
            .Include(d => d.DoctorLanguages)
            .ThenInclude(d => d.Language)
            .Include(d => d.DoctorLanguages)
            .ThenInclude(d => d.Language)
            .Include(d => d.DoctorCountries)
            .ThenInclude(d => d.Country)
            .Include(d => d.Sessions)
            .FirstOrDefault(e => e.Id == id);

        if (doctor == null)
            return NotFound();
        
        return View(doctor);
    }
}