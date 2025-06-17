namespace InnerPeace.Entities;

public class VideoCallMeetingModel
{
    public string MeetingId { get; set; } = null!;

    public string? RoomName { get; set; }

    public Uri RoomUrl { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Uri? HostRoomUrl { get; set; }

    public Uri? ViewerRoomUrl { get; set; }
}