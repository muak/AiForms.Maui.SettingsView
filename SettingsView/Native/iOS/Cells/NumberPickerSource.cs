﻿using System;
using System.Collections.Generic;
using UIKit;

namespace AiForms.Settings.Platforms.iOS;

[Foundation.Preserve(AllMembers = true)]
internal class NumberPickerSource : UIPickerViewModel
{
    private readonly string _unit;

    public NumberPickerSource(string unit)
    {
        _unit = unit;
    }

    internal IList<int> Items { get; private set; }

    internal event EventHandler UpdatePickerFromModel;

    internal int SelectedIndex { get; set; }

    internal int SelectedItem { get; set; }

    internal int PreSelectedItem { get; set; }

    /// <summary>
    /// Gets the component count.
    /// </summary>
    /// <returns>The component count.</returns>
    /// <param name="picker">Picker.</param>
    public override nint GetComponentCount(UIPickerView picker)
    {
        return 1;
    }

    /// <summary>
    /// Gets the rows in component.
    /// </summary>
    /// <returns>The rows in component.</returns>
    /// <param name="pickerView">Picker view.</param>
    /// <param name="component">Component.</param>
    public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
    {
        return Items != null ? Items.Count : 0;
    }

    /// <summary>
    /// Gets the title.
    /// </summary>
    /// <returns>The title.</returns>
    /// <param name="picker">Picker.</param>
    /// <param name="row">Row.</param>
    /// <param name="component">Component.</param>
    public override string GetTitle(UIPickerView picker, nint row, nint component)
    {
        int number = SelectItem((int)row);
        return !String.IsNullOrEmpty(_unit)
            ? $"{number} {_unit}"
            : number.ToString();
    }

    /// <summary>
    /// Selected the specified picker, row and component.
    /// </summary>
    /// <returns>The selected.</returns>
    /// <param name="picker">Picker.</param>
    /// <param name="row">Row.</param>
    /// <param name="component">Component.</param>
    public override void Selected(UIPickerView picker, nint row, nint component)
    {

        if (Items.Count == 0)
        {
            SelectedItem = 0;
            SelectedIndex = -1;
        }
        else
        {
            SelectedItem = SelectItem((int)row);
            SelectedIndex = (int)row;
        }

    }

    /// <summary>
    /// Sets the numbers.
    /// </summary>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    public void SetNumbers(int min, int max)
    {
        if (min < 0) min = 0;
        if (max < 0) max = 0;
        if (min > max)
        {
            //Set min value to zero temporally, because it is sometimes min greater than max depending on the order which min and max value is bound.
            min = 0;
        }
        Items = Enumerable.Range(min, max - min + 1).ToList();
    }

    /// <summary>
    /// Ons the update picker form model.
    /// </summary>
    public void OnUpdatePickerFormModel()
    {
        PreSelectedItem = SelectedItem;
        UpdatePickerFromModel?.Invoke(this, EventArgs.Empty);
    }
    private int SelectItem(int row)
    {
        if (row >= Items.Count)
        {
            return Items[Items.Count - 1];
        }
        if (row < 0)
        {
            return Items[0];
        }
        return Items[row];
    }
}
