namespace MauiTimeZonePicker;

public partial class TimeZoneResourceProvider
{
    public IReadOnlyList<string> GetTimeZoneIds() =>
        TimeZoneInfo.GetSystemTimeZones().Select(tzi => tzi.Id).ToList();

    public string GetGenericName(string timeZoneId)
    {
        var tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        var displayName = tzi.DisplayName;

        // strip off the leading "(UTC+00:00) " offset string
        return displayName[(displayName.IndexOf(')') + 1)..];
    }

    public string? GetLocation(string timeZoneId)
    {
        // Not available from TimeZoneInfo by default
        return null;
    }
}
