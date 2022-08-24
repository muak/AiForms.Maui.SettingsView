using System;
using AiForms.Settings.Platforms.Droid;
using Android.Bluetooth;

namespace AiForms.Settings.Handlers;

public partial class CommandCellHandler : LabelCellBaseHandler<CommandCell, CommandCellView>
{
    public static IPropertyMapper<CommandCell, CommandCellHandler> CommandMapper =
        new PropertyMapper<CommandCell, CommandCellHandler>(LabelMapper)
        {
            [nameof(CommandCell.Command)] = MapCommand,
            [nameof(CommandCell.CommandParameter)] = MapCommand,
        };

    private static void MapCommand(CommandCellHandler handler, CommandCell cell)
    {
        handler.PlatformView.UpdateCommand();
    }
}

