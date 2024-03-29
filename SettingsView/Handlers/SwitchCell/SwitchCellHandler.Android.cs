﻿using System;
using AiForms.Settings.Platforms.Droid;

namespace AiForms.Settings.Handlers;

public partial class SwitchCellHandler : CellBaseHandler<SwitchCell, SwitchCellView>
{
    public static IPropertyMapper<SwitchCell, SwitchCellHandler> SwitchMapper =
        new PropertyMapper<SwitchCell, SwitchCellHandler>(BasePropertyMapper)
        {
            [nameof(SwitchCell.AccentColor)] = MapAccentColor,
            [nameof(SwitchCell.On)] = MapOn
        };

    private static void MapAccentColor(SwitchCellHandler handler, SwitchCell arg2)
    {
        handler.PlatformView.UpdateAccentColor();
    }

    private static void MapOn(SwitchCellHandler handler, SwitchCell arg2)
    {
        handler.PlatformView.UpdateOn();
    }
}

