using System;
namespace AiForms.Settings.Handlers;

public partial class TimePickerCellHandler
{
    public TimePickerCellHandler() : base(TimePickerMapper)
    {
    }

    public TimePickerCellHandler(IPropertyMapper mapper = null) : base(mapper ?? TimePickerMapper)
    {
    }
}

