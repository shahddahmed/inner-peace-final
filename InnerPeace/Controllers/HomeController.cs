using InnerPeace.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InnerPeace.Controllers;

[Route("/")]
public class HomeController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var ratings = await context.Ratings.Include(r => r.Doctor).ToListAsync();
        var doctors = await context.Doctors.Take(5).ToListAsync();

        HomeViewModel model = new() { Ratings = ratings, Doctors = doctors };

        return View(model);
    }
}