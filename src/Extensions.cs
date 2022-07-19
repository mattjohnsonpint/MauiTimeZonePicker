namespace MauiTimeZonePicker;

internal static class Extensions
{
    private static readonly string[] UtcAliases = {
        "UTC",
        "Etc/UTC",
        "Etc/UCT",
        "Etc/Universal",
        "Etc/Zulu",
        "UCT",
        "Universal",
        "Zulu"
    };

    public static bool TimeZoneIsUtc(this string timeZoneId) =>
        UtcAliases.Contains(timeZoneId, StringComparer.OrdinalIgnoreCase);

    public static string FormatAsOffset(this TimeSpan offset) =>
        (offset < TimeSpan.Zero ? "-" : "+") + offset.ToString(@"hh\:mm");

    public static string AdjustTimeZoneDisplayText(this string s)
    {
        // Older globalization data may not have made this important change yet.
        return s.Replace("Kiev", "Kyiv", StringComparison.CurrentCultureIgnoreCase);
    }
}
