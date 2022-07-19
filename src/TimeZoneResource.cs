namespace MauiTimeZonePicker;

public class TimeZoneResource
{
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Location { get; init; } = null!;
    public TimeZoneInfo TimeZoneInfo { get; init; } = null!;
}