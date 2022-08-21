using System;
namespace AiForms.Settings.Handlers;

public partial class EntryCellHandler
{
    public EntryCellHandler() : base(EntryMapper)
    {
    }

    public EntryCellHandler(IPropertyMapper mapper = null) : base(mapper ?? EntryMapper)
    {
    }
}

