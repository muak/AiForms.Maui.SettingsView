using System;
namespace AiForms.Settings.Handlers;

public partial class CheckboxCellHandler: CellBaseHandler<CheckboxCell, CellBaseView>
{
    public static IPropertyMapper<CheckboxCell, CheckboxCellHandler> CheckboxMapper =
        new PropertyMapper<CheckboxCell, CheckboxCellHandler>(BasePropertyMapper);
}

