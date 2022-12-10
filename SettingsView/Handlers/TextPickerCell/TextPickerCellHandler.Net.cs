using System;
namespace AiForms.Settings.Handlers;

public partial class TextPickerCellHandler : LabelCellBaseHandler<TextPickerCell, CellBaseView>
{
    public static IPropertyMapper<TextPickerCell, TextPickerCellHandler> TextPickerMapper =
        new PropertyMapper<TextPickerCell, TextPickerCellHandler>(LabelMapper);
}

