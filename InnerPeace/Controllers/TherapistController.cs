using InnerPeace.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InnerPeace.Controllers;

[Route("therapist")]
public class TherapistController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(string? specializationFilter, string? gender)
    {
        var query = context.Doctors.AsNoTracking();

        /*if (!string.IsNullOrEmpty(specializationFilter))
        {
            var specializationList = specializationFilter.Split(',');
            query = query.Where(d =>
                d.DoctorSpecializations.Any(s => specializationList.Contains(s.Specialization.Name)));
        }

        if (!string.IsNullOrEmpty(gender))
            query = query.Where(d => (gender.ToLower().Trim() == "male" && d.IsMale) ||
                                     (gender.ToLower().Trim() == "female" && !d.IsMale));*/
        ViewBag.Specializations = specializationFilter;
        ViewBag.Gender = gender;

        query = query
            .Include(d => d.DoctorCountries)
            .ThenInclude(e => e.Country)
            .Include(d => d.DoctorLanguages)
            .ThenInclude(d => d.Language)
            .Include(d => d.Educations)
            .ThenInclude(d => d.EducationDegree)
            .Include(d => d.Ratings)
            .Include(d => d.DoctorSpecializations)
            .ThenInclude(d => d.Specialization);

        var doctors = await query.ToListAsync();
        doctors = doctors.ToList();
        
        var languages = await context.Languages.ToListAsync();
        var educationDegreeList = await context.EducationDegree.ToListAsync();
        var specializations = await context.Specializations.ToListAsync();
        var durations = await context.SessionDurations.ToListAsync();

        TherapistViewModel model = new()
        {
            Doctor = doctors,
            Specializations = specializations,
            Durations = durations,
            EducationDegrees = educationDegreeList,
            Languages = languages
        };

        return View(model);
    }
}