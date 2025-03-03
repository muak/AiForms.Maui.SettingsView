﻿using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Platform;

namespace AiForms.Settings.Platforms.Droid;

/// <summary>
/// Switch cell view.
/// </summary>
[Android.Runtime.Preserve(AllMembers = true)]
public class SwitchCellView : CellBaseView, CompoundButton.IOnCheckedChangeListener
{
    SwitchCompat _switch { get; set; }
    SwitchCell _SwitchCell => Cell as SwitchCell;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.SwitchCellView"/> class.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="cell">Cell.</param>
    public SwitchCellView(Context context, CellBase cell) : base(context, cell)
    {

        _switch = new SwitchCompat(context);

        _switch.SetOnCheckedChangeListener(this);
        _switch.Gravity = Android.Views.GravityFlags.Right | GravityFlags.CenterVertical;

        var switchParam = new LinearLayout.LayoutParams(
            ViewGroup.LayoutParams.WrapContent,
            ViewGroup.LayoutParams.WrapContent)
        {
        };

        using (switchParam)
        {
            AccessoryStack.AddView(_switch, switchParam);
        }

        _switch.Focusable = false;
        Focusable = false;
        DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;
    }

    public SwitchCellView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell()
    {
        //UpdateAccentColor();
        //UpdateOn();
        base.UpdateCell();
    }

    /// <summary>
    /// Cells the property changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        base.CellPropertyChanged(sender, e);
    }

    /// <summary>
    /// Parents the property changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    public override void ParentPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        base.ParentPropertyChanged(sender, e);
        if (e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName)
        {
            UpdateAccentColor();
        }
    }

    /// <summary>
    /// Ons the checked changed.
    /// </summary>
    /// <param name="buttonView">Button view.</param>
    /// <param name="isChecked">If set to <c>true</c> is checked.</param>
    public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
    {
        _SwitchCell.On = isChecked;
    }

    /// <summary>
    /// Rows the selected.
    /// </summary>
    /// <param name="adapter">Adapter.</param>
    /// <param name="position">Position.</param>
    public override void RowSelected(SettingsViewRecyclerAdapter adapter, int position)
    {
        _switch.Checked = !_switch.Checked;
    }

    /// <summary>
    /// Dispose the specified disposing.
    /// </summary>
    /// <returns>The dispose.</returns>
    /// <param name="disposing">If set to <c>true</c> disposing.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _switch.SetOnCheckedChangeListener(null);
            _switch.Background?.Dispose();
            _switch.Background = null;
            _switch.ThumbDrawable?.Dispose();
            _switch.ThumbDrawable = null;
            _switch.Dispose();
            _switch = null;
        }
        base.Dispose(disposing);
    }

    /// <summary>
    /// Sets the enabled appearance.
    /// </summary>
    /// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
    internal override void SetEnabledAppearance(bool isEnabled)
    {
        if (isEnabled)
        {
            _switch.Enabled = true;
            _switch.Alpha = 1.0f;
        }
        else
        {
            _switch.Enabled = false;
            _switch.Alpha = 0.3f;
        }
        base.SetEnabledAppearance(isEnabled);
    }

    internal void UpdateOn()
    {
        _switch.Checked = _SwitchCell.On;
    }

    internal void UpdateAccentColor()
    {
        if (_SwitchCell.AccentColor.IsNotDefault())
        {
            ChangeSwitchColor(_SwitchCell.AccentColor.ToPlatform());
        }
        else if (CellParent != null && CellParent.CellAccentColor.IsNotDefault())
        {
            ChangeSwitchColor(CellParent.CellAccentColor.ToPlatform());
        }
    }

    void ChangeSwitchColor(Android.Graphics.Color accent)
    {
        var trackColors = new ColorStateList(new int[][]
              {
                            new int[]{global::Android.Resource.Attribute.StateChecked},
                            new int[]{-global::Android.Resource.Attribute.StateChecked},
              },
             new int[] {
                            Android.Graphics.Color.Argb(76,accent.R,accent.G,accent.B),
                            Android.Graphics.Color.Argb(76, 117, 117, 117)
              });


        _switch.TrackDrawable.SetTintList(trackColors);

        var thumbColors = new ColorStateList(new int[][]
             {
                            new int[]{global::Android.Resource.Attribute.StateChecked},
                            new int[]{-global::Android.Resource.Attribute.StateChecked},
             },
            new int[] {
                            accent,
                            Android.Graphics.Color.Argb(255, 244, 244, 244)
             });

        _switch.ThumbDrawable.SetTintList(thumbColors);

        var ripple = _switch.Background as RippleDrawable;
        ripple.SetColor(trackColors);
    }
}