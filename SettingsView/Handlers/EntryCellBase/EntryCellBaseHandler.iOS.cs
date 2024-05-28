using System;
using AiForms.Settings.Platforms.iOS;

namespace AiForms.Settings.Handlers;

public partial class EntryCellBaseHandler<TvirtualCell, TnativeCell>
{
    public static IPropertyMapper<EntryCell, EntryCellBaseHandler<TvirtualCell, TnativeCell>> EntryMapper =
        new PropertyMapper<EntryCell, EntryCellBaseHandler<TvirtualCell, TnativeCell>>(BasePropertyMapper)
        {
            [nameof(EntryCell.ValueText)] = MapValueText,
            [nameof(EntryCell.ValueTextFontFamily)] = MapValueTextFont,
            [nameof(EntryCell.ValueTextFontSize)] = MapValueTextFont,
            [nameof(EntryCell.ValueTextFontAttributes)] = MapValueTextFont,
            [nameof(EntryCell.ValueTextColor)] = MapValueTextColor,
            [nameof(EntryCell.Keyboard)] = MapKeyboard,
            [nameof(EntryCell.Placeholder)] = MapPlaceholder,
            [nameof(EntryCell.PlaceholderColor)] = MapPlaceholder,
            [nameof(EntryCell.TextAlignment)] = MapTextAlignment,
            [nameof(EntryCell.IsPassword)] = MapIsPassword,
        };

    private static void MapValueText(EntryCellBaseHandler<TvirtualCell, TnativeCell> handler, EntryCell arg2)
    {
        if (handler.IsDisconnect) return;
        handler.PlatformView.UpdateValueText();
    }    

    private static void MapValueTextFont(EntryCellBaseHandler<TvirtualCell, TnativeCell> handler, EntryCell arg2)
    {
        if (handler.IsDisconnect) return;
        handler.PlatformView.UpdateValueTextFont();
        handler.PlatformView.UpdateLayout();
    }

    private static void MapValueTextColor(EntryCellBaseHandler<TvirtualCell, TnativeCell> handler, EntryCell arg2)
    {
        if (handler.IsDisconnect) return;
        handler.PlatformView.UpdateValueTextColor();
    }

    private static void MapKeyboard(EntryCellBaseHandler<TvirtualCell, TnativeCell> handler, EntryCell arg2)
    {
        if (handler.IsDisconnect) return;
        handler.PlatformView.UpdateKeyboard();
    }

    private static void MapPlaceholder(EntryCellBaseHandler<TvirtualCell, TnativeCell> handler, EntryCell arg2)
    {
        if (handler.IsDisconnect) return;
        handler.PlatformView.UpdatePlaceholder();
    }    

    private static void MapTextAlignment(EntryCellBaseHandler<TvirtualCell, TnativeCell> handler, EntryCell arg2)
    {
        if (handler.IsDisconnect) return;
        handler.PlatformView.UpdateTextAlignment();
    }

    private static void MapIsPassword(EntryCellBaseHandler<TvirtualCell, TnativeCell> handler, EntryCell arg2)
    {
        if (handler.IsDisconnect) return;
        handler.PlatformView.UpdateIsPassword();
    }
}

