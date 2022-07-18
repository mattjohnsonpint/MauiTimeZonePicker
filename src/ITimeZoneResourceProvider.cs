namespace MauiTimeZonePicker;

public interface ITimeZoneResourceProvider
{
    string GetGenericName(string timeZoneId);
    string GetLocation(string timeZoneId);
}
