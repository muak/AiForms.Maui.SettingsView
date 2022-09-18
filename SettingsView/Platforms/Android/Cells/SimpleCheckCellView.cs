using System;
using System.ComponentModel;
using Android.Content;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Platform;

namespace AiForms.Settings.Platforms.Droid;

public class SimpleCheckCellView: CellBaseView
{
    SimpleCheck _checkView;
    SimpleCheckCell _simpleCheckCell => Cell as SimpleCheckCell;

    public SimpleCheckCellView(Context context, CellBase cell) : base(context, cell)
    {
        _checkView = new SimpleCheck(context);
        _checkView.Focusable = false;

        var lparam = new LinearLayout.LayoutParams(
            ViewGroup.LayoutParams.WrapContent,
            ViewGroup.LayoutParams.WrapContent)
        {
            Width = (int)context.ToPixels(30),
            Height = (int)context.ToPixels(30)
        };

        using (lparam)
        {
            AccessoryStack.AddView(_checkView, lparam);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _checkView.RemoveFromParent();
            _checkView.Dispose();
            _checkView = null;
        }
        base.Dispose(disposing);
    }

    public override void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.ParentPropertyChanged(sender, e);
        if (e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName)
        {
            UpdateAccentColor();
        }
    }

    public override void RowSelected(SettingsViewRecyclerAdapter adapter, int position)
    {
        base.RowSelected(adapter, position);
    }

    internal void UpdateChecked()
    {
        _checkView.Selected = _simpleCheckCell.Checked;
    }

    internal void UpdateAccentColor()
    {
        if (_simpleCheckCell.AccentColor.IsNotDefault())
        {
            _checkView.Color = _simpleCheckCell.AccentColor.ToPlatform();
        }
        else if (CellParent != null && CellParent.CellAccentColor.IsNotDefault())
        {
            _checkView.Color = CellParent.CellAccentColor.ToPlatform();
        }
        _checkView.Invalidate();
    }
}

