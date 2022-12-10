using System;
namespace AiForms.Settings.Handlers;

public partial class SwitchCellHandler : CellBaseHandler<SwitchCell, CellBaseView>
{
    public static IPropertyMapper<SwitchCell, SwitchCellHandler> SwitchMapper =
        new PropertyMapper<SwitchCell, SwitchCellHandler>(BasePropertyMapper);
}

