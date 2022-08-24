using System;
namespace AiForms.Settings.Handlers;

public partial class NumberPickerCellHandler
{
    public NumberPickerCellHandler() : base(NumberPickerMapper)
    {
    }

    public NumberPickerCellHandler(IPropertyMapper mapper = null) : base(mapper ?? NumberPickerMapper)
    {
    }
}

