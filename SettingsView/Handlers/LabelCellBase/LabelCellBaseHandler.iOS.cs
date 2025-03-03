﻿using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers;

public partial class LabelCellBaseHandler<TvirtualCell, TnativeCell>
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
        if (handler.IsDisconnect) return;
        handler.PlatformView.UpdateValueTextColor();
    }

    private static void MapValueText(LabelCellBaseHandler<TvirtualCell, TnativeCell> handler, LabelCell cell)
    {
        if (handler.IsDisconnect) return;
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

    private static void MapValueTextFont(LabelCellBaseHandler<TvirtualCell, TnativeCell> handler, LabelCell cell)
    {
        if (handler.IsDisconnect) return;
        handler.PlatformView.UpdateValueTextFont();
        handler.PlatformView.UpdateLayout();
    }
}

