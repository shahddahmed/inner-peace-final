namespace InnerPeace.Entities;

public class DoctorSessionSettings : BaseEntity
{
    public Dictionary<int,double> DurationPrice { get; set; } = null!;
    public List<AvailabilityTime> AvailabilityTimes { get; set; } = null!;
    public bool IsAvailable { get; set; }
}