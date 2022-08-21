using System;
using AiForms.Settings.Platforms.Droid;

namespace AiForms.Settings.Handlers;

public partial class EntryCellHandler : CellBaseHandler<EntryCell, EntryCellView>
{
    public static IPropertyMapper<EntryCell, CellBaseHandler<EntryCell, EntryCellView>> EntryMapper =
        new PropertyMapper<EntryCell, CellBaseHandler<EntryCell, EntryCellView>>(BasePropertyMapper)
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

    private static void MapValueText(CellBaseHandler<EntryCell, EntryCellView> handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateValueText();
    }

    private static void MapValueTextFontSize(CellBaseHandler<EntryCell, EntryCellView> handler, EntryCell arg2)
    {
        handler.PlatformView.UpdateValueTextFontSize();
        handler.PlatformView.updatelayout
    }

    private static void MapValueTextFont(CellBaseHandler<EntryCell, EntryCellView> handler, EntryCell arg2)
    {
        throw new NotImplementedException();
    }

    private static void MapValueTextColor(CellBaseHandler<EntryCell, EntryCellView> handler, EntryCell arg2)
    {
        throw new NotImplementedException();
    }

    private static void MapKeyboard(CellBaseHandler<EntryCell, EntryCellView> handler, EntryCell arg2)
    {
        throw new NotImplementedException();
    }

    private static void MapPlaceholder(CellBaseHandler<EntryCell, EntryCellView> handler, EntryCell arg2)
    {
        throw new NotImplementedException();
    }

    private static void MapAccentColor(CellBaseHandler<EntryCell, EntryCellView> handler, EntryCell arg2)
    {
        throw new NotImplementedException();
    }

    private static void MapTextAlignment(CellBaseHandler<EntryCell, EntryCellView> handler, EntryCell arg2)
    {
        throw new NotImplementedException();
    }

    private static void MapIsPassword(CellBaseHandler<EntryCell, EntryCellView> handler, EntryCell arg2)
    {
        throw new NotImplementedException();
    }
}

