using System.Collections;

namespace MauiTimeZonePicker;

public class TimeZonePicker : Picker
{
    private readonly ITimeZoneResourceProvider _resourceProvider = new TimeZoneResourceProvider();
    
    public TimeZonePicker()
    {
        ItemsSource = (IList) _resourceProvider.GetTimeZoneResources();
    }
}