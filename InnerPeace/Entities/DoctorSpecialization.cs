using System.Text.Json.Serialization;

namespace InnerPeace.Entities;

public class DoctorSpecialization
{
    [JsonIgnore]
    public Guid SpecializationId { get; set; }
    [JsonIgnore]
    public virtual Specialization Specialization { get; set; } = null!;
    [JsonIgnore]
    public Guid DoctorId { get; set; }
    public virtual Doctor Doctor { get; set; } = null!;
}