namespace MauiTimeZonePicker;

using Foundation;

public partial class TimeZoneResourceProvider
{
    private readonly NSDateFormatter _formatter = new();
    private readonly NSLocale _locale = NSLocale.CurrentLocale;
    private readonly NSDate _referenceDate = NSDate.Now;

    private readonly Lazy<IDictionary<string, string>> _zoneToRegionMap = new(() =>
        File.ReadLines("/usr/share/zoneinfo/zone.tab")
            .Where(line => !line.StartsWith("#") || string.IsNullOrWhiteSpace(line))
            .Select(line => line.Split())
            .ToDictionary(parts => parts[2], parts => parts[0]));

    public TimeZoneResourceProvider()
    {
        _formatter.Locale = _locale;
        _formatter.DefaultDate = _referenceDate;
    }

    public IReadOnlyList<string> GetTimeZoneIds() =>
        TimeZoneInfo.GetSystemTimeZones().Select(tzi => tzi.Id).ToList();

    public string GetGenericName(string timeZoneId)
    {
        using var zone = new NSTimeZone(timeZoneId);
        return zone.GetLocalizedName(NSTimeZoneNameStyle.Generic, _locale);
    }

    public string? GetLocation(string timeZoneId)
    {
        if (timeZoneId.TimeZoneIsUtc())
        {
            return null;
        }
        
        // Get the exemplar location for the time zone
        // See https://unicode-org.github.io/icu/userguide/format_parse/datetime/#date-field-symbol-table
        using var zone = new NSTimeZone(timeZoneId);
        _formatter.TimeZone = zone;
        _formatter.DateFormat = "VVV";
        var location = _formatter.ToString(_referenceDate)?.AdjustTimeZoneDisplayText();

        // Augment with region name if possible
        if (_zoneToRegionMap.Value.TryGetValue(timeZoneId, out var regionCode))
        {
            var regionName = _locale.GetCountryCodeDisplayName(regionCode);
            if (location != regionName && !string.IsNullOrEmpty(regionName))
            {
                return string.IsNullOrWhiteSpace(location) ? regionName : $"{location}, {regionName}";
            }
        }

        return string.IsNullOrWhiteSpace(location) ? null : location;
    }

    private void DisposeResources()
    {
        _formatter.Dispose();
        _locale.Dispose();
        _referenceDate.Dispose();
    }
}