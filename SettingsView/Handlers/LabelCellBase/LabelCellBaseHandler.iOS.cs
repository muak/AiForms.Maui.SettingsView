using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers
{
    public partial class LabelCellBaseHandler<TvirtualCell, TnativeCell>: CellBaseHandler<TvirtualCell, TnativeCell>
        where TvirtualCell : LabelCell
        where TnativeCell : LabelCellView
    {
        public static IPropertyMapper<LabelCell, LabelCellBaseHandler<TvirtualCell, TnativeCell>> LabelMapper =
        new PropertyMapper<LabelCell, LabelCellBaseHandler<TvirtualCell, TnativeCell>>(BasePropertyMapper)
        {
            [nameof(LabelCell.ValueText)] = MapValueText,
            [nameof(LabelCell.ValueTextColor)] = MapValueTextColor,
            [nameof(LabelCell.ValueTextFontSize)] = MapValueTextFont,
            [nameof(LabelCell.ValueTextFontAttributes)] = MapValueTextFont,
            [nameof(LabelCell.ValueTextFontFamily)] = MapValueTextFont,
        };


        private static void MapValueTextColor(LabelCellBaseHandler<TvirtualCell, TnativeCell> handler, LabelCell cell)
        {
            handler.PlatformView.UpdateValueTextColor();
        }

        private static void MapValueText(LabelCellBaseHandler<TvirtualCell, TnativeCell> handler, LabelCell cell)
        {
            handler.PlatformView.UpdateValueText();
        }

        private static void MapValueTextFont(LabelCellBaseHandler<TvirtualCell, TnativeCell> handler, LabelCell cell)
        {
            handler.PlatformView.UpdateValueTextFont();
            handler.PlatformView.UpdateLayout();
        }
    }
}

