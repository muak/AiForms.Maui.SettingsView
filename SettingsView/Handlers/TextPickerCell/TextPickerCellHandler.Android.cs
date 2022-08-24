using System;
using AiForms.Settings.Platforms.Droid;

namespace AiForms.Settings.Handlers;

public partial class TextPickerCellHandler : LabelCellBaseHandler<TextPickerCell, TextPickerCellView>
{
    public static IPropertyMapper<TextPickerCell, TextPickerCellHandler> TextPickerMapper =
        new PropertyMapper<TextPickerCell, TextPickerCellHandler>(LabelMapper)
        {
            [nameof(TextPickerCell.SelectedItem)] = MapSelectedItem,
            [nameof(TextPickerCell.PickerTitle)] = MapPickerTitle,
            [nameof(TextPickerCell.SelectedCommand)] = MapSelectedCommand,
        };

    private static void MapSelectedItem(TextPickerCellHandler handler, TextPickerCell arg2)
    {
        handler.PlatformView.UpdateSelectedItem();
    }

    private static void MapPickerTitle(TextPickerCellHandler handler, TextPickerCell arg2)
    {
        handler.PlatformView.UpdatePickerTitle();
    }

    private static void MapSelectedCommand(TextPickerCellHandler handler, TextPickerCell arg2)
    {
        handler.PlatformView.UpdateCommand();
    }
}

