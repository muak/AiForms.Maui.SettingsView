using System;
namespace AiForms.Settings.Handlers;

public partial class CommandCellHandler
{
    public static CommandMapper<CommandCell, CommandCellHandler> CommandCommandMapper = new(LabelCommandMapper);

    public CommandCellHandler() : base(CommandMapper)
    {
    }

    public CommandCellHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? CommandMapper, commandMapper ?? CommandCommandMapper)
    {
    }
}

