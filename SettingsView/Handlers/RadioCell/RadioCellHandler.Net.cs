using System;
namespace AiForms.Settings.Handlers;

public partial class RadioCellHandler : CellBaseHandler<RadioCell, CellBaseView>
{
    public static IPropertyMapper<RadioCell, RadioCellHandler> RadioMapper =
        new PropertyMapper<RadioCell, RadioCellHandler>(BasePropertyMapper);
}

