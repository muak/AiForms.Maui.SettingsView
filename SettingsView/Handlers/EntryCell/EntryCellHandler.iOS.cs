using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers;

public partial class EntryCellHandler : CellBaseHandler<EntryCell, EntryCellView>
{
    public static IPropertyMapper<EntryCell, CellBaseHandler<EntryCell, EntryCellView>> EntryMapper =
        new PropertyMapper<EntryCell, CellBaseHandler<EntryCell, EntryCellView>>(BasePropertyMapper)
        {
        };
}

