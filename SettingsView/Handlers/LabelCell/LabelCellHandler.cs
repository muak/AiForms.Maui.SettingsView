using System;
namespace AiForms.Settings.Handlers;

public partial class LabelCellHandler
{
    public LabelCellHandler() : base(LabelMapper)
    {
    }

    public LabelCellHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? LabelMapper, commandMapper ?? LabelCommandMapper)
    {
    }
}

