using InnerPeace.Entities;

namespace InnerPeace.ViewModels;

public class HomeViewModel
{
    public List<Rating> Ratings { get; set; } = [];
    public List<Doctor> Doctors { get; set; } = [];
}