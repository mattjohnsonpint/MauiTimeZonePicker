using System.Diagnostics;

namespace MauiTimeZonePicker;

public class TimeZonePicker : Picker
{
    private readonly ITimeZoneResourceProvider _resourceProvider = new TimeZoneResourceProvider();
    
    public TimeZonePicker()
    {
        var zones = TimeZoneInfo.GetSystemTimeZones();
        foreach (var zone in zones)
        {
            var genericName = _resourceProvider.GetGenericName(zone.Id);
            var location = _resourceProvider.GetLocation(zone.Id);
            Debug.WriteLine($"{zone.Id} => Generic: \"{genericName}\", Location:\"{location}\"");
        }
    }
}