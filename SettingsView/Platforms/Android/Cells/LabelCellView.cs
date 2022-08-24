using System;
using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using AiForms.Settings.Extensions;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;

namespace AiForms.Settings.Platforms.Droid;

public class LabelCellView: CellBaseView
{
    LabelCell _LabelCell => Cell as LabelCell;
    /// <summary>
    /// Gets or sets the value label.
    /// </summary>
    /// <value>The value label.</value>
    public TextView ValueLabel { get; set; }
    /// <summary>
    /// The v value label.
    /// </summary>
    public TextView vValueLabel;


    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.LabelCellView"/> class.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="cell">Cell.</param>
    public LabelCellView(Context context, CellBase cell) : base(context, cell)
    {

        ValueLabel = new TextView(context);
        ValueLabel.SetSingleLine(true);
        ValueLabel.Ellipsize = TextUtils.TruncateAt.End;
        ValueLabel.Gravity = GravityFlags.Right;

        var textParams = new LinearLayout.LayoutParams(
            ViewGroup.LayoutParams.WrapContent,
            ViewGroup.LayoutParams.WrapContent)
        {

        };
        using (textParams)
        {
            ContentStack.AddView(ValueLabel, textParams);
        }

        UpdateUseDescriptionAsValue();
    }

    public LabelCellView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

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

        if (e.PropertyName == SettingsView.CellValueTextColorProperty.PropertyName)
        {
            UpdateValueTextColor();
        }
        else if (e.PropertyName == SettingsView.CellValueTextFontSizeProperty.PropertyName)
        {
            UpdateValueTextFontSize();
        }
        else if (e.PropertyName == SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
                 e.PropertyName == SettingsView.CellValueTextFontAttributesProperty.PropertyName)
        {
            UpdateValueTextFont();
        }
    }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell()
    {
        base.UpdateCell();
        //UpdateUseDescriptionAsValue();  //at first after base
        //UpdateValueText();
        //UpdateValueTextColor();
        //UpdateValueTextFontSize();
        //UpdateValueTextFont();
    }

    /// <summary>
    /// Sets the enabled appearance.
    /// </summary>
    /// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
    internal override void SetEnabledAppearance(bool isEnabled)
    {
        if (isEnabled)
        {
            ValueLabel.Alpha = 1f;
        }
        else
        {
            ValueLabel.Alpha = 0.3f;
        }
        base.SetEnabledAppearance(isEnabled);
    }

    internal void UpdateUseDescriptionAsValue()
    {
        if (!_LabelCell.IgnoreUseDescriptionAsValue && CellParent != null && CellParent.UseDescriptionAsValue)
        {
            vValueLabel = DescriptionLabel;
            DescriptionLabel.Visibility = ViewStates.Visible;
            ValueLabel.Visibility = ViewStates.Gone;
        }
        else
        {
            vValueLabel = ValueLabel;
            ValueLabel.Visibility = ViewStates.Visible;
        }
    }

    /// <summary>
    /// Updates the value text.
    /// </summary>
    internal void UpdateValueText()
    {
        vValueLabel.Text = _LabelCell.ValueText;
    }

    internal void UpdateValueTextFontSize()
    {
        if (_LabelCell.ValueTextFontSize > 0)
        {
            ValueLabel.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)_LabelCell.ValueTextFontSize);
        }
        else if (CellParent != null)
        {
            ValueLabel.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)CellParent.CellValueTextFontSize);
        }
        Invalidate();
    }

    internal void UpdateValueTextFont()
    {
        var family = _LabelCell.ValueTextFontFamily ?? CellParent?.CellValueTextFontFamily;
        var attr = _LabelCell.ValueTextFontAttributes ?? CellParent.CellValueTextFontAttributes;
        var size = _LabelCell.ValueTextFontSize > 0 ? _LabelCell.ValueTextFontSize : CellParent.CellValueTextFontSize;

        ValueLabel.Typeface = family.ToFont(size, attr).ToTypeface(_fontManager.Value);
        Invalidate();
    }

    internal void UpdateValueTextColor()
    {
        if (_LabelCell.ValueTextColor.IsNotDefault())
        {
            ValueLabel.SetTextColor(_LabelCell.ValueTextColor.ToPlatform());
        }
        else if (CellParent != null && CellParent.CellValueTextColor.IsNotDefault())
        {
            ValueLabel.SetTextColor(CellParent.CellValueTextColor.ToPlatform());
        }
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
            ValueLabel?.Dispose();
            ValueLabel = null;
            vValueLabel = null;
        }
        base.Dispose(disposing);
    }
}

