using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnerPeace.Entities;

public class Country : BaseEntity
{
    public string Name { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<DoctorCountry>? DoctorsCountry { get; set; }
}