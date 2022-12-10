using System;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Handlers;

namespace AiForms.Settings.Handlers;

public partial class SettingsViewHandler : ViewHandler<SettingsView, View>
{
    public static IPropertyMapper<SettingsView, SettingsViewHandler> Mapper =
    new PropertyMapper<SettingsView, SettingsViewHandler>(ViewMapper);

    protected override View CreatePlatformView()
    {
        throw new NotImplementedException();
    }
}

