using System;
using AiForms.Settings.Platforms.Droid;

namespace AiForms.Settings.Handlers;

public partial class LabelCellHandler: CellBaseHandler<LabelCell, LabelCellView>
{
    public static IPropertyMapper<LabelCell, CellBaseHandler<LabelCell, LabelCellView>> LabelMapper =
        new PropertyMapper<LabelCell, CellBaseHandler<LabelCell, LabelCellView>>(BasePropertyMapper)
        {
            [nameof(LabelCell.ValueText)] = MapValueText,
            [nameof(LabelCell.ValueTextColor)] = MapValueTextColor,
            [nameof(LabelCell.ValueTextFontSize)] = MapValueTextFontSize,
            [nameof(LabelCell.ValueTextFontAttributes)] = MapValueTextFont,
            [nameof(LabelCell.ValueTextFontFamily)] = MapValueTextFont,
            [nameof(LabelCell.IgnoreUseDescriptionAsValue)] = MapUseDescriptionAsValue,
        };    

    private static void MapValueText(CellBaseHandler<LabelCell, LabelCellView> handler, LabelCell cell)
    {
        handler.PlatformView.UpdateValueText();
    }

    private static void MapValueTextColor(CellBaseHandler<LabelCell, LabelCellView> handler, LabelCell cell)
    {
        handler.PlatformView.UpdateValueTextColor();
    }

    private static void MapValueTextFontSize(CellBaseHandler<LabelCell, LabelCellView> handler, LabelCell cell)
    {
        handler.PlatformView.UpdateValueTextFontSize();
    }

    private static void MapValueTextFont(CellBaseHandler<LabelCell, LabelCellView> handler, LabelCell cell)
    {
        handler.PlatformView.UpdateValueTextFont();
    }

    private static void MapUseDescriptionAsValue(CellBaseHandler<LabelCell, LabelCellView> handler, LabelCell cell)
    {
        handler.PlatformView.UpdateUseDescriptionAsValue();
    }
}

