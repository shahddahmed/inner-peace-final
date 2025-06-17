using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnerPeace.Entities;

public class Education : BaseEntity
{
    public string University { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    [JsonIgnore]
    public Doctor Doctor { get; set; } = null!;
    public Guid DoctorId { get; set; }
    public Guid EducationDegreeId { get; set; }
    public virtual EducationDegree EducationDegree { get; set; } = null!;
}