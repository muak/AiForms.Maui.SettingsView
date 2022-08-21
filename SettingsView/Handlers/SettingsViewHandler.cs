using System;
using Microsoft.Maui;

namespace AiForms.Settings.Handlers;

public partial class SettingsViewHandler
{   
    public SettingsViewHandler(): base(Mapper)
    {
    }

    public SettingsViewHandler(IPropertyMapper mapper = null) : base(mapper ?? Mapper)
    {
    }
}

