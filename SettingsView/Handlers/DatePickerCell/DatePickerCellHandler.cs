using System;
namespace AiForms.Settings.Handlers;

public partial class DatePickerCellHandler
{
    public static CommandMapper<CommandCell, CommandCellHandler> DatePickerCommandMapper = new(LabelCommandMapper);

    public DatePickerCellHandler() : base(DatePickerMapper)
    {
    }

    public DatePickerCellHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? DatePickerMapper, commandMapper ?? DatePickerCommandMapper)
    {
    }
}

