using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnerPeace.Entities;

public class DoctorLanguage
{
    [JsonIgnore]
    public Guid LanguageId { get; set; }

    public virtual Language Language { get; set; } = null!;
    [JsonIgnore]
    public Guid DoctorId { get; set; }
    [JsonIgnore]
    public virtual Doctor Doctor { get; set; } = null!;
}