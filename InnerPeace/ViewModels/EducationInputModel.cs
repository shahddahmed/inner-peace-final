using System.ComponentModel.DataAnnotations;

namespace InnerPeace;

public class EducationInputModel
{
    [Required]
    public string University { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    public DateOnly StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateOnly EndDate { get; set; }
    
    [Required]
    public Guid EducationDegreeId { get; set; }
}