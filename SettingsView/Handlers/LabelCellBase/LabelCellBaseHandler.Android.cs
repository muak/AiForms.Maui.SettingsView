using System;
using AiForms.Settings.Platforms.Droid;

namespace AiForms.Settings.Handlers
{
    public partial class LabelCellBaseHandler<TvirtualCell, TnativeCell>
    {
        public static IPropertyMapper<LabelCell, LabelCellBaseHandler<TvirtualCell, TnativeCell>> LabelMapper =
        new PropertyMapper<LabelCell, LabelCellBaseHandler<TvirtualCell, TnativeCell>>(BasePropertyMapper)
        {
            [nameof(LabelCell.ValueText)] = MapValueText,
            [nameof(LabelCell.ValueTextColor)] = MapValueTextColor,
            [nameof(LabelCell.ValueTextFontSize)] = MapValueTextFontSize,
            [nameof(LabelCell.ValueTextFontAttributes)] = MapValueTextFont,
            [nameof(LabelCell.ValueTextFontFamily)] = MapValueTextFont,
            [nameof(LabelCell.IgnoreUseDescriptionAsValue)] = MapUseDescriptionAsValue,
        };

        private static void MapValueText(LabelCellBaseHandler<TvirtualCell, TnativeCell> handler, LabelCell cell)
        {
            switch (cell)
            {
                // disabled ValueTextMapper if cell is PickerCell.
                case NumberPickerCell:
                case TimePickerCell:
                case DatePickerCell:
                case TextPickerCell:
                    return;
            }

            handler.PlatformView.UpdateValueText();
        }

        private static void MapValueTextColor(LabelCellBaseHandler<TvirtualCell, TnativeCell> handler, LabelCell cell)
        {
            handler.PlatformView.UpdateValueTextColor();
        }

        private static void MapValueTextFontSize(LabelCellBaseHandler<TvirtualCell, TnativeCell> handler, LabelCell cell)
        {
            handler.PlatformView.UpdateValueTextFontSize();
        }

        private static void MapValueTextFont(LabelCellBaseHandler<TvirtualCell, TnativeCell> handler, LabelCell cell)
        {
            handler.PlatformView.UpdateValueTextFont();
        }

        private static void MapUseDescriptionAsValue(LabelCellBaseHandler<TvirtualCell, TnativeCell> handler, LabelCell cell)
        {
            handler.PlatformView.UpdateUseDescriptionAsValue();
        }
    }
}

