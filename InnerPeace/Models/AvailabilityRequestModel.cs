namespace InnerPeace.ApiController;

public class AvailabilityRequestModel
{
    public Guid DoctorId { get; set; }
    public DateOnly Date { get; set; }
    public int Duration { get; set; }
}