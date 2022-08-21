using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers;

public partial class CheckboxCellHandler : CellBaseHandler<CheckboxCell, CheckboxCellView>
{
    public static IPropertyMapper<CheckboxCell, CellBaseHandler<CheckboxCell, CheckboxCellView>> CheckboxMapper =
        new PropertyMapper<CheckboxCell, CellBaseHandler<CheckboxCell, CheckboxCellView>>(BasePropertyMapper)
        {
            [nameof(CheckboxCell.AccentColor)] = MapAccentColor,
            [nameof(CheckboxCell.Checked)] = MapChecked,
        };

    private static void MapAccentColor(CellBaseHandler<CheckboxCell, CheckboxCellView> handler, CheckboxCell arg2)
    {
        handler.PlatformView.UpdateAccentColor();
    }

    private static void MapChecked(CellBaseHandler<CheckboxCell, CheckboxCellView> handler, CheckboxCell arg2)
    {
        handler.PlatformView.UpdateChecked();
    }
}

