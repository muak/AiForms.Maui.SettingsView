﻿using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers;

public partial class CommandCellHandler: LabelCellBaseHandler<CommandCell, CommandCellView>
{
    public static IPropertyMapper<CommandCell, CommandCellHandler> CommandMapper =
        new PropertyMapper<CommandCell, CommandCellHandler>(LabelMapper)
        {
            [nameof(CommandCell.Command)] = MapCommand,
            [nameof(CommandCell.CommandParameter)] = MapCommand,
        };

    private static void MapCommand(CommandCellHandler handler, CommandCell cell)
    {
        if (handler.IsDisconnect) return;
        handler.PlatformView.UpdateCommand();
    }
}

