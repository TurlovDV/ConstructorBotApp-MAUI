using CommunityToolkit.Maui;
using ConstructorBot.ViewModel.ConstructorPageViewModel;
using ConstructorBot.ViewModel.ConstructorPageViewModel.Action;
using ConstructorBot.ViewModel.MainPageViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace ConstructorBot;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

        //builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<MainViewModel>();

        builder.Services.AddSingleton<ConstructorViewModel>();

#if ANDROID
			builder.Services.AddSingleton<IServiceDomainBot, ConstructorBot.ServiceDomainBot>();
#endif

		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
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
			});


        //builder.Services.AddSingleton<List<ActionBox>>();

        //builder.Services.AddScoped<ConstructorViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
	}

}
