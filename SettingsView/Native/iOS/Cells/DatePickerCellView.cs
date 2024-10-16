﻿using System;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Platform;
using UIKit;

namespace AiForms.Settings.Platforms.iOS;

/// <summary>
/// Date picker cell view.
/// </summary>
[Foundation.Preserve(AllMembers = true)]
public class DatePickerCellView : LabelCellView
{
    DatePickerCell _DatePickerCell => Cell as DatePickerCell;
    /// <summary>
    /// Gets or sets the dummy field.
    /// </summary>
    /// <value>The dummy field.</value>
    public UITextField DummyField { get; set; }
    NSDate _preSelectedDate;
    UIDatePicker _picker;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.DatePickerCellView"/> class.
    /// </summary>
    /// <param name="formsCell">Forms cell.</param>
    public DatePickerCellView(CellBase formsCell) : base(formsCell)
    {
        DummyField = new NoCaretField();
        DummyField.BorderStyle = UITextBorderStyle.None;
        DummyField.BackgroundColor = UIColor.Clear;
        ContentView.AddSubview(DummyField);
        ContentView.SendSubviewToBack(DummyField);

        SelectionStyle = UITableViewCellSelectionStyle.Default;

        SetUpDatePicker();
    }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell(UITableView tableView)
    {
        base.UpdateCell(tableView);
        //UpdateMaximumDate();
        //UpdateMinimumDate();
        //UpdateDate();
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
    /// Rows the selected.
    /// </summary>
    /// <param name="tableView">Table view.</param>
    /// <param name="indexPath">Index path.</param>
    public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
    {
        tableView.DeselectRow(indexPath, true);
        DummyField.BecomeFirstResponder();
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
            DummyField.RemoveFromSuperview();
            DummyField?.Dispose();
            DummyField = null;
            _picker.Dispose();
            _picker = null;
        }
        base.Dispose(disposing);
    }

    /// <summary>
    /// Layouts the subviews.
    /// </summary>
    public override void LayoutSubviews()
    {
        base.LayoutSubviews();

        DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
    }

    void SetUpDatePicker()
    {
        _picker = new UIDatePicker { Mode = UIDatePickerMode.Date, TimeZone = new Foundation.NSTimeZone("UTC") };
        if (UIDevice.CurrentDevice.CheckSystemVersion(13, 4))
        {
            _picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
        }

        var width = UIScreen.MainScreen.Bounds.Width;
        var toolbar = new UIToolbar(new CGRect(0, 0, width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };

        var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (o, e) =>
        {
            DummyField.ResignFirstResponder();
            _picker.Date = _preSelectedDate;
        });

        var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
        var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
        {
            DummyField.ResignFirstResponder();
            Done();
        });

        if (!string.IsNullOrEmpty(_DatePickerCell.TodayText))
        {
            var labelButton = new UIBarButtonItem(_DatePickerCell.TodayText, UIBarButtonItemStyle.Plain, (sender, e) =>
            {
                SetToday();
            });
            var fixspacer = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 20 };
            toolbar.SetItems(new[] { cancelButton, spacer, labelButton, fixspacer, doneButton }, false);
        }
        else
        {
            toolbar.SetItems(new[] { cancelButton, spacer, doneButton }, false);
        }

        DummyField.InputView = _picker;
        DummyField.InputAccessoryView = toolbar;
    }

    void SetToday()
    {
        if (_picker.MinimumDate.ToDateTime() <= DateTime.Today && _picker.MaximumDate.ToDateTime() >= DateTime.Today)
        {
            _picker.SetDate(DateTime.Today.ToNSDate(), true);
        }
    }

    void Done()
    {
        _DatePickerCell.Date = _picker.Date.ToDateTime().Date;
        ValueLabel.Text = _DatePickerCell.Date?.ToString(_DatePickerCell.Format);
        _preSelectedDate = _picker.Date;
    }

    internal void UpdateDate()
    {
        if (_DatePickerCell.Date.HasValue)
        {
            _picker.SetDate(_DatePickerCell.Date.Value.ToNSDate(), false);
            ValueLabel.Text = _DatePickerCell.Date.Value.ToString(_DatePickerCell.Format);
            _preSelectedDate = _DatePickerCell.Date.Value.ToNSDate();
        }
        else
        {
            _picker.SetDate(_DatePickerCell.InitialDate.ToNSDate(), false);
            ValueLabel.Text = string.Empty;
            _preSelectedDate = _DatePickerCell.InitialDate.ToNSDate();
        }
    }

    internal void UpdateMaximumDate()
    {
        _picker.MaximumDate = _DatePickerCell.MaximumDate.ToNSDate();
        UpdateDate();   //without this code, selected date isn't sometimes correct when it is shown first.
    }

    internal void UpdateMinimumDate()
    {
        _picker.MinimumDate = _DatePickerCell.MinimumDate.ToNSDate();
        UpdateDate();
    }

    internal void UpdateTodayText()
    {
        SetUpDatePicker();
        UpdateDate();
        UpdateMaximumDate();
        UpdateMinimumDate();
    }
}
