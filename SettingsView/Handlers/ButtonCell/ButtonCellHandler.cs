using System;
namespace AiForms.Settings.Handlers;

public partial class ButtonCellHandler
{
    public ButtonCellHandler() : base(ButtonMapper)
    {
    }

    public ButtonCellHandler(IPropertyMapper mapper = null) : base(mapper ?? ButtonMapper)
    {
    }
}

