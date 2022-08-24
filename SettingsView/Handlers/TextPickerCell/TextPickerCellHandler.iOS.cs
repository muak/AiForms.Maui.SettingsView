using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers;

public partial class TextPickerCellHandler : LabelCellBaseHandler<TextPickerCell, TextPickerCellView>
{
    public static IPropertyMapper<TextPickerCell, TextPickerCellHandler> TextPickerMapper =
        new PropertyMapper<TextPickerCell, TextPickerCellHandler>(LabelMapper)
        {
            [nameof(TextPickerCell.Items)] = MapItems,
            [nameof(TextPickerCell.SelectedItem)] = MapSelectedItem,
            [nameof(TextPickerCell.PickerTitle)] = MapPickerTitle,
            [nameof(TextPickerCell.SelectedCommand)] = MapSelectedCommand,            
        };

    private static void MapItems(TextPickerCellHandler handler, TextPickerCell arg2)
    {
        handler.PlatformView.UpdateItems();
    }

    private static void MapSelectedItem(TextPickerCellHandler handler, TextPickerCell arg2)
    {
        handler.PlatformView.UpdateSelectedItem();
    }

    private static void MapPickerTitle(TextPickerCellHandler handler, TextPickerCell arg2)
    {
        handler.PlatformView.UpdateTitle();
    }

    private static void MapSelectedCommand(TextPickerCellHandler handler, TextPickerCell arg2)
    {
        handler.PlatformView.UpdateCommand();
    }
}

