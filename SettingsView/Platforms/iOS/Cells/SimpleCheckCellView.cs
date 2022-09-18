using System;
using System.ComponentModel;
using Foundation;
using Microsoft.Maui.Platform;
using UIKit;

namespace AiForms.Settings.Platforms.iOS;

[Foundation.Preserve(AllMembers = true)]
public class SimpleCheckCellView: CellBaseView
{
    SimpleCheckCell _simpleCheckCell => Cell as SimpleCheckCell;

    public SimpleCheckCellView(CellBase virtualCell): base(virtualCell)
    {
        SelectionStyle = UITableViewCellSelectionStyle.Default;
    }

    public override void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.ParentPropertyChanged(sender, e);
        if (e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName)
        {
            UpdateAccentColor();
        }        
    }

    public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
    {
        base.RowSelected(tableView, indexPath);

        tableView.DeselectRow(indexPath, true);
    }

    internal void UpdateChecked()
    {

        Accessory = _simpleCheckCell.Checked ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
    }

    internal void UpdateAccentColor()
    {
        if (_simpleCheckCell.AccentColor.IsNotDefault())
        {
            TintColor = _simpleCheckCell.AccentColor.ToPlatform();
        }
        else if (CellParent != null && CellParent.CellAccentColor.IsNotDefault())
        {
            TintColor = CellParent.CellAccentColor.ToPlatform();
        }
    }
}

