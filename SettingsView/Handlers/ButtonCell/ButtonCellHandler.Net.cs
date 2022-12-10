using System;
namespace AiForms.Settings.Handlers;

public partial class ButtonCellHandler : CellBaseHandler<ButtonCell, CellBaseView>
{
    public static IPropertyMapper<ButtonCell, ButtonCellHandler> ButtonMapper =
        new PropertyMapper<ButtonCell, ButtonCellHandler>(BasePropertyMapper)
        {
        };
}

