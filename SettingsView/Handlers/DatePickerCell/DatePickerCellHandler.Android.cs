﻿using System;
using AiForms.Settings.Platforms.Droid;

namespace AiForms.Settings.Handlers;

public partial class DatePickerCellHandler : LabelCellBaseHandler<DatePickerCell, DatePickerCellView>
{
    public static IPropertyMapper<DatePickerCell, DatePickerCellHandler> DatePickerMapper =
        new PropertyMapper<DatePickerCell, DatePickerCellHandler>(LabelMapper)
        {
            [nameof(DatePickerCell.Date)] = MapDate,
            [nameof(DatePickerCell.Format)] = MapDate,
            [nameof(DatePickerCell.MaximumDate)] = MapMaximumDate,
            [nameof(DatePickerCell.MinimumDate)] = MapMinimumDate,
        };

    private static void MapDate(DatePickerCellHandler handler, DatePickerCell arg2)
    {
        handler.PlatformView.UpdateDate();
    }

    private static void MapMaximumDate(DatePickerCellHandler handler, DatePickerCell arg2)
    {
        handler.PlatformView.UpdateMaximumDate();
    }

    private static void MapMinimumDate(DatePickerCellHandler handler, DatePickerCell arg2)
    {
        handler.PlatformView.UpdateMinimumDate();
    }
}

