using System.Collections.Generic;
using System.Linq;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Platform;

namespace AiForms.Settings.Platforms.Droid;

[Android.Runtime.Preserve(AllMembers = true)]
public class SettingsViewLayoutManager:LinearLayoutManager
{
    SettingsView _settingsView;
    Android.Content.Context _context;
    Dictionary<Android.Views.View, int> ItemHeights = new Dictionary<Android.Views.View, int>();
    bool _disposed;

    public SettingsViewLayoutManager(Android.Content.Context context,SettingsView settingsView):base(context)
    {
        _context = context;
        _settingsView = settingsView;
    }

    public override int GetDecoratedMeasuredHeight(Android.Views.View child)
    {
        if (_disposed) return 0;

        var height =  base.GetDecoratedMeasuredHeight(child);
        ItemHeights[child] = height;
        return height;
    }

    protected override void Dispose(bool disposing)
    {
        if (_disposed) return;
        _disposed = true;

        if(disposing)
        {
            ItemHeights?.Clear();
            ItemHeights = null;
            _context = null;
            _settingsView = null;
        }
        base.Dispose(disposing);
    }

    public override void OnLayoutCompleted(RecyclerView.State state)
    {
        base.OnLayoutCompleted(state);

        if (_disposed || _settingsView == null || _context == null || ItemHeights == null)
        {
            return;
        }

        var total = ItemHeights.Sum(x => x.Value);

        _settingsView.VisibleContentHeight = _context.FromPixels(total);
    }
}
