using System.Collections;

namespace MauiTimeZonePicker;


public class TimeZonePicker : Picker
{
    private readonly ITimeZoneResourceProvider _resourceProvider = new TimeZoneResourceProvider();
    
    public TimeZonePicker()
    {
        Title = "Pick a time zone...";
        ItemsSource = (IList) _resourceProvider.GetTimeZoneResources();
    }
}