using System.Text.Json.Serialization;

namespace InnerPeace.Entities;

public class Specialization : BaseEntity
{
    public string Name { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<DoctorSpecialization>? DoctorsSpecialization { get; set; }
}