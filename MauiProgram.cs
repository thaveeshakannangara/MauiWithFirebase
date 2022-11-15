namespace MauiApp13;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton(typeof(IDataStore<>), typeof(FirebaseDataStore <>));
		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<BranchViewModel>();	
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<BranchView>();
		return builder.Build();
	}
}
