using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers;

public partial class RadioCellHandler : CellBaseHandler<RadioCell, RadioCellView>
{
    public static IPropertyMapper<RadioCell, RadioCellHandler> RadioMapper =
        new PropertyMapper<RadioCell, RadioCellHandler>(BasePropertyMapper)
        {
            [nameof(RadioCell.AccentColor)] = MapAccentColor,
        };

    private static void MapAccentColor(RadioCellHandler handler, RadioCell arg2)
    {
        handler.PlatformView.UpdateAccentColor();
    }
}

