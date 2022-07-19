namespace MauiTimeZonePickerSampleApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSentry(options =>
			{
				// This DSN is specifically for this sample application
				options.Dsn = "https://78b4454330094076b5dbf4e064916caa@o1162446.ingest.sentry.io/6583452";
			})
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		return builder.Build();
	}
}
