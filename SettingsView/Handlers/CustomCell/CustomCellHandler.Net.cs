using System;
namespace AiForms.Settings.Handlers;

public partial class CustomCellHandler : CellBaseHandler<CustomCell, CellBaseView>
{
    public static IPropertyMapper<CustomCell, CustomCellHandler> CustomMapper =
        new PropertyMapper<CustomCell, CustomCellHandler>(BasePropertyMapper);
}

