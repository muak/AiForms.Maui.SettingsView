using System;
namespace AiForms.Settings.Handlers;

public partial class CellBaseHandler<TvirtualCell, TnativeCell>
{
    public static IPropertyMapper<CellBase, CellBaseHandler<TvirtualCell, TnativeCell>> BasePropertyMapper =
            new PropertyMapper<CellBase, CellBaseHandler<TvirtualCell, TnativeCell>>(ElementMapper);

    protected override TnativeCell CreatePlatformElement()
    {
        throw new NotImplementedException();
    }
}

