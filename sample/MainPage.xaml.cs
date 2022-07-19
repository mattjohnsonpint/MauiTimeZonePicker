namespace MauiTimeZonePickerSampleApp;

public partial class MainPage
{
	private IDispatcherTimer _timer;

	public MainPage()
	{
		InitializeComponent();
	}

	~MainPage()
	{
		_timer.Tick -= OnTimerTick;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		
		// TODO: The timer is a bit sluggish on Android while scrolling the list of time zones.  Figure out why.
		_timer = Dispatcher.CreateTimer();
		_timer.Interval = TimeSpan.FromSeconds(0.1);
		_timer.Tick += OnTimerTick;
		_timer.Start();
	}

	private void OnTimerTick(object sender, EventArgs e)
	{
		UpdateCurrentTimeText();
	}

	private void TimeZoneChanged(object sender, TimeZonePicker.SelectedItemChangedEventArgs e)
	{
		UpdateCurrentTimeText();

		if (e.CurrentSelection is { } selectedTimeZone)
		{
			SelectedTimeZoneIdText.Text = $"Time Zone ID: {selectedTimeZone.Id} ";
			SelectedTimeZoneIdText.IsVisible = true;
		}
		else
		{
			SelectedTimeZoneIdText.IsVisible = false;
		}
	}

	private void UpdateCurrentTimeText()
	{
		if (TimeZonePicker.SelectedItem is { } selectedTimeZone)
		{
			var now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, selectedTimeZone.TimeZone);
			CurrentTimeText.Text = $"It is {now:F}\nin {selectedTimeZone.Location ?? selectedTimeZone.Name}";
			CurrentTimeText.IsVisible = true;
		}
		else
		{
			CurrentTimeText.IsVisible = false;
		}
	}
}

