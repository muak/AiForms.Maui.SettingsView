﻿using System;
using System.Windows.Input;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Platform;
using UIKit;

namespace AiForms.Settings.Platforms.iOS;

/// <summary>
/// Number picker cell view.
/// </summary>
[Foundation.Preserve(AllMembers = true)]
public class NumberPickerCellView : LabelCellView
{
    /// <summary>
    /// Gets or sets the dummy field.
    /// </summary>
    /// <value>The dummy field.</value>
    public UITextField DummyField { get; set; }
    NumberPickerSource _model;
    UILabel _titleLabel;
    UIPickerView _picker;
    ICommand _command;

    NumberPickerCell _NumberPickerCell => Cell as NumberPickerCell;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.NumberPickerCellView"/> class.
    /// </summary>
    /// <param name="virtualCell">Forms cell.</param>
    public NumberPickerCellView(CellBase virtualCell) : base(virtualCell)
    {

        DummyField = new NoCaretField();
        DummyField.BorderStyle = UITextBorderStyle.None;
        DummyField.BackgroundColor = UIColor.Clear;
        ContentView.AddSubview(DummyField);
        ContentView.SendSubviewToBack(DummyField);

        SelectionStyle = UITableViewCellSelectionStyle.Default;

        SetUpPicker();
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
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell(UITableView tableView)
    {
        base.UpdateCell(tableView);
        if (DummyField is null)
            return; // For HotReload

        //UpdateNumberList();
        //UpdateNumber();
        //UpdateTitle();
        //UpdateCommand();
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
            _model.UpdatePickerFromModel -= Model_UpdatePickerFromModel;
            DummyField.RemoveFromSuperview();
            DummyField?.Dispose();
            DummyField = null;
            _titleLabel?.Dispose();
            _titleLabel = null;
            _model?.Dispose();
            _model = null;
            _picker?.Dispose();
            _picker = null;
            _command = null;

        }
        base.Dispose(disposing);
    }

    void SetUpPicker()
    {
        _picker = new UIPickerView();

        var width = UIScreen.MainScreen.Bounds.Width;

        _titleLabel = new UILabel();
        _titleLabel.TextAlignment = UITextAlignment.Center;

        var toolbar = new UIToolbar(new CGRect(0, 0, (float)width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
        var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (o, e) =>
        {
            DummyField.ResignFirstResponder();
            Select(_model.PreSelectedItem);
        });

        var labelButton = new UIBarButtonItem(_titleLabel);
        var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
        var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
        {
            _model.OnUpdatePickerFormModel();
            DummyField.ResignFirstResponder();
            _command?.Execute(_model.SelectedItem);
        });

        toolbar.SetItems(new[] { cancelButton, spacer, labelButton, spacer, doneButton }, false);

        DummyField.InputView = _picker;
        DummyField.InputAccessoryView = toolbar;

        _model = new NumberPickerSource(_NumberPickerCell.Unit);
        _picker.Model = _model;

        _model.UpdatePickerFromModel += Model_UpdatePickerFromModel;
    }

    internal virtual void UpdateNumber()
    {
        Select(_NumberPickerCell.Number);
        ValueLabel.Text = FormatNumber(_NumberPickerCell.Number);
    }

    private string FormatNumber(int? number)
    {
        return number.HasValue && !String.IsNullOrEmpty(_NumberPickerCell.Unit)
            ? $"{number} {_NumberPickerCell.Unit}"
            : number?.ToString() ?? "";
    }

    internal void UpdateNumberList()
    {
        _model.SetNumbers(_NumberPickerCell.Min, _NumberPickerCell.Max);
        Select(_NumberPickerCell.Number);
    }

    internal void UpdateTitle()
    {
        _titleLabel.Text = _NumberPickerCell.PickerTitle;
        _titleLabel.SizeToFit();
        _titleLabel.Frame = new CGRect(0, 0, 160, 44);
    }

    internal void UpdateCommand()
    {
        _command = _NumberPickerCell.SelectedCommand;
    }

    void Model_UpdatePickerFromModel(object sender, EventArgs e)
    {
        _NumberPickerCell.Number = _model.SelectedItem;
        ValueLabel.Text = FormatNumber(_model.SelectedItem);
    }

    /// <summary>
    /// Layouts the subviews.
    /// </summary>
    public override void LayoutSubviews()
    {
        base.LayoutSubviews();
        if (DummyField is null)
            return; // For HotReload

        DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
    }

    void Select(int? number)
    {
        var idx = number.HasValue ? _model.Items.IndexOf(number.Value) : -1;
        if (idx == -1)
        {
            number = _model.Items[0];
            idx = 0;
        }
        _picker.Select(idx, 0, false);
        _model.SelectedItem = number.Value;
        _model.SelectedIndex = idx;
        _model.PreSelectedItem = number.Value;
    }
}
