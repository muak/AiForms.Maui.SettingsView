using System;
namespace AiForms.Settings.Handlers;

public partial class NumberPickerCellHandler : LabelCellBaseHandler<NumberPickerCell, CellBaseView>
{
    public static IPropertyMapper<NumberPickerCell, NumberPickerCellHandler> NumberPickerMapper =
        new PropertyMapper<NumberPickerCell, NumberPickerCellHandler>(LabelMapper);
}

