using System;
namespace AiForms.Settings.Handlers;

public partial class CommandCellHandler
{
    public CommandCellHandler() : base(CommandMapper)
    {
    }

    public CommandCellHandler(IPropertyMapper mapper = null) : base(mapper ?? CommandMapper)
    {
    }
}

