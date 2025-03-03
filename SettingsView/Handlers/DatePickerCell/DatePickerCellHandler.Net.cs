using System;
namespace AiForms.Settings.Handlers;

public partial class DatePickerCellHandler : LabelCellBaseHandler<DatePickerCell, CellBaseView>
{
    public static IPropertyMapper<DatePickerCell, DatePickerCellHandler> DatePickerMapper =
        new PropertyMapper<DatePickerCell, DatePickerCellHandler>(LabelMapper);
}

