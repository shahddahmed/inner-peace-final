using Microsoft.AspNetCore.Authorization;

namespace InnerPeace.Controllers;
[Authorize(Roles = "User")]

[Route("find-therapist")]
public class FindTherapistController(ApplicationDbContext context) : Controller
{
    public IActionResult Index()
    {
        var specializations = context.Specializations.ToList();
        
        return View(specializations);
    }
}