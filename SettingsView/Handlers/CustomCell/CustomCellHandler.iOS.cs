using System;
using AiForms.Settings.Platforms.iOS;

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
        if (handler.IsDisconnect) return;
        handler.PlatformView.UpdateCommand();
    }
}

