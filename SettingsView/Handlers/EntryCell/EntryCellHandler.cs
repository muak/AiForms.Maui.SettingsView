using System;
namespace AiForms.Settings.Handlers;

public partial class EntryCellHandler
{   

    public EntryCellHandler() : base(EntryMapper, EntryCommandMapper)
    {
    }

    public EntryCellHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? EntryMapper, commandMapper ?? EntryCommandMapper)
    {
    }
}

