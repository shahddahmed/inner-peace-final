using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnerPeace.Entities;

public class User : BaseEntity
{
    [Required]
    public string Name { get; set; } = null!;

    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required, MinLength(6)]
    public string Password { get; set; } = null!;

    public string? ImagePath { get; set; } 

    [Required, Phone]
    public string Phone { get; set; } = null!;

    [Required, DataType(DataType.Date)]
    public DateTime Birthday { get; set; }

    [Required]
    public bool IsMale { get; set; }
    /*public virtual ICollection<MoodTracking>? Moods { get; set; }*/
    public virtual ICollection<Rating>? Ratings { get; set; }
    [JsonIgnore]
    public virtual ICollection<Session>? Sessions { get; set; }
}