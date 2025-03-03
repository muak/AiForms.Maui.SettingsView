﻿using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using System.ComponentModel;
using Microsoft.Maui.Platform;

namespace AiForms.Settings.Platforms.Droid;

/// <summary>
/// Radio cell view.
/// </summary>
[Android.Runtime.Preserve(AllMembers = true)]
public class RadioCellView : CellBaseView
{
    SimpleCheck _simpleCheck;
    RadioCell _radioCell => Cell as RadioCell;

    private object SelectedValue
    {
        get
        {
            return RadioCell.GetSelectedValue(_radioCell.Section) ?? RadioCell.GetSelectedValue(CellParent);
        }
        set
        {
            if (RadioCell.GetSelectedValue(_radioCell.Section) != null)
            {
                RadioCell.SetSelectedValue(_radioCell.Section, value);
            }
            else
            {
                RadioCell.SetSelectedValue(CellParent, value);
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.RadioCellView"/> class.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="cell">Cell.</param>
    public RadioCellView(Context context, CellBase cell) : base(context, cell)
    {
        _simpleCheck = new SimpleCheck(context);
        _simpleCheck.Focusable = false;

        var lparam = new LinearLayout.LayoutParams(
            ViewGroup.LayoutParams.WrapContent,
            ViewGroup.LayoutParams.WrapContent)
        {
            Width = (int)context.ToPixels(30),
            Height = (int)context.ToPixels(30)
        };

        using (lparam)
        {
            AccessoryStack.AddView(_simpleCheck, lparam);
        }
    }

    public RadioCellView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

    /// <summary>
    /// Dispose the specified disposing.
    /// </summary>
    /// <param name="disposing">If set to <c>true</c> disposing.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _simpleCheck.RemoveFromParent();
            _simpleCheck.Dispose();
            _simpleCheck = null;
        }
        base.Dispose(disposing);
    }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell()
    {
        //UpdateAccentColor();
        UpdateSelectedValue();
        base.UpdateCell();
    }

    /// <summary>
    /// Cells the property changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    public override void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.CellPropertyChanged(sender, e);
    }

    /// <summary>
    /// Parents the property changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    public override void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.ParentPropertyChanged(sender, e);
        if (e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName)
        {
            UpdateAccentColor();
        }
        else if (e.PropertyName == RadioCell.SelectedValueProperty.PropertyName)
        {
            UpdateSelectedValue();
        }
    }

    /// <summary>
    /// Sections the property changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    public override void SectionPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.SectionPropertyChanged(sender, e);
        if (e.PropertyName == RadioCell.SelectedValueProperty.PropertyName)
        {
            UpdateSelectedValue();
        }
    }

    /// <summary>
    /// Rows the selected.
    /// </summary>
    /// <param name="adapter">Adapter.</param>
    /// <param name="position">Position.</param>
    public override void RowSelected(SettingsViewRecyclerAdapter adapter, int position)
    {
        if (!_simpleCheck.Selected)
        {
            SelectedValue = _radioCell.Value;
        }
    }

    void UpdateSelectedValue()
    {
        bool result;
        if (_radioCell.Value.GetType().IsValueType || _radioCell.Value is string)
        {
            result = object.Equals(_radioCell.Value, SelectedValue);
        }
        else
        {
            result = object.ReferenceEquals(_radioCell.Value, SelectedValue);
        }

        _simpleCheck.Selected = result;
    }

    internal void UpdateAccentColor()
    {
        if (_radioCell.AccentColor.IsNotDefault())
        {
            _simpleCheck.Color = _radioCell.AccentColor.ToPlatform();
        }
        else if (CellParent != null && CellParent.CellAccentColor.IsNotDefault())
        {
            _simpleCheck.Color = CellParent.CellAccentColor.ToPlatform();
        }
        _simpleCheck.Invalidate();
    }
}