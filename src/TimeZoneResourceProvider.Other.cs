namespace MauiTimeZonePicker;

public class TimeZoneResourceProvider : ITimeZoneResourceProvider
{
    public IReadOnlyList<string> GetIanaTimeZoneIds()
    {
        return TimeZoneInfo.GetSystemTimeZones().Select(tzi => tzi.Id).ToList();
    }

    public string GetGenericName(string timeZoneId)
    {
        throw new NotImplementedException();
    }

    public string GetLocation(string timeZoneId)
    {
        throw new NotImplementedException();
    }
}
