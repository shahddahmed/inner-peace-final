using System.Text.Json.Serialization;

namespace InnerPeace.Entities;

public class Rating : BaseEntity
{
    public RatingEnum Communication { get; set; }
    public RatingEnum UnderstandingOfTheSituation { get; set; }
    public RatingEnum ProvidingEffectiveSolution { get; set; }
    public RatingEnum CommitmentToStartAndEndTimes { get; set; }
    public string? Comment { get; set; }
    [JsonIgnore]
    public Guid DoctorId { get; set; }
    [JsonIgnore]
    public virtual Doctor Doctor { get; set; } = null!;
    public Guid UserId { get; set; }
    [JsonIgnore]
    public virtual User User { get; set; } = null!;

    public virtual Session Session { get; set; } = null!;
    public Guid SessionId { get; set; }
}