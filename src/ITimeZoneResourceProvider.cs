namespace MauiTimeZonePicker;

public interface ITimeZoneResourceProvider
{
    IReadOnlyList<TimeZoneResource> GetTimeZoneResources();
    IReadOnlyList<string> GetTimeZoneIds();
    string GetGenericName(string timeZoneId);
    string? GetLocation(string timeZoneId);
}
