using System;
namespace AiForms.Settings.Handlers;

public partial class EntryCellBaseHandler<TvirtualCell, TnativeCell>
{
    public static IPropertyMapper<EntryCell, EntryCellBaseHandler<TvirtualCell,TnativeCell>> EntryMapper =
        new PropertyMapper<EntryCell, EntryCellBaseHandler<TvirtualCell, TnativeCell>>(BasePropertyMapper);
}

