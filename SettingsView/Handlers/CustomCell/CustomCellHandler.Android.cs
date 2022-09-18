using System;
using AiForms.Settings.Platforms.Droid;
using Android.Bluetooth;

namespace AiForms.Settings.Handlers;

public partial class CustomCellHandler : CellBaseHandler<CustomCell, CustomCellView>
{
    public static IPropertyMapper<CustomCell, CellBaseHandler<CustomCell, CustomCellView>> CustomMapper =
        new PropertyMapper<CustomCell, CellBaseHandler<CustomCell, CustomCellView>>(BasePropertyMapper)
        {
            [nameof(CustomCell.Command)] = MapCommand,
            [nameof(CustomCell.CommandParameter)] = MapCommand,
        };

    private static void MapCommand(CellBaseHandler<CustomCell, CustomCellView> handler, CustomCell arg2)
    {
        handler.PlatformView.UpdateCommand();
    }
}

