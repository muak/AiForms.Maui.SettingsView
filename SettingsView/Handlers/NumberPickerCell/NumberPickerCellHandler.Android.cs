using System;
using AiForms.Settings.Platforms.Droid;

namespace AiForms.Settings.Handlers;

public partial class NumberPickerCellHandler : LabelCellBaseHandler<NumberPickerCell, NumberPickerCellView>
{
    public static IPropertyMapper<NumberPickerCell, NumberPickerCellHandler> NumberPickerMapper =
        new PropertyMapper<NumberPickerCell, NumberPickerCellHandler>(LabelMapper)
        {
            [nameof(NumberPickerCell.Min)] = MapMin,
            [nameof(NumberPickerCell.Max)] = MapMax,
            [nameof(NumberPickerCell.Number)] = MapNumber,
            [nameof(NumberPickerCell.PickerTitle)] = MapPickerTitle,
            [nameof(NumberPickerCell.SelectedCommand)] = MapSelectedCommand,
        };

    private static void MapMin(NumberPickerCellHandler handler, NumberPickerCell arg2)
    {
        handler.PlatformView.UpdateMin();
    }

    private static void MapMax(NumberPickerCellHandler handler, NumberPickerCell arg2)
    {
        handler.PlatformView.UpdateMax();
    }

    private static void MapNumber(NumberPickerCellHandler handler, NumberPickerCell arg2)
    {
        handler.PlatformView.UpdateNumber();
    }

    private static void MapPickerTitle(NumberPickerCellHandler handler, NumberPickerCell arg2)
    {
        handler.PlatformView.UpdatePickerTitle();
    }

    private static void MapSelectedCommand(NumberPickerCellHandler handler, NumberPickerCell arg2)
    {
        handler.PlatformView.UpdateCommand();
    }
}

