using System.Text.Json.Serialization;

namespace InnerPeace.Entities;

public class MoodTracking : BaseEntity
{
    public MoodEnum Mood { get; set; }
    public string? Notes { get; set; }
    public Guid UserId { get; set; }
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}