using System;
namespace AiForms.Settings.Handlers;

public partial class SwitchCellHandler
{
    public static CommandMapper<SwitchCell, SwitchCellHandler> SwitchCommandMapper = new(BaseCommandMapper);

    public SwitchCellHandler() : base(SwitchMapper)
    {
    }

    public SwitchCellHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? SwitchMapper, commandMapper ?? SwitchCommandMapper)
    {
    }
}

