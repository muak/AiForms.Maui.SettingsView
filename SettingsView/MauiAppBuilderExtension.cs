using System;
namespace AiForms.Settings;

public static class MauiAppBuilderExtension
{
    public static MauiAppBuilder UseSettingsView(this MauiAppBuilder builder, bool shouldCallDisconnectHandlerWhenPageUnloaded = false)
    {
        builder.ConfigureMauiHandlers(handler =>
        {
            handler.AddSettingsViewHandler();
        });

        SettingsViewConfiguration.ShouldAutoDisconnect = shouldCallDisconnectHandlerWhenPageUnloaded;

        return builder;
    }
}

