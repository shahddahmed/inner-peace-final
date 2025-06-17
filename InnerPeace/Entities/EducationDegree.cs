namespace InnerPeace.Entities;

public class EducationDegree : BaseEntity
{
    public string Degree { get; set; } = null!;
    public virtual ICollection<Education>? Educations { get; set; } 
}