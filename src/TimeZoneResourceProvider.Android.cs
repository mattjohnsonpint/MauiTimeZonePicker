using Android.OS;
using Android.Icu.Text;
using Locale = Java.Util.Locale;

namespace MauiTimeZonePicker;

public class TimeZoneResourceProvider : ITimeZoneResourceProvider, IDisposable
{
private readonly Locale _locale;
    private readonly TimeZoneNames _timeZoneNames;
    private readonly LocaleDisplayNames _localeDisplayNames;
    private readonly Java.Util.Date _referenceDate = new();
    private readonly DateFormat _genericTzFormatter;

    public TimeZoneResourceProvider()
    {
        _locale = GetCurrentLocale();
        _timeZoneNames = TimeZoneNames.GetInstance(_locale)!;
        _localeDisplayNames = LocaleDisplayNames.GetInstance(_locale)!;
        _genericTzFormatter = DateFormat.GetPatternInstance(DateFormat.GenericTz, _locale)!;
    }

    public IReadOnlyList<string> GetIanaTimeZoneIds()
    {
        return TimeZoneInfo.GetSystemTimeZones().Select(tzi => tzi.Id).ToList();
    }

    public string GetGenericName(string timeZoneId)
    {
        timeZoneId = Android.Icu.Util.TimeZone.GetCanonicalID(timeZoneId) ?? timeZoneId;
        if (timeZoneId == "Etc/UTC")
        {
            return _timeZoneNames.GetTimeZoneDisplayName(timeZoneId, TimeZoneNames.NameType.LongStandard)!;
        }
        
        var name = _timeZoneNames.GetDisplayName(timeZoneId, TimeZoneNames.NameType.LongGeneric, _referenceDate.Time)!;

        if (string.IsNullOrWhiteSpace(name))
        {
            // Fallback to formatting if we couldn't get it directly
            using var zone = Android.Icu.Util.TimeZone.GetTimeZone(timeZoneId);
            _genericTzFormatter.TimeZone = zone;
            name = _genericTzFormatter.Format(_referenceDate);
        }

        return name ?? "";
    }

    public string GetLocation(string timeZoneId)
    {
        timeZoneId = Android.Icu.Util.TimeZone.GetCanonicalID(timeZoneId) ?? timeZoneId;
        
        // Get the exemplar location for the time zone
        var location = _timeZoneNames.GetExemplarLocationName(timeZoneId);
        
        // Augment with region name if possible
        if (Android.Icu.Util.TimeZone.GetRegion(timeZoneId) is { } regionCode)
        {
            var regionName = _localeDisplayNames.RegionDisplayName(regionCode);
            if (location != regionName && !string.IsNullOrEmpty(regionName))
            {
                return string.IsNullOrWhiteSpace(location) ? regionName : $"{location}, {regionName}";
            }
        }

        return location ?? "";
    }

    private static Locale GetCurrentLocale()
    {
        var context = Android.App.Application.Context;
        var configuration = context.Resources!.Configuration!;
        if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
        {
            return configuration.Locales.Get(0)!;
        }
#pragma warning disable CS0618
        return configuration.Locale!;
#pragma warning restore CS0618
    }

    public void Dispose()
    {
        _timeZoneNames.Dispose();
        _localeDisplayNames.Dispose();
        _genericTzFormatter.Dispose();
        _referenceDate.Dispose();
        _locale.Dispose();
    }
}