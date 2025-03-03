﻿using System;
using AiForms.Settings.Platforms.Droid;
using Android.Bluetooth;

namespace AiForms.Settings.Handlers;

public partial class CustomCellHandler : CellBaseHandler<CustomCell, CustomCellView>
{
    public static IPropertyMapper<CustomCell, CustomCellHandler> CustomMapper =
        new PropertyMapper<CustomCell, CustomCellHandler>(BasePropertyMapper)
        {
            [nameof(CustomCell.Command)] = MapCommand,
            [nameof(CustomCell.CommandParameter)] = MapCommand,
        };

    private static void MapCommand(CustomCellHandler handler, CustomCell arg2)
    {
        handler.PlatformView.UpdateCommand();
    }
}

