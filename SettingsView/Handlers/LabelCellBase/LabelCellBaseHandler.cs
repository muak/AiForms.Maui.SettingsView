using System;
#if IOS || MACCATALYST
using LabelCellView = AiForms.Settings.Platforms.iOS.LabelCellView;
#elif __ANDROID__
using LabelCellView = AiForms.Settings.Platforms.Droid.LabelCellView;
#elif NET6_0_OR_GREATER
using LabelCellView = AiForms.Settings.Handlers.CellBaseView;
#endif


namespace AiForms.Settings.Handlers;

public partial class LabelCellBaseHandler<TvirtualCell, TnativeCell> : CellBaseHandler<TvirtualCell, TnativeCell>
        where TvirtualCell : LabelCell
        where TnativeCell : LabelCellView
{
    public static CommandMapper<LabelCell, LabelCellBaseHandler<TvirtualCell, TnativeCell>> LabelCommandMapper = new(BaseCommandMapper);

    public LabelCellBaseHandler(): base(LabelMapper, LabelCommandMapper)
    {
    }

    public LabelCellBaseHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base (mapper ?? LabelMapper, commandMapper ?? LabelCommandMapper)
    {
    }
}

