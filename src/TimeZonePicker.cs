namespace MauiTimeZonePicker;

public class TimeZonePicker : Picker
{
    private readonly ITimeZoneResourceProvider _resourceProvider = new TimeZoneResourceProvider();
    
    public TimeZonePicker()
    {
        var zones = _resourceProvider.GetIanaTimeZoneIds();
        var items = zones.Select(zone =>
        {
            var genericName = _resourceProvider.GetGenericName(zone);
            var location = _resourceProvider.GetLocation(zone);
            return $"{zone} => Generic: \"{genericName}\", Location:\"{location}\"";
        }).ToList();
        
        ItemsSource = items;
    }
}