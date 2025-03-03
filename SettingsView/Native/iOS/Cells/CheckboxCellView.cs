﻿using CoreGraphics;
using Foundation;
using Microsoft.Maui.Platform;
using UIKit;

namespace AiForms.Settings.Platforms.iOS;

/// <summary>
/// Checkbox cell view.
/// </summary>
[Foundation.Preserve(AllMembers = true)]
public class CheckboxCellView : CellBaseView
{
    CheckBox _checkbox;
    CheckboxCell _CheckboxCell => Cell as CheckboxCell;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.CheckboxCellView"/> class.
    /// </summary>
    /// <param name="virtualCell">Forms cell.</param>
    public CheckboxCellView(CellBase virtualCell) : base(virtualCell)
    {
        _checkbox = new CheckBox(new CGRect(0, 0, 20, 20));
        _checkbox.Layer.BorderWidth = 2;
        _checkbox.Layer.CornerRadius = 3;
        _checkbox.Inset = new UIEdgeInsets(10, 10, 10, 10);

        _checkbox.CheckChanged = CheckChanged;

        AccessoryView = _checkbox;
        EditingAccessoryView = _checkbox;
    }

    public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
    {
        base.RowSelected(tableView, indexPath);
        _checkbox.Selected = !_checkbox.Selected;
        CheckChanged(_checkbox);
    }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell(UITableView tableView)
    {
        //UpdateAccentColor();
        //UpdateChecked();
        base.UpdateCell(tableView);
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
    /// Dispose the specified disposing.
    /// </summary>
    /// <returns>The dispose.</returns>
    /// <param name="disposing">If set to <c>true</c> disposing.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _checkbox.CheckChanged = null;
            _checkbox?.Dispose();
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
            _checkbox.Alpha = 1.0f;
        }
        else
        {
            _checkbox.Alpha = 0.3f;
        }
        base.SetEnabledAppearance(isEnabled);
    }

    void CheckChanged(UIButton button)
    {
        _CheckboxCell.Checked = button.Selected;
    }

    internal void UpdateChecked()
    {
        _checkbox.Selected = _CheckboxCell.Checked;
    }

    internal void UpdateAccentColor()
    {
        if (_CheckboxCell.AccentColor.IsNotDefault())
        {
            ChangeCheckColor(_CheckboxCell.AccentColor.ToCGColor());
        }
        else if (CellParent != null && CellParent.CellAccentColor.IsNotDefault())
        {
            ChangeCheckColor(CellParent.CellAccentColor.ToCGColor());
        }
    }

    void ChangeCheckColor(CGColor accent)
    {
        _checkbox.Layer.BorderColor = accent;
        _checkbox.FillColor = accent;
        _checkbox.SetNeedsDisplay(); //update inner rect
    }
}

/// <summary>
/// Check box.
/// </summary>
public class CheckBox : UIButton
{
    /// <summary>
    /// Gets or sets the inset.
    /// </summary>
    /// <value>The inset.</value>
    public UIEdgeInsets Inset { get; set; } = new UIEdgeInsets(20, 20, 20, 20);
    /// <summary>
    /// Gets or sets the color of the fill.
    /// </summary>
    /// <value>The color of the fill.</value>
    public CGColor FillColor { get; set; }
    /// <summary>
    /// Gets or sets the check changed.
    /// </summary>
    /// <value>The check changed.</value>
    public Action<UIButton> CheckChanged { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.CheckBox"/> class.
    /// </summary>
    /// <param name="rect">Rect.</param>
    public CheckBox(CGRect rect) : base(rect)
    {
        this.AddGestureRecognizer(new UITapGestureRecognizer((obj) =>
        {
            Selected = !Selected;
            CheckChanged?.Invoke(this);
        }));
    }

    /// <summary>
    /// Draw the specified rect.
    /// </summary>
    /// <returns>The draw.</returns>
    /// <param name="rect">Rect.</param>
    public override void Draw(CGRect rect)
    {
        base.Draw(rect);

        var lineWidth = rect.Size.Width / 10;

        // Draw check mark
        if (Selected)
        {
            this.Layer.BackgroundColor = FillColor;


            var checkmark = new UIBezierPath();
            var size = rect.Size;
            checkmark.MoveTo(new CGPoint(x: 22f / 100f * size.Width, y: 52f / 100f * size.Height));
            checkmark.AddLineTo(new CGPoint(x: 38f / 100f * size.Width, y: 68f / 100f * size.Height));
            checkmark.AddLineTo(new CGPoint(x: 76f / 100f * size.Width, y: 30f / 100f * size.Height));

            checkmark.LineWidth = lineWidth;
            UIColor.White.SetStroke();
            checkmark.Stroke();
        }

        else
        {
            this.Layer.BackgroundColor = new CGColor(0, 0, 0, 0);
        }
    }

    /// <summary>
    /// Points the inside.
    /// </summary>
    /// <returns><c>true</c>, if inside was pointed, <c>false</c> otherwise.</returns>
    /// <param name="point">Point.</param>
    /// <param name="uievent">Uievent.</param>
    public override bool PointInside(CGPoint point, UIEvent uievent)
    {
        var rect = this.Bounds;
        rect.X -= Inset.Left;
        rect.Y -= Inset.Top;
        rect.Width += Inset.Left + Inset.Right;
        rect.Height += Inset.Top + Inset.Bottom;

        return rect.Contains(point);
    }

}

