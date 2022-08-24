using System;
namespace AiForms.Settings.Handlers;

public partial class SwitchCellHandler
{
    public SwitchCellHandler() : base(SwitchMapper)
    {
    }

    public SwitchCellHandler(IPropertyMapper mapper = null) : base(mapper ?? SwitchMapper)
    {
    }
}

