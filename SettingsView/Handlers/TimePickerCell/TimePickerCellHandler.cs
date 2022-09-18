using System;
namespace AiForms.Settings.Handlers;

public partial class TimePickerCellHandler
{
    public static CommandMapper<TimePickerCell, TimePickerCellHandler> TimePickerCommandMapper = new(LabelCommandMapper);

    public TimePickerCellHandler() : base(TimePickerMapper)
    {
    }

    public TimePickerCellHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? TimePickerMapper, commandMapper ?? TimePickerCommandMapper)
    {
    }
}

