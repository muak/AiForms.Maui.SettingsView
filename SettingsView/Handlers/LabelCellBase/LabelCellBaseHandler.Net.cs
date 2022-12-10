using System;
namespace AiForms.Settings.Handlers;

public partial class LabelCellBaseHandler<TvirtualCell, TnativeCell>
{
    public static IPropertyMapper<LabelCell, LabelCellBaseHandler<TvirtualCell, TnativeCell>> LabelMapper =
    new PropertyMapper<LabelCell, LabelCellBaseHandler<TvirtualCell, TnativeCell>>(BasePropertyMapper);
}

