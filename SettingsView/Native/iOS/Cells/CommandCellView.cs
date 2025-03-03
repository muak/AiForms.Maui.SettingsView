﻿using System;
using System;
using UIKit;
using System.Windows.Input;
using Foundation;

namespace AiForms.Settings.Platforms.iOS;

/// <summary>
/// Command cell view.
/// </summary>
[Foundation.Preserve(AllMembers = true)]
public class CommandCellView : LabelCellView
{
    internal Action Execute { get; set; }
    CommandCell _CommandCell => Cell as CommandCell;
    ICommand _command;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.CommandCellView"/> class.
    /// </summary>
    /// <param name="virtualCell">Forms cell.</param>
    public CommandCellView(CellBase virtualCell) : base(virtualCell)
    {
        SelectionStyle = UITableViewCellSelectionStyle.Default;

        if (!_CommandCell.HideArrowIndicator)
        {
            Accessory = UITableViewCellAccessory.DisclosureIndicator;
            EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;
            SetRightMarginZero();
        }
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
        Execute?.Invoke();
        if (!_CommandCell.KeepSelectedUntilBack)
        {
            tableView.DeselectRow(indexPath, true);
        }
    }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell(UITableView tableView)
    {
        base.UpdateCell(tableView);
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
            if (_command != null)
            {
                _command.CanExecuteChanged -= Command_CanExecuteChanged;
            }
            Execute = null;
            _command = null;
        }
        base.Dispose(disposing);
    }

    internal void UpdateCommand()
    {
        if (_command != null)
        {
            _command.CanExecuteChanged -= Command_CanExecuteChanged;
        }

        _command = _CommandCell.Command;

        if (_command != null)
        {
            _command.CanExecuteChanged += Command_CanExecuteChanged;
            Command_CanExecuteChanged(_command, System.EventArgs.Empty);
        }

        Execute = () =>
        {
            if (_command == null)
            {
                return;
            }
            if (_command.CanExecute(_CommandCell.CommandParameter))
            {
                _command.Execute(_CommandCell.CommandParameter);
            }
        };

    }

    /// <summary>
    /// Updates the is enabled.
    /// </summary>
    internal override void UpdateIsEnabled()
    {
        if (_command != null && !_command.CanExecute(_CommandCell.CommandParameter))
        {
            return;
        }
        base.UpdateIsEnabled();
    }

    void Command_CanExecuteChanged(object sender, EventArgs e)
    {
        if (!Cell.IsEnabled)
        {
            return;
        }

        SetEnabledAppearance(_command.CanExecute(_CommandCell.CommandParameter));
    }
}

