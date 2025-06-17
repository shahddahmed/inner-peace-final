namespace InnerPeace.Entities;

public record AvailabilityHour
{
    public TimeOnly AvailableFrom { get; set; }
    public TimeOnly AvailableTo { get; set; }
}