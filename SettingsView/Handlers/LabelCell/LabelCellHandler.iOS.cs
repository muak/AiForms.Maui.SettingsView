using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers;

public partial class LabelCellHandler: CellBaseHandler<LabelCell, LabelCellView>
{
    public static IPropertyMapper<LabelCell, CellBaseHandler<LabelCell, LabelCellView>> LabelMapper =
        new PropertyMapper<LabelCell, CellBaseHandler<LabelCell, LabelCellView>>(BasePropertyMapper)
        {
            [nameof(LabelCell.ValueText)]  = MapValueText,
            [nameof(LabelCell.ValueTextColor)] = MapValueTextColor,
            [nameof(LabelCell.ValueTextFontSize)] = MapValueTextFont,
            [nameof(LabelCell.ValueTextFontAttributes)] = MapValueTextFont,
            [nameof(LabelCell.ValueTextFontFamily)] = MapValueTextFont,
        };

    private static void MapValueTextColor(CellBaseHandler<LabelCell, LabelCellView> handler, LabelCell cell)
    {
        handler.PlatformView.UpdateValueTextColor();
    }

    private static void MapValueText(CellBaseHandler<LabelCell, LabelCellView> handler, LabelCell cell)
    {
        handler.PlatformView.UpdateValueText();
    }

    private static void MapValueTextFont(CellBaseHandler<LabelCell, LabelCellView> handler, LabelCell cell)
    {
        handler.PlatformView.UpdateValueTextFont();
        handler.PlatformView.UpdateLayout();
    }    
}

