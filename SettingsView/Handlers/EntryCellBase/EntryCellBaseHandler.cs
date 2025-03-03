using System;
#if IOS || MACCATALYST
using EntryCellView = AiForms.Settings.Platforms.iOS.EntryCellView;
#elif __ANDROID__
using EntryCellView = AiForms.Settings.Platforms.Droid.EntryCellView;
#elif NET6_0_OR_GREATER
using EntryCellView = AiForms.Settings.Handlers.CellBaseView;
#endif

namespace AiForms.Settings.Handlers;

public partial class EntryCellBaseHandler<TvirtualCell, TnativeCell>: CellBaseHandler<TvirtualCell, TnativeCell>
    where TvirtualCell: EntryCell
    where TnativeCell: EntryCellView
{
    public static CommandMapper<EntryCell, EntryCellBaseHandler<TvirtualCell, TnativeCell>> EntryCommandMapper = new(BaseCommandMapper);

    public EntryCellBaseHandler() : base(EntryMapper, EntryCommandMapper)
    {
    }

    public EntryCellBaseHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? EntryMapper, commandMapper ?? EntryCommandMapper)
    {
    }
}

