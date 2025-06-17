using System.ComponentModel.DataAnnotations;
using InnerPeace.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InnerPeace;

public class DoctorRegisterViewModel
{
    [Required]
    public string Name { get; set; } = null!;

    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Phone { get; set; } = null!;

    [Required]
    public string ProfessionTitle { get; set; } = null!;

    [Required, MinLength(6)]
    public string Password { get; set; } = null!;

    [Required, Compare("Password")]
    public string ConfirmPassword { get; set; } = null!;

    [Required]
    public bool IsMale { get; set; } = true;

    public IFormFile? Image { get; set; }

    [Required]
    public List<Guid> SelectedLanguageIds { get; set; } = [];

    [Required]
    public List<Guid> SelectedCountryIds { get; set; } = [];

    [Required]
    public List<Guid> SelectedSpecializationIds { get; set; } = [];
    
    public List<EducationInputModel> Educations { get; set; } = [];

    public List<SelectListItem> Languages { get; set; } = [];
    public List<SelectListItem> Countries { get; set; } = [];
    public List<SelectListItem> Specializations { get; set; } = [];
    
    public List<EducationDegree> EducationDegrees { get; set; } = [];
}