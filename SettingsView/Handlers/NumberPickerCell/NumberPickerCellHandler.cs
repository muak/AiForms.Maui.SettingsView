using System;
namespace AiForms.Settings.Handlers;

public partial class NumberPickerCellHandler
{
    public static CommandMapper<NumberPickerCell, NumberPickerCellHandler> NumberPickerCommandMapper = new(LabelCommandMapper);

    public NumberPickerCellHandler() : base(NumberPickerMapper)
    {
    }

    public NumberPickerCellHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? NumberPickerMapper, commandMapper ?? NumberPickerCommandMapper)
    {
    }
}

