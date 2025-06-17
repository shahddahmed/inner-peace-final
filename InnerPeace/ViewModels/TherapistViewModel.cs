using InnerPeace.Entities;

namespace InnerPeace.ViewModels;

public class TherapistViewModel
{
    public List<Doctor> Doctor { get; set; } = [];
    public List<Specialization> Specializations { get; set; } = [];
    public List<Language> Languages { get; set; } = [];
    public List<SessionDuration> Durations { get; set; } = [];
    public List<EducationDegree> EducationDegrees { get; set; } = [];
}