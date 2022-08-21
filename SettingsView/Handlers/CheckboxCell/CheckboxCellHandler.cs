using System;
namespace AiForms.Settings.Handlers;

public partial class CheckboxCellHandler
{
    public CheckboxCellHandler() : base(CheckboxMapper)
    {
    }

    public CheckboxCellHandler(IPropertyMapper mapper = null) : base(mapper ?? CheckboxMapper)
    {
    }
}

