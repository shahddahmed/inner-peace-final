using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace InnerPeace.Entities;

public static class AuthHelper
{
    public static string HashPassword(this string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}

public static class WherebyService
{

    public static async Task<VideoCallMeetingModel?> GenerateConsultingMeetingUrl(this DateTime endTime)
    {
        object request = new
        {
            EndDate = endTime,
            IsLocked = true,
            RoomMode = "normal",
            Fields = new List<string> { "hostRoomUrl", "viewerRoomUrl" }

        };
        
        var httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmFwcGVhci5pb" +
            "iIsImF1ZCI6Imh0dHBzOi8vYXBpLmFwcGVhci5pbi92MSIsImV4cCI6OTAwNzE5OTI1NDc0MDk5MSwiaWF0Ijox" +
            "NzUwMDU0Nzc1LCJvcmdhbml6YXRpb25JZCI6MzE4MDQ0LCJqdGkiOiI3MWYxZDc5Mi01MDMyLTRjODItYmNjMi04MzE" +
            "3YjE5MmMwMDIifQ.s0DMpCboQhE3pxvrOChch6TwYiYnZtr_wg9-CzyyUVM");

        VideoCallMeetingModel? meeting = null;

        HttpResponseMessage response =
            await httpClient.PostAsJsonAsync("https://api.whereby.dev/v1/meetings", request);

        if (response.IsSuccessStatusCode)
            meeting = await response.Content.ReadFromJsonAsync<VideoCallMeetingModel>();

        return meeting;
    }

}