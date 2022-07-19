namespace MauiTimeZonePicker;

internal static  class Helpers
{
    private static readonly string[] UtcAliases = new[] {
        "UTC",
        "Etc/UTC",
        "Etc/UCT",
        "Etc/Universal",
        "Etc/Zulu",
        "UCT",
        "Universal",
        "Zulu"
    };

    public static bool TimeZoneIsUtc(string timeZoneId) =>
        UtcAliases.Contains(timeZoneId, StringComparer.OrdinalIgnoreCase);

    public static string FormatAsOffset(this TimeSpan offset) =>
        (offset < TimeSpan.Zero ? "-" : "+") + offset.ToString(@"hh\:mm");
}
