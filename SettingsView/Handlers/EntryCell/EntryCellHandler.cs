using System;
namespace AiForms.Settings.Handlers;

public partial class EntryCellHandler
{
    public static CommandMapper<EntryCell, EntryCellHandler> EntryCommandMapper = new(BaseCommandMapper);

    public EntryCellHandler() : base(EntryMapper)
    {
    }

    public EntryCellHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? EntryMapper, commandMapper ?? EntryCommandMapper)
    {
    }
}

