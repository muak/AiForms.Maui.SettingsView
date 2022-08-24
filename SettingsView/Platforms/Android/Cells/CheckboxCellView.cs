using System;
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
/// Checkbox cell view.
/// </summary>
public class CheckboxCellView : CellBaseView, CompoundButton.IOnCheckedChangeListener
{
    AppCompatCheckBox _checkbox;
    CheckboxCell _CheckboxCell => Cell as CheckboxCell;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.CheckboxCellView"/> class.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="cell">Cell.</param>
    public CheckboxCellView(Context context, CellBase cell) : base(context, cell)
    {
        _checkbox = new AppCompatCheckBox(context);
        _checkbox.SetOnCheckedChangeListener(this);
        _checkbox.Gravity = Android.Views.GravityFlags.Right;

        var lparam = new LinearLayout.LayoutParams(
            ViewGroup.LayoutParams.WrapContent,
            ViewGroup.LayoutParams.WrapContent)
        {
            Width = (int)context.ToPixels(30),
            Height = (int)context.ToPixels(30)
        };

        using (lparam)
        {
            AccessoryStack.AddView(_checkbox, lparam);
        }

        _checkbox.Focusable = false;
        Focusable = false;
        DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;
    }

    public CheckboxCellView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell()
    {
        //UpdateAccentColor();
        //UpdateChecked();
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
    /// Rows the selected.
    /// </summary>
    /// <param name="adapter">Adapter.</param>
    /// <param name="position">Position.</param>
    public override void RowSelected(SettingsViewRecyclerAdapter adapter, int position)
    {
        _checkbox.Checked = !_checkbox.Checked;
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
            _checkbox.SetOnCheckedChangeListener(null);
            _checkbox.Dispose();
            _checkbox = null;
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
            _checkbox.Enabled = true;
            _checkbox.Alpha = 1.0f;
        }
        else
        {
            _checkbox.Enabled = false;
            _checkbox.Alpha = 0.3f;
        }
        base.SetEnabledAppearance(isEnabled);
    }

    /// <summary>
    /// Ons the checked changed.
    /// </summary>
    /// <param name="buttonView">Button view.</param>
    /// <param name="isChecked">If set to <c>true</c> is checked.</param>
    public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
    {
        _CheckboxCell.Checked = isChecked;
        buttonView.JumpDrawablesToCurrentState();
    }

    internal void UpdateChecked()
    {
        _checkbox.Checked = _CheckboxCell.Checked;
    }

    internal void UpdateAccentColor()
    {
        if (_CheckboxCell.AccentColor.IsNotDefault())
        {
            ChangeCheckColor(_CheckboxCell.AccentColor.ToPlatform());
        }
        else if (CellParent != null && CellParent.CellAccentColor.IsNotDefault())
        {
            ChangeCheckColor(CellParent.CellAccentColor.ToPlatform());
        }
    }


    void ChangeCheckColor(Android.Graphics.Color accent)
    {

        var colorList = new ColorStateList(
            new int[][]{
                    new int[]{global::Android.Resource.Attribute.StateChecked},
                    new int[]{-global::Android.Resource.Attribute.StateChecked},
            },
            new int[]{
                    accent,
                    accent
            }
        );

        _checkbox.SupportButtonTintList = colorList;

        var rippleColor = new ColorStateList(
            new int[][]{
                    new int[]{global::Android.Resource.Attribute.StateChecked},
                    new int[]{-global::Android.Resource.Attribute.StateChecked}
            },
            new int[] {
                    Android.Graphics.Color.Argb(76,accent.R,accent.G,accent.B),
                    Android.Graphics.Color.Argb(76, 117, 117, 117)
            }
        );
        var ripple = _checkbox.Background as RippleDrawable;
        ripple.SetColor(rippleColor);
        _checkbox.Background = ripple;
    }
}

