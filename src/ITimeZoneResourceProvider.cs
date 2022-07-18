namespace MauiTimeZonePicker;

public interface ITimeZoneResourceProvider
{
    IReadOnlyList<string> GetIanaTimeZoneIds();
    string GetGenericName(string timeZoneId);
    string GetLocation(string timeZoneId);
}
