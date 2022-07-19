namespace MauiTimeZonePicker;

public partial class TimeZoneResourceProvider : ITimeZoneResourceProvider
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
            .OrderBy(r => r.TimeZone.BaseUtcOffset)
            .ThenBy(r => r.Name)
            .ThenBy(r => r.Location)
            .ToList();
}