using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnerPeace.Entities;

public class Doctor : BaseEntity
{
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ProfessionTitle { get; set; } = null!;
    public bool IsMale { get; set; }
    public DoctorSessionSettings? SessionSettings { get; set; }
    public virtual ICollection<Education> Educations { get; set; } = null!;
    public virtual ICollection<DoctorLanguage> DoctorLanguages { get; set; } = null!;
    public virtual ICollection<DoctorCountry> DoctorCountries { get; set; } = null!;
    public virtual ICollection<DoctorSpecialization> DoctorSpecializations { get; set; } = null!;
    public virtual ICollection<Rating>? Ratings { get; set; }
    [JsonIgnore]
    public virtual ICollection<Session>? Sessions { get; set; }
}