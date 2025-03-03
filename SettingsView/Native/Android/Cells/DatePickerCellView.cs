﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

namespace AiForms.Settings.Platforms.Droid;

/// <summary>
/// Date picker cell view.
/// </summary>
[Android.Runtime.Preserve(AllMembers = true)]
public class DatePickerCellView : LabelCellView
{
    DatePickerCell _datePickerCell => Cell as DatePickerCell;
    DatePickerDialog _dialog;
    Context _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.DatePickerCellView"/> class.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="cell">Cell.</param>
    public DatePickerCellView(Context context, CellBase cell) : base(context, cell)
    {
        _context = context;
    }

    public DatePickerCellView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell()
    {
        base.UpdateCell();
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
    /// <param name="adapter">Adapter.</param>
    /// <param name="position">Position.</param>
    public override void RowSelected(SettingsViewRecyclerAdapter adapter, int position)
    {
        ShowDialog();
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
            if (_dialog != null)
            {
                _dialog.CancelEvent -= OnCancelButtonClicked;
                _dialog?.Dispose();
                _dialog = null;
            }
        }
        base.Dispose(disposing);
    }

    void ShowDialog()
    {
        if (_datePickerCell.Date.HasValue)
        {
            CreateDatePickerDialog(_datePickerCell.Date.Value.Year, _datePickerCell.Date.Value.Month - 1, _datePickerCell.Date.Value.Day);
        }
        else
        {
            CreateDatePickerDialog(_datePickerCell.InitialDate.Year, _datePickerCell.InitialDate.Month - 1, _datePickerCell.InitialDate.Day);
        }



        UpdateMinimumDate();
        UpdateMaximumDate();

        if (_datePickerCell.MinimumDate > _datePickerCell.MaximumDate)
        {
            throw new ArgumentOutOfRangeException(
                nameof(DatePickerCell.MaximumDate),
                "MaximumDate must be greater than or equal to MinimumDate."
            );
        }

        _dialog.CancelEvent += OnCancelButtonClicked;

        _dialog.Show();

        if (!_datePickerCell.AndroidButtonColor.IsDefault())
        {
            _dialog.GetButton((int)DialogButtonType.Positive).SetTextColor(_datePickerCell.AndroidButtonColor.ToAndroid());
            _dialog.GetButton((int)DialogButtonType.Negative).SetTextColor(_datePickerCell.AndroidButtonColor.ToAndroid());
        }
    }

    void CreateDatePickerDialog(int year, int month, int day)
    {
        EventHandler<DatePickerDialog.DateSetEventArgs> callback = (o, e) => {
            _datePickerCell.Date = e.Date;
            ClearFocus();
            if (_dialog != null)
            {
                _dialog.CancelEvent -= OnCancelButtonClicked;
            }

            _dialog = null;
        };

        if (_datePickerCell.IsAndroidSpinnerStyle)
        {
            _dialog = new DatePickerDialog(_context, global::AiForms.Settings.Resource.Style.datePickerDialogSpinnerTheme, callback, year, month, day);
        }
        else
        {
            _dialog = new DatePickerDialog(_context, callback, year, month, day);
        }
    }

    void OnCancelButtonClicked(object sender, EventArgs e)
    {
        ClearFocus();
    }

    internal void UpdateDate()
    {
        if (_datePickerCell.Date.HasValue)
        {
            var format = _datePickerCell.Format;
            vValueLabel.Text = _datePickerCell.Date.Value.ToString(format);
        }
        else
        {
            vValueLabel.Text = string.Empty;
        }
    }

    internal void UpdateMaximumDate()
    {
        if (_dialog != null)
        {
            //when not to specify 23:59:59,last day can't be selected.
            _dialog.DatePicker.MaxDate = (long)_datePickerCell.MaximumDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
        }
    }

    internal void UpdateMinimumDate()
    {
        if (_dialog != null)
        {
            _dialog.DatePicker.MinDate = (long)_datePickerCell.MinimumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
        }
    }

}

