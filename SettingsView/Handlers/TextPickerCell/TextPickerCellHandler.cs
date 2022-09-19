using System;
namespace AiForms.Settings.Handlers;

public partial class TextPickerCellHandler
{
    public static CommandMapper<TextPickerCell, TextPickerCellHandler> TextPickerCommandMapper = new(LabelCommandMapper);

    public TextPickerCellHandler() : base(TextPickerMapper, TextPickerCommandMapper)
    {
    }

    public TextPickerCellHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? TextPickerMapper, commandMapper ?? TextPickerCommandMapper)
    {
    }
}

