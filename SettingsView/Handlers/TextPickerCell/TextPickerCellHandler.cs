using System;
namespace AiForms.Settings.Handlers;

public partial class TextPickerCellHandler
{
    public TextPickerCellHandler() : base(TextPickerMapper)
    {
    }

    public TextPickerCellHandler(IPropertyMapper mapper = null) : base(mapper ?? TextPickerMapper)
    {
    }
}

