using System;
using AiForms.Settings.Platforms.Droid;

namespace AiForms.Settings.Handlers;

public partial class EntryCellHandler : CellBaseHandler<EntryCell, EntryCellView>
{
    public static IPropertyMapper<EntryCell, EntryCellHandler> EntryMapper =
        new PropertyMapper<EntryCell, EntryCellHandler>(BasePropertyMapper)
        {
            [nameof(EntryCell.ValueText)] = MapValueText,
            [nameof(EntryCell.ValueTextFontSize)] = MapValueTextFontSize,
            [nameof(EntryCell.ValueTextFontFamily)] = MapValueTextFont,
            [nameof(EntryCell.ValueTextFontAttributes)] = MapValueTextFont,
            [nameof(EntryCell.ValueTextColor)] = MapValueTextColor,
            [nameof(EntryCell.Keyboard)] = MapKeyboard,
            [nameof(EntryCell.Placeholder)] = MapPlaceholder,
            [nameof(EntryCell.PlaceholderColor)] = MapPlaceholder,
            [nameof(EntryCell.AccentColor)] = MapAccentColor,
            [nameof(EntryCell.TextAlignment)] = MapTextAlignment,
            [nameof(EntryCell.IsPassword)] = MapIsPassword,
        };

    private static void MapValueText(EntryCellHandler handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateValueText();
    }

    private static void MapValueTextFontSize(EntryCellHandler handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateValueTextFontSize();
        handler.PlatformView.UpdateLayout();
    }

    private static void MapValueTextFont(EntryCellHandler handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateValueTextFont();
        handler.PlatformView.UpdateLayout();
    }

    private static void MapValueTextColor(EntryCellHandler handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateValueTextColor();
        handler.PlatformView.UpdateLayout();
    }

    private static void MapKeyboard(EntryCellHandler handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateKeyboard();
    }

    private static void MapPlaceholder(EntryCellHandler handler, EntryCell arg2)
    {
        handler.PlatformView.UpdatePlaceholder();
    }

    private static void MapAccentColor(EntryCellHandler handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateAccentColor();
    }

    private static void MapTextAlignment(EntryCellHandler handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateTextAlignment();
    }

    private static void MapIsPassword(EntryCellHandler handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateIsPassword();
    }
}

