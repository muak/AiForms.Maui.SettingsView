using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers;

public partial class ButtonCellHandler : CellBaseHandler<ButtonCell, ButtonCellView>
{
    public static IPropertyMapper<ButtonCell, ButtonCellHandler> ButtonMapper =
        new PropertyMapper<ButtonCell, ButtonCellHandler>(BasePropertyMapper)
        {
            [nameof(ButtonCell.Command)] = MapCommand,
            [nameof(ButtonCell.CommandParameter)] = MapCommand,
            [nameof(ButtonCell.TitleAlignment)] = MapTitleAlignment,
        };

    private static void MapTitleAlignment(ButtonCellHandler handler, ButtonCell arg2)
    {
        handler.PlatformView.UpdateTitleAlignment();
    }

    private static void MapCommand(ButtonCellHandler handoer, ButtonCell arg2)
    {
        handoer.PlatformView.UpdateCommand();
    }
}

