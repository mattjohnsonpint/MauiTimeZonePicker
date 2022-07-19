namespace MauiTimeZonePickerSampleApp;

public partial class MainPage : ContentPage
{
	private TimeZoneResource _selectedTimeZone;

	public MainPage()
	{
		InitializeComponent();
	}

	private void TimeZoneChanged(object sender, EventArgs e)
	{
		var picker = (TimeZonePicker) sender;
		_selectedTimeZone = (TimeZoneResource) picker.SelectedItem;
		UpdateCurrentTimeText();
	}

	private void UpdateCurrentTimeText()
	{
		if (_selectedTimeZone == null)
		{
			CurrentTimeText.IsVisible = false;
			return;
		}

		var now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _selectedTimeZone.TimeZone);
		CurrentTimeText.Text = $"It is {now:F} in {_selectedTimeZone.Location ?? _selectedTimeZone.Name}";
		CurrentTimeText.IsVisible = true;
	}
}

