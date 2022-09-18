using System;
namespace AiForms.Settings.Handlers;

public partial class ButtonCellHandler
{
    public static CommandMapper<ButtonCell, ButtonCellHandler> ButtonCommandMapper = new(BaseCommandMapper);  

    public ButtonCellHandler() : base(ButtonMapper)
    {
    }

    public ButtonCellHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? ButtonMapper, commandMapper ?? ButtonCommandMapper)
    {
    }
}

