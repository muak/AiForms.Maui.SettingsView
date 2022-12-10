using System;
using AiForms.Settings.Platforms.Droid;

namespace AiForms.Settings.Handlers;

public partial class SimpleCheckCellHandler : CellBaseHandler<SimpleCheckCell, SimpleCheckCellView>
{
    private static void MapAccentColor(SimpleCheckCellHandler handler, SimpleCheckCell arg2)
    {
        handler.PlatformView.UpdateAccentColor();
    }

    private static void MapChecked(SimpleCheckCellHandler handler, SimpleCheckCell arg2)
    {
        handler.PlatformView.UpdateChecked();
    }
}

