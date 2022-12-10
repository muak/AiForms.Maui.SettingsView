using System;
namespace AiForms.Settings.Handlers;

public partial class EntryCellHandler : CellBaseHandler<EntryCell, CellBaseView>
{
    public static IPropertyMapper<EntryCell, EntryCellHandler> EntryMapper =
        new PropertyMapper<EntryCell, EntryCellHandler>(BasePropertyMapper);
}

