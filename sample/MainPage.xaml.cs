namespace MauiTimeZonePickerSampleApp;

public partial class MainPage : ContentPage
{
	private TimeZoneResource _selectedTimeZone;
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
		
		_timer = Dispatcher.CreateTimer();
		_timer.Interval = TimeSpan.FromSeconds(0.1);
		_timer.Tick += OnTimerTick;
		_timer.Start();
	}

	private void OnTimerTick(object sender, EventArgs e)
	{
		UpdateCurrentTimeText();
	}

	private void TimeZoneChanged(object sender, EventArgs e)
	{
		var picker = (TimeZonePicker) sender;
		_selectedTimeZone = (TimeZoneResource) picker.SelectedItem;
		
		SelectedTimeZoneIdText.Text = $"Time Zone ID: {_selectedTimeZone.Id} ";
		SelectedTimeZoneIdText.IsVisible = true;
	}

	private void UpdateCurrentTimeText()
	{
		if (_selectedTimeZone == null)
		{
			CurrentTimeText.IsVisible = false;
			return;
		}

		var now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _selectedTimeZone.TimeZone);
		CurrentTimeText.Text = $"It is {now:F}\nin {_selectedTimeZone.Location ?? _selectedTimeZone.Name}";
		CurrentTimeText.IsVisible = true;
	}
}

