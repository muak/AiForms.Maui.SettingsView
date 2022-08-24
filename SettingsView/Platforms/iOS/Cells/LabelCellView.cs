using System;
using System.ComponentModel;
using AiForms.Settings.Extensions;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using UIKit;

namespace AiForms.Settings.Platforms.iOS;

public class LabelCellView: CellBaseView
{
    /// <summary>
    /// Gets or sets the value label.
    /// </summary>
    /// <value>The value label.</value>
    public UILabel ValueLabel { get; set; }
    LabelCell _LabelCell => Cell as LabelCell;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.LabelCellView"/> class.
    /// </summary>
    /// <param name="virtualCell">Forms cell.</param>
    public LabelCellView(CellBase virtualCell) : base(virtualCell)
    {
        ValueLabel = new UILabel();
        ValueLabel.TextAlignment = UITextAlignment.Right;

        ContentStack.AddArrangedSubview(ValueLabel);
        ValueLabel.SetContentHuggingPriority(100f, UILayoutConstraintAxis.Horizontal);
        ValueLabel.SetContentCompressionResistancePriority(100f, UILayoutConstraintAxis.Horizontal);
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
        else if (e.PropertyName == SettingsView.CellValueTextFontSizeProperty.PropertyName ||
                 e.PropertyName == SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
                 e.PropertyName == SettingsView.CellValueTextFontAttributesProperty.PropertyName)
        {
            UpdateWithForceLayout(UpdateValueTextFont);
        }
    }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell(UITableView tableView)
    {
        //UpdateValueText();
        //UpdateValueTextColor();
        //UpdateValueTextFont();
        base.UpdateCell(tableView);
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

    /// <summary>
    /// Updates the value text.
    /// </summary>
    internal void UpdateValueText()
    {
        ValueLabel.Text = _LabelCell.ValueText;
    }

    internal void UpdateValueTextFont()
    {
        if (ValueLabel.Font is null)
            return; // for HotReload

        var family = _LabelCell.ValueTextFontFamily ?? CellParent.CellValueTextFontFamily;
        var attr = _LabelCell.ValueTextFontAttributes ?? CellParent.CellValueTextFontAttributes;

        if (_LabelCell.ValueTextFontSize > 0)
        {
            ValueLabel.Font = family.ToFont(_LabelCell.ValueTextFontSize, attr).ToUIFont(_fontManager.Value); 
        }
        else if (CellParent != null)
        {
            ValueLabel.Font = family.ToFont(CellParent.CellValueTextFontSize, attr).ToUIFont(_fontManager.Value);
        }
    }

    internal void UpdateValueTextColor()
    {
        if (_LabelCell.ValueTextColor.IsNotDefault())
        {
            ValueLabel.TextColor = _LabelCell.ValueTextColor.ToPlatform();
        }
        else if (CellParent != null && CellParent.CellValueTextColor.IsNotDefault())
        {
            ValueLabel.TextColor = CellParent.CellValueTextColor.ToPlatform();
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
            if (!ValueLabel.IsDisposed())
            {
                ContentStack.RemoveArrangedSubview(ValueLabel);
                ValueLabel.Dispose();
            }
            ValueLabel = null;
        }
        base.Dispose(disposing);
    }
}

