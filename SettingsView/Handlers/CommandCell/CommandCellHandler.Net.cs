using System;
namespace AiForms.Settings.Handlers;

public partial class CommandCellHandler : LabelCellBaseHandler<CommandCell, CellBaseView>
{
    public static IPropertyMapper<CommandCell, CommandCellHandler> CommandMapper =
        new PropertyMapper<CommandCell, CommandCellHandler>(LabelMapper);
}

