using System;
namespace AiForms.Settings.Handlers;

public partial class RadioCellHandler
{
    public static CommandMapper<RadioCell, RadioCellHandler> RadioCommandMapper = new(BaseCommandMapper);

    public RadioCellHandler() : base(RadioMapper, RadioCommandMapper)
    {
    }

    public RadioCellHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? RadioMapper, commandMapper ?? RadioCommandMapper)
    {
    }
}

