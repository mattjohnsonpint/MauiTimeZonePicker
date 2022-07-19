namespace MauiTimeZonePicker;

public partial class TimeZoneResourceProvider : ITimeZoneResourceProvider, IDisposable
{
    public IReadOnlyList<TimeZoneResource> GetTimeZoneResources() =>
        GetTimeZoneIds()
            .Select(id => new TimeZoneResource
            {
                Id = id,
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById(id),
                Name = GetGenericName(id),
                Location = GetLocation(id)
            })
            .OrderBy(r => r.TimeZone.GetUtcOffset(DateTime.UtcNow))
            .ThenBy(r => r.Name)
            .ThenBy(r => r.Location)
            .ToList();
    
    public void Dispose()
    {
        DisposeResources();
        GC.SuppressFinalize(this);
    }

    ~TimeZoneResourceProvider()
    {
        DisposeResources();
    }
}