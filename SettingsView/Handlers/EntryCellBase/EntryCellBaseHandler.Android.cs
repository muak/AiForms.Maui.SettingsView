using System;
using AiForms.Settings.Platforms.Droid;

namespace AiForms.Settings.Handlers;

public partial class EntryCellBaseHandler<TvirtualCell, TnativeCell>
{
    public static IPropertyMapper<EntryCell, EntryCellBaseHandler<TvirtualCell, TnativeCell>> EntryMapper =
        new PropertyMapper<EntryCell, EntryCellBaseHandler<TvirtualCell, TnativeCell>>(BasePropertyMapper)
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

    private static void MapValueText(EntryCellBaseHandler<TvirtualCell,TnativeCell> handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateValueText();
    }

    private static void MapValueTextFontSize(EntryCellBaseHandler<TvirtualCell,TnativeCell> handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateValueTextFontSize();
        handler.PlatformView.UpdateLayout();
    }

    private static void MapValueTextFont(EntryCellBaseHandler<TvirtualCell,TnativeCell> handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateValueTextFont();
        handler.PlatformView.UpdateLayout();
    }

    private static void MapValueTextColor(EntryCellBaseHandler<TvirtualCell,TnativeCell> handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateValueTextColor();
        handler.PlatformView.UpdateLayout();
    }

    private static void MapKeyboard(EntryCellBaseHandler<TvirtualCell,TnativeCell> handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateKeyboard();
    }

    private static void MapPlaceholder(EntryCellBaseHandler<TvirtualCell,TnativeCell> handler, EntryCell arg2)
    {
        handler.PlatformView.UpdatePlaceholder();
    }

    private static void MapAccentColor(EntryCellBaseHandler<TvirtualCell,TnativeCell> handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateAccentColor();
    }

    private static void MapTextAlignment(EntryCellBaseHandler<TvirtualCell,TnativeCell> handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateTextAlignment();
    }

    private static void MapIsPassword(EntryCellBaseHandler<TvirtualCell,TnativeCell> handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateIsPassword();
    }
}

