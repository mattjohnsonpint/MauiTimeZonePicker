using System.Collections;

namespace MauiTimeZonePicker;


public class TimeZonePicker : Picker, IDisposable
{
    private readonly ITimeZoneResourceProvider _resourceProvider = new TimeZoneResourceProvider();
    
    public TimeZonePicker()
    {
        Title = "Pick a time zone...";
        ItemsSource = (IList) _resourceProvider.GetTimeZoneResources();
    }

    private void DisposeResources()
    {
        (_resourceProvider as IDisposable)?.Dispose();
    }

    public void Dispose()
    {
        DisposeResources();
        GC.SuppressFinalize(this);
    }

    ~TimeZonePicker()
    {
        DisposeResources();
    }
}