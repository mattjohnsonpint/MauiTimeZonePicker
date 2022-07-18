namespace MauiTimeZonePicker;

public class TimeZoneResourceProvider : ITimeZoneResourceProvider
{
    public IReadOnlyList<string> GetIanaTimeZoneIds() =>
        TimeZoneInfo.GetSystemTimeZones().Select(tzi => tzi.Id).ToList();

    public string GetGenericName(string timeZoneId)
    {
        var tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        var displayName = tzi.DisplayName;

        // strip off the leading "(UTC+00:00) " offset string
        return displayName.Substring(displayName.IndexOf(')') + 1);
    }

    public string GetLocation(string timeZoneId)
    {
        // Not available from TimeZoneInfo by default
        return "";
    }
}
