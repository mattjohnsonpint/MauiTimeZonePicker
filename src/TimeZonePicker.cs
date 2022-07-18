namespace MauiTimeZonePicker;

public class TimeZonePicker : Picker
{
    private readonly ITimeZoneResourceProvider _resourceProvider = new TimeZoneResourceProvider();
    
    public TimeZonePicker()
    {
        var zones = TimeZoneInfo.GetSystemTimeZones();
        var items = zones.Select(zone =>
        {
            var genericName = _resourceProvider.GetGenericName(zone.Id);
            var location = _resourceProvider.GetLocation(zone.Id);
            return $"{zone.Id} => Generic: \"{genericName}\", Location:\"{location}\"";
        }).ToList();
        
        ItemsSource = items;
    }
}