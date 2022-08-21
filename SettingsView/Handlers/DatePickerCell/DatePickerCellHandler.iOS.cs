using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers;

public partial class DatePickerCellHandler : CellBaseHandler<DatePickerCell, DatePickerCellView>
{
    public static IPropertyMapper<DatePickerCell, CellBaseHandler<DatePickerCell, DatePickerCellView>> DatePickerMapper =
        new PropertyMapper<DatePickerCell, CellBaseHandler<DatePickerCell, DatePickerCellView>>(BasePropertyMapper)
        {
            [nameof(DatePickerCell.Date)] = MapDate,
            [nameof(DatePickerCell.Format)] = MapDate,
            [nameof(DatePickerCell.MaximumDate)] = MapMaximumDate,
            [nameof(DatePickerCell.MinimumDate)] = MapMinimumDate,
            [nameof(DatePickerCell.TodayText)] = MapTodayText,
        };

    private static void MapDate(CellBaseHandler<DatePickerCell, DatePickerCellView> handler, DatePickerCell arg2)
    {
        handler.PlatformView.UpdateDate();
    }

    private static void MapMaximumDate(CellBaseHandler<DatePickerCell, DatePickerCellView> handler, DatePickerCell arg2)
    {
        handler.PlatformView.UpdateMaximumDate();
    }

    private static void MapMinimumDate(CellBaseHandler<DatePickerCell, DatePickerCellView> handler, DatePickerCell arg2)
    {
        handler.PlatformView.UpdateMinimumDate();
    }

    private static void MapTodayText(CellBaseHandler<DatePickerCell, DatePickerCellView> handler, DatePickerCell arg2)
    {
        handler.PlatformView.UpdateTodayText();
    }
}

