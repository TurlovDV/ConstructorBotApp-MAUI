using CommunityToolkit.Maui;
using ConstructorBot.ViewModel.ConstructorPageViewModel;
using ConstructorBot.ViewModel.MainPageViewModel;
using ConstructorBotMessengerApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.PlatformConfiguration;
using ConstructorBot.Services.ServiceStorage;
using static ConstructorBot.App;
using ConstructorBot.Services.PopupService;
using ConstructorBot.Services;
using Mopups.Hosting;
using System.Net;
using Plugin.LocalNotification;

namespace ConstructorBot;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();      

        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<ConstructorViewModel>();
		builder.Services.AddSingleton<ConstructorBotMessengerApi.IMessengerService, MessengerService>();
        builder.Services.AddSingleton<IStorageService, FileStorageService>();
        builder.Services.AddSingleton<IMessageService, MopupsService>();
        builder.Services.AddSingleton<IBuildMessengerService, BuildMessengerService>();
#if ANDROID
		builder.Services.AddSingleton<IBackgroundWorkService, ConstructorBot.BackgroundWorkService>();
#endif
        builder
            .UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseLocalNotification()
			.UseSentry(options =>
			{
				options.Dsn = "https://8536cd6308be41458da7a34b5635d6f2@o4505274608648192.ingest.sentry.io/4505274617364480";
            })
			.ConfigureMopups()
			.ConfigureMauiHandlers(handlers =>
			{
#if ANDROID
                handlers.AddHandler(typeof(Entry), typeof(ConstructorBot.Platforms.Android.MyEntryHandler));
#endif
            })
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Shentox-Regular.ttf", "ShentoxRegular");               
                fonts.AddFont("Srbija-Sans.otf", "SrbijaSans");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
	}

}
