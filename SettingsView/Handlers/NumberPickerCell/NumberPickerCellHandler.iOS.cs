using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers;

public partial class NumberPickerCellHandler : LabelCellBaseHandler<NumberPickerCell, NumberPickerCellView>
{
    public static IPropertyMapper<NumberPickerCell, NumberPickerCellHandler> NumberPickerMapper =
        new PropertyMapper<NumberPickerCell, NumberPickerCellHandler>(LabelMapper)
        {
            [nameof(NumberPickerCell.Min)] = MapNumberList,
            [nameof(NumberPickerCell.Max)] = MapNumberList,
            [nameof(NumberPickerCell.Number)] = MapNumber,
            [nameof(NumberPickerCell.PickerTitle)] = MapPickerTitle,
            [nameof(NumberPickerCell.SelectedCommand)] = MapSelectedCommand,
        };

    private static void MapNumberList(NumberPickerCellHandler handler, NumberPickerCell arg2)
    {
        handler.PlatformView.UpdateNumberList();
    }

    private static void MapNumber(NumberPickerCellHandler handler, NumberPickerCell arg2)
    {
        handler.PlatformView.UpdateNumber();
    }

    private static void MapPickerTitle(NumberPickerCellHandler handler, NumberPickerCell arg2)
    {
        handler.PlatformView.UpdateTitle();
    }

    private static void MapSelectedCommand(NumberPickerCellHandler handler, NumberPickerCell arg2)
    {
        handler.PlatformView.UpdateCommand();
    }
}

