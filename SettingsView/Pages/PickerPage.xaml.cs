using Microsoft.Maui;
using System.Collections;
using System.Runtime.InteropServices;

namespace AiForms.Settings.Pages;

public partial class PickerPage : ContentPage
{
    public List<DisplayValue> ItemsSource { get; } = new List<DisplayValue>();

    PickerCell _pickerCell;
    SettingsView _parent;
    HashSet<DisplayValue> _selectedCache = new HashSet<DisplayValue>();

    public PickerPage(PickerCell cell)
    {
        InitializeComponent();

        settingsView.Model.RowSelected += Model_RowSelected;    

        _pickerCell = cell;
        _parent = _pickerCell.Parent as SettingsView;

        SetUpProperties();

        foreach (var item in _pickerCell.ItemsSource)
        {
            var isSelected = _pickerCell.SelectedItems.IndexOf(item) >= 0;           
            var displayValue = new DisplayValue($"{_pickerCell.DisplayValue(item)}", $"{_pickerCell.SubDisplayValue(item)}", isSelected, item);
            ItemsSource.Add(displayValue);

            if (isSelected)
            {
                _selectedCache.Add(displayValue);
            }
        }

        BindingContext = this;
    }

    private void Model_RowSelected(CellBase cell)
    {
        UpdateSelected(cell.BindingContext as DisplayValue);
    }

    void SetUpProperties()
    {
        Title = _pickerCell.Title;
        settingsView.SeparatorColor = _parent.SeparatorColor;
        settingsView.BackgroundColor = _parent.BackgroundColor;
        settingsView.SelectedColor = _parent.SelectedColor;

        if (_pickerCell.AccentColor.IsNotDefault())
        {
            settingsView.CellAccentColor = _pickerCell.AccentColor;
        }
        else if (_parent.CellAccentColor.IsNotDefault())
        {
            settingsView.CellAccentColor = _parent.CellAccentColor;
        }

        if (_pickerCell.TitleColor.IsNotDefault())
        {
            settingsView.CellTitleColor = _pickerCell.TitleColor;
        }
        else if (_parent != null && _parent.CellTitleColor.IsNotDefault())
        {
            settingsView.CellTitleColor = _parent.CellTitleColor;
        }

        if (_pickerCell.TitleFontSize > 0)
        {
            settingsView.CellTitleFontSize = _pickerCell.TitleFontSize;
        }
        else if (_parent != null)
        {
            settingsView.CellTitleFontSize = _parent.CellTitleFontSize;
        }

        if (_pickerCell.DescriptionColor.IsNotDefault())
        {
            settingsView.CellDescriptionColor = _pickerCell.DescriptionColor;
        }
        else if (_parent != null && _parent.CellDescriptionColor.IsNotDefault())
        {
            settingsView.CellDescriptionColor = _parent.CellDescriptionColor;
        }

        if (_pickerCell.DescriptionFontSize > 0)
        {
            settingsView.CellDescriptionFontSize = _pickerCell.DescriptionFontSize;
        }
        else if (_parent != null)
        {
            settingsView.CellDescriptionFontSize = _parent.CellDescriptionFontSize;
        }

        if (_pickerCell.BackgroundColor.IsNotDefault())
        {
            settingsView.CellBackgroundColor = _pickerCell.BackgroundColor;
        }
        else if (_parent != null && _parent.CellBackgroundColor.IsNotDefault())
        {
            settingsView.CellBackgroundColor = _parent.CellBackgroundColor;
        }

        if(_pickerCell.SelectedItems == null)
        {
            _pickerCell.SelectedItems = new ArrayList();
            if (_pickerCell.SelectedItem != null)
            {
                _pickerCell.SelectedItems.Add(_pickerCell.SelectedItem);
            }
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        settingsView.Model.RowSelected -= Model_RowSelected;

        _pickerCell.SelectedItems.Clear();

        foreach(var item in _selectedCache)
        {
            _pickerCell.SelectedItems.Add(item.Value);
        }

        _pickerCell.SelectedItem = _selectedCache.FirstOrDefault()?.Value;

        _pickerCell.InvokeCommand();

        _parent = null;
        _pickerCell = null;
        _selectedCache = null;
    } 

    void UpdateSelected(DisplayValue item)
    {
        if(_pickerCell.MaxSelectedNumber == 1)
        {
            SelectedSingle(item);
            DoPickToClose();
        }
        else
        {
            SelectedMulti(item);
        }
    }

    void SelectedSingle(DisplayValue item)
    {
        if (_selectedCache.Contains(item))
        {
            return;
        }

        foreach(var dv in ItemsSource)
        {
            dv.IsSelected = false;
        }

        item.IsSelected = true;

        _selectedCache.Clear();
        _selectedCache.Add(item);
    }

    void SelectedMulti(DisplayValue item)
    {
        if (_selectedCache.Contains(item))
        {
            item.IsSelected = false;
            _selectedCache.Remove(item);
            return;
        }

        if(_pickerCell.MaxSelectedNumber != 0 && _selectedCache.Count() >= _pickerCell.MaxSelectedNumber)
        {
            return;
        }

        item.IsSelected = true;
        _selectedCache.Add(item);

        DoPickToClose();
    }

    void DoPickToClose()
    {
        if(_pickerCell.UsePickToClose && _selectedCache.Count == _pickerCell.MaxSelectedNumber)
        {
            Navigation.PopAsync(true);
        }
    }
}

public class DisplayValue : BindableBase
{
    
    public object Value { get; set; }

    private string _Display;
    public string Display
    {
        get => _Display;
        set => SetProperty(ref _Display, value);
    }

    private string _SubDisplay;
    public string SubDisplay
    {
        get => _SubDisplay;
        set => SetProperty(ref _SubDisplay, value);
    }


    private bool _IsSelected;
    public bool IsSelected
    {
        get => _IsSelected;
        set => SetProperty(ref _IsSelected, value);
    }

    public DisplayValue(string display, string subDisplay, bool selected, object value)
    {
        Display = display;
        SubDisplay = subDisplay;
        IsSelected = selected;
        Value = value;
    }
}
