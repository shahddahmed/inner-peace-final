using System.Security.Claims;
using InnerPeace.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InnerPeace.Controllers;

public class AccountController(ApplicationDbContext context) : Controller
{
    [HttpGet("account/register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("account/register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(
        string name,
        string email,
        string password,
        string phone,
        DateTime birthday,
        bool isMale)
    {
        if (string.IsNullOrEmpty(name) || 
            string.IsNullOrEmpty(email) || 
            string.IsNullOrEmpty(password) || 
            string.IsNullOrEmpty(phone) ||
            birthday == default)
        {
            ModelState.AddModelError("", "Please fill all required fields.");
            return View();
        }

        if (await context.Users.AnyAsync(u => u.Email == email))
        {
            ModelState.AddModelError(nameof(email), "Email is already registered");
            return View();
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            Password = password.HashPassword(),
            Phone = phone,
            Birthday = birthday,
            IsMale = isMale
        };

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "User")
        };

        var claimsIdentity = new ClaimsIdentity(claims, "UserScheme");
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
        };

        await HttpContext.SignInAsync(
            "UserScheme",
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("account/login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("account/login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var hashedPassword = password.HashPassword();
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == hashedPassword);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "User")
        };

        var claimsIdentity = new ClaimsIdentity(claims, "UserScheme");
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
        };

        await HttpContext.SignInAsync(
            "UserScheme",
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToAction("Index", "Home");
    }
    [HttpGet("account/doctor/login")]
    public IActionResult DoctorLogin()
    {
        return View();
    }
    
    [HttpPost("account/doctor/login")]
    public async Task<IActionResult> DoctorLogin(string email, string password)
    {
        var hashedPassword = password.HashPassword();
        var doctor = await context.Doctors.FirstOrDefaultAsync(u => u.Email == email && u.Password == hashedPassword);

        if (doctor == null)
        {
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, doctor.Id.ToString()),
            new Claim(ClaimTypes.Name, doctor.Name),
            new Claim(ClaimTypes.Email, doctor.Email),
            new Claim(ClaimTypes.Role, "Doctor"),
            new Claim("ImagePath", doctor.ImagePath ?? string.Empty)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "DoctorScheme");
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
        };

        await HttpContext.SignInAsync(
            "DoctorScheme",
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToAction("Index", "DoctorAppointment");
    }
    
    [HttpGet("account/doctor/register")]
    public async Task<IActionResult> DoctorRegister()
    {
        var model = new DoctorRegisterViewModel
        {
            Languages = await context.Languages
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name }).ToListAsync(),
            Countries = await context.Countries
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync(),
            Specializations = await context.Set<Specialization>()
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToListAsync(),

            EducationDegrees = await context.EducationDegree.ToListAsync()
        };

        return View(model);
    }

    [HttpPost("account/doctor/register")]
    public async Task<IActionResult> DoctorRegister(DoctorRegisterViewModel model)
    {
        if (await context.Doctors.AnyAsync(u => u.Email == model.Email))
            ModelState.AddModelError(nameof(model.Email), "Email is already registered");
        
        if (await context.Doctors.AnyAsync(u => u.Phone == model.Phone))
            ModelState.AddModelError(nameof(model.Phone), "Phone is already registered");

        if (!ModelState.IsValid)
        {
            model.Languages = await context.Languages
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name }).ToListAsync();
            model.Countries = await context.Countries
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync();
            model.Specializations = await context.Specializations
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToListAsync();
            model.EducationDegrees = await context.EducationDegree.ToListAsync();

            return View(model);
        }

        string imagePath = "";
        if (model.Image != null)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
            var path = Path.Combine("wwwroot/Profiles", fileName);
            await using var stream = new FileStream(path, FileMode.Create);
            await model.Image.CopyToAsync(stream);
            imagePath = $"/Profiles/{fileName}";
        }

        var doctor = new Doctor
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            Password = model.Password.HashPassword(),
            ProfessionTitle = model.ProfessionTitle,
            IsMale = model.IsMale,
            ImagePath = imagePath,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        await context.Doctors.AddAsync(doctor);

        foreach (var langId in model.SelectedLanguageIds)
            await context.AddAsync(new DoctorLanguage { DoctorId = doctor.Id, LanguageId = langId });

        foreach (var countryId in model.SelectedCountryIds)
           await context.AddAsync(new DoctorCountry { DoctorId = doctor.Id, CountryId = countryId });

        foreach (var specId in model.SelectedSpecializationIds)
           await  context.AddAsync(new DoctorSpecialization { DoctorId = doctor.Id, SpecializationId = specId });
        
        if (model.Educations.Any())
        {
            foreach (var edu in model.Educations)
            {
                if (!string.IsNullOrWhiteSpace(edu.University) && edu.StartDate != default && edu.EndDate != default)
                {
                    await context.Educations.AddAsync(new Education()
                    {
                        Id = Guid.NewGuid(),
                        DoctorId = doctor.Id,
                        University = edu.University,
                        StartDate = edu.StartDate,
                        EndDate = edu.EndDate,
                        EducationDegreeId = edu.EducationDegreeId
                    });
                }
            }
        }

        await context.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, doctor.Id.ToString()),
            new Claim(ClaimTypes.Name, doctor.Name),
            new Claim(ClaimTypes.Email, doctor.Email),
            new Claim(ClaimTypes.Role, "Doctor"),
            new Claim("ImagePath", doctor.ImagePath ?? string.Empty)
        };
        
        var claimsIdentity = new ClaimsIdentity(claims, "DoctorScheme");
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
        };

        await HttpContext.SignInAsync(
            "DoctorScheme",
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToAction("Index", "DoctorAppointment");
    }

    [Authorize(AuthenticationSchemes = "UserScheme,DoctorScheme")]
    [HttpGet("account/logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("UserScheme");
        await HttpContext.SignOutAsync("DoctorScheme");
    
        return RedirectToAction("Index", "Home");
    }
    
    [AllowAnonymous]
    [Route("Account/access-denied")]
    public IActionResult AccessDenied()
    {
        return View("AccessDenied");
    }
}
