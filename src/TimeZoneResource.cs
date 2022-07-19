using System.Text;

namespace MauiTimeZonePicker;

public class TimeZoneResource
{
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? Location { get; init; }
    public TimeZoneInfo TimeZone { get; init; } = null!;

    public string BaseOffset => Helpers.TimeZoneIsUtc(Id)
        ? "UTC"
        : $"UTC{TimeZone.BaseUtcOffset.FormatAsOffset()}";
    
    public string CurrentOffset => Helpers.TimeZoneIsUtc(Id)
        ? "UTC"
        : $"UTC{TimeZone.GetUtcOffset(DateTime.UtcNow).FormatAsOffset()}";

    public override string ToString()
    {
        var sb = new StringBuilder($"({BaseOffset}) {Name}");

        if (Location != null)
        {
            sb.Append($" - {Location}");
        }

        if (BaseOffset != CurrentOffset)
        {
            sb.Append($" (currently {CurrentOffset})");
        }

        return sb.ToString();
    }
}