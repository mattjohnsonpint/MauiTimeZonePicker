namespace MauiTimeZonePicker;

public class TimeZoneResourceProvider : ITimeZoneResourceProvider, IDisposable
{
    public IReadOnlyList<string> GetIanaTimeZoneIds() => 
        // TODO: This will be limited. We could P/Invoke ICU (ucal_openTimeZoneIdEnumeration) to get more data.
        TimeZoneInfo.GetSystemTimeZones()
            .Select(tzi =>
            {
                if (tzi.HasIanaId)
                {
                    return tzi.Id;
                }

                if (TimeZoneInfo.TryConvertWindowsIdToIanaId(tzi.Id, out var id))
                {
                    return id;
                }

                return "";
            })
            .Where(x => x != "")
            .ToList();

    public string GetGenericName(string timeZoneId)
    {
        var tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        var displayName = tzi.DisplayName;

        // strip off the leading "(UTC+00:00) " offset string
        return displayName.Substring(displayName.IndexOf(')') + 1);
    }

    public string GetLocation(string timeZoneId)
    {
        // Not available from TimeZoneInfo by default
        return "";
    }

    public void Dispose()
    {
    }
}