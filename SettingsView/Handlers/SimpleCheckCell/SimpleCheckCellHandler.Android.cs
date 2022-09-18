using System;
using AiForms.Settings.Platforms.Droid;

namespace AiForms.Settings.Handlers;

public partial class SimpleCheckCellHandler : CellBaseHandler<SimpleCheckCell, SimpleCheckCellView>
{
    private static void MapAccentColor(CellBaseHandler<SimpleCheckCell, SimpleCheckCellView> handler, SimpleCheckCell arg2)
    {
        handler.PlatformView.UpdateAccentColor();
    }

    private static void MapChecked(CellBaseHandler<SimpleCheckCell, SimpleCheckCellView> handler, SimpleCheckCell arg2)
    {
        handler.PlatformView.UpdateChecked();
    }
}

