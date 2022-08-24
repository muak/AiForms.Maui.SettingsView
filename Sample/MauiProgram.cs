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
                })
                .OnAppStart(navigationService =>
                {
                    navigationService.CreateBuilder()
                    .AddSegment(nameof(MyNavigationPage))
                    .AddSegment<MainViewModel>()
                    .Navigate();
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

        ViewModelLocationProvider2.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
        {              
            // Hoge     -> HogeViewModel
            // HogePage -> HogeViewModel
            // HogeView -> HogeViewModel
            var viewName = viewType.FullName;
            viewName = viewName.Replace(".Views.", ".ViewModels.");
            viewName = viewName.EndsWith("Page") ? viewName.Replace("Page", "") : viewName;
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var suffix = viewName.EndsWith("View") ? "Model" : "ViewModel";
            var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", viewName, suffix, viewAssemblyName);
            return Type.GetType(viewModelName);
        });

        return builder.Build();
    }
}

