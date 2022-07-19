using System.Text;

namespace MauiTimeZonePicker;

public class TimeZoneResource
{
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? Location { get; init; }
    public TimeZoneInfo TimeZone { get; init; } = null!;

    public string BaseOffset => Id.TimeZoneIsUtc()
        ? "UTC"
        : $"UTC{TimeZone.BaseUtcOffset.FormatAsOffset()}";
    
    public string CurrentOffset => Id.TimeZoneIsUtc()
        ? "UTC"
        : $"UTC{TimeZone.GetUtcOffset(DateTime.UtcNow).FormatAsOffset()}";

    public override string ToString()
    {
        var sb = new StringBuilder();
        
        sb.AppendLine(Name);

        sb.Append($"({CurrentOffset})");

        if (Location != null)
        {
            sb.Append($" {Location}");
        }

        sb.AppendLine();

        return sb.ToString();
    }
}