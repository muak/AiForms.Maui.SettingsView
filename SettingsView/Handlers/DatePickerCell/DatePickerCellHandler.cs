using System;
namespace AiForms.Settings.Handlers;

public partial class DatePickerCellHandler
{
    public DatePickerCellHandler() : base(DatePickerMapper)
    {
    }

    public DatePickerCellHandler(IPropertyMapper mapper = null) : base(mapper ?? DatePickerMapper)
    {
    }
}

