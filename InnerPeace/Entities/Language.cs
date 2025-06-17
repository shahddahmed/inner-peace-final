using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnerPeace.Entities;

public class Language : BaseEntity
{
    public string Name { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<DoctorLanguage>? DoctorsLanguage { get; set; } 
}