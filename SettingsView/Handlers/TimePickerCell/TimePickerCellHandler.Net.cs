using System;
namespace AiForms.Settings.Handlers;

public partial class TimePickerCellHandler : LabelCellBaseHandler<TimePickerCell, CellBaseView>
{
    public static IPropertyMapper<TimePickerCell, TimePickerCellHandler> TimePickerMapper =
        new PropertyMapper<TimePickerCell, TimePickerCellHandler>(LabelMapper);
}