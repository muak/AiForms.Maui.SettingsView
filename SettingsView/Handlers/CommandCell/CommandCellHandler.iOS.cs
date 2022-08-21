using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers;

public partial class CommandCellHandler: CellBaseHandler<CommandCell, CommandCellView>
{
    public static IPropertyMapper<CommandCell, CellBaseHandler<CommandCell, CommandCellView>> CommandMapper =
        new PropertyMapper<CommandCell, CellBaseHandler<CommandCell, CommandCellView>>(BasePropertyMapper)
        {
            [nameof(CommandCell.Command)] = MapCommand,
            [nameof(CommandCell.CommandParameter)] = MapCommand,
        };

    private static void MapCommand(CellBaseHandler<CommandCell, CommandCellView> handler, CommandCell cell)
    {
        handler.PlatformView.UpdateCommand();
    }
}

