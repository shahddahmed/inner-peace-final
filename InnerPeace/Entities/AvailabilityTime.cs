namespace InnerPeace.Entities;

public record AvailabilityTime
{
    public DaysEnum Day { get; set; }
    public List<AvailabilityHour>? AvailabilityHours { get; set; }
}