using System.Globalization;
using System.Reflection;
using AiForms.Settings;
using Microsoft.Maui;
using Prism;
using Prism.Navigation;
using Sample.ViewModels;
using Sample.Views;

namespace Sample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()            
            .UsePrism(prism =>
            {
                prism.RegisterTypes(containerRegistry =>
                {
                    containerRegistry.RegisterForNavigation<MyNavigationPage>();
                    containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();
                    containerRegistry.RegisterForNavigation<ContentPage>();
                    containerRegistry.RegisterForNavigation<ListPage, ListViewModel>();
                    containerRegistry.RegisterForNavigation<CustomHeaderPage, CustomHeaderViewModel>();
                    containerRegistry.RegisterForNavigation<TapSurveyPage, TapSurveyViewModel>();
                })
                .OnAppStart(async(container, navigation) =>
                {
                    await navigation.CreateBuilder()
                    .AddSegment(nameof(MyNavigationPage))
                    .AddSegment<MainViewModel>()
                    .NavigateAsync();
                })
                .OnInitialized(container =>
                {
                    ;
                });
            })
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddSettingsViewHandler();
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });        

        return builder.Build();
    }
}

