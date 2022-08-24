using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers;

public partial class CheckboxCellHandler : CellBaseHandler<CheckboxCell, CheckboxCellView>
{
    public static IPropertyMapper<CheckboxCell, CheckboxCellHandler> CheckboxMapper =
        new PropertyMapper<CheckboxCell, CheckboxCellHandler>(BasePropertyMapper)
        {
            [nameof(CheckboxCell.AccentColor)] = MapAccentColor,
            [nameof(CheckboxCell.Checked)] = MapChecked,
        };

    private static void MapAccentColor(CheckboxCellHandler handler, CheckboxCell arg2)
    {
        handler.PlatformView.UpdateAccentColor();
    }

    private static void MapChecked(CheckboxCellHandler handler, CheckboxCell arg2)
    {
        handler.PlatformView.UpdateChecked();
    }
}

