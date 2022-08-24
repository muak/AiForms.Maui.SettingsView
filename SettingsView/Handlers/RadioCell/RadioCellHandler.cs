using System;
namespace AiForms.Settings.Handlers;

public partial class RadioCellHandler
{
    public RadioCellHandler() : base(RadioMapper)
    {
    }

    public RadioCellHandler(IPropertyMapper mapper = null) : base(mapper ?? RadioMapper)
    {
    }
}

