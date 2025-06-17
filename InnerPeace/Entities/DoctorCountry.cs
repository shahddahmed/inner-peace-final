using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnerPeace.Entities;

public class DoctorCountry
{
    [JsonIgnore]
    public Guid CountryId { get; set; }
    public virtual Country Country { get; set; } = null!;
    [JsonIgnore]
    public Guid DoctorId { get; set; }
    [JsonIgnore]
    public virtual Doctor Doctor { get; set; } = null!;
}