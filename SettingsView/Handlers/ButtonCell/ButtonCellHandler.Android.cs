using System;
using AiForms.Settings.Platforms.Droid;

namespace AiForms.Settings.Handlers;

public partial class ButtonCellHandler : CellBaseHandler<ButtonCell, ButtonCellView>
{
    public static IPropertyMapper<ButtonCell, CellBaseHandler<ButtonCell, ButtonCellView>> ButtonMapper =
        new PropertyMapper<ButtonCell, CellBaseHandler<ButtonCell, ButtonCellView>>(BasePropertyMapper)
        {
            [nameof(ButtonCell.Command)] = MapCommand,
            [nameof(ButtonCell.CommandParameter)] = MapCommand,
            [nameof(ButtonCell.TitleAlignment)] = MapTitleAlignment,
        };

    private static void MapTitleAlignment(CellBaseHandler<ButtonCell, ButtonCellView> handler, ButtonCell arg2)
    {
        handler.PlatformView.UpdateTitleAlignment();
    }

    private static void MapCommand(CellBaseHandler<ButtonCell, ButtonCellView> handoer, ButtonCell arg2)
    {
        handoer.PlatformView.UpdateCommand();
    }
}

