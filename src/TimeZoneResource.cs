namespace MauiTimeZonePicker;

public class TimeZoneResource
{
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Location { get; init; } = null!;
    public TimeZoneInfo TimeZone { get; init; } = null!;

    public string BaseOffset => Helpers.TimeZoneIsUtc(Id)
        ? "UTC"
        : $"UTC{TimeZone.BaseUtcOffset.FormatAsOffset()}";
    
    public string CurrentOffset => Helpers.TimeZoneIsUtc(Id)
        ? "UTC"
        : $"UTC{TimeZone.GetUtcOffset(DateTime.UtcNow).FormatAsOffset()}";

    public override string ToString()
    {
        var baseOffset = BaseOffset;
        var currentOffset = CurrentOffset;
        var extraInfo = baseOffset != currentOffset ? $" (currently {currentOffset})" : "";
        
        return $"({BaseOffset}) {Name} - {Location}{extraInfo}";
    }
}