using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers;

public partial class TimePickerCellHandler : LabelCellBaseHandler<TimePickerCell, TimePickerCellView>
{
    public static IPropertyMapper<TimePickerCell, TimePickerCellHandler> TimePickerMapper =
        new PropertyMapper<TimePickerCell, TimePickerCellHandler>(LabelMapper)
        {
            [nameof(TimePickerCell.Time)] = MapTime,
            [nameof(TimePickerCell.Format)] = MapTime,
            [nameof(TimePickerCell.PickerTitle)] = MapPickerTitle,
        };

    private static void MapTime(TimePickerCellHandler handler, TimePickerCell arg2)
    {
        handler.PlatformView.UpdateTime();
    }

    private static void MapPickerTitle(TimePickerCellHandler handler, TimePickerCell arg2)
    {
        handler.PlatformView.UpdatePickerTitle();
    }
}

