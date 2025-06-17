using System.Text.Json.Serialization;

namespace InnerPeace.Entities;

public class SessionDuration : BaseEntity
{
    public int Duration { get; set; }
    [JsonIgnore]
    public virtual ICollection<Session>? Sessions { get; set; }
}