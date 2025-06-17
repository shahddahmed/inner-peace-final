namespace InnerPeace.Controllers;

[Route("support")]

public class SupportController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}