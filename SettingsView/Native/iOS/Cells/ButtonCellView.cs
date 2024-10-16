using System;
using System.Windows.Input;
using Foundation;
using UIKit;

namespace AiForms.Settings.Platforms.iOS;

/// <summary>
/// Button cell view.
/// </summary>
[Foundation.Preserve(AllMembers = true)]
public class ButtonCellView : CellBaseView
{
    Action Execute { get; set; }
    ButtonCell _ButtonCell => Cell as ButtonCell;
    ICommand _command;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.ButtonCellView"/> class.
    /// </summary>
    /// <param name="virtualCell">Forms cell.</param>
    public ButtonCellView(CellBase virtualCell) : base(virtualCell)
    {
        DescriptionLabel.Hidden = true;
        SelectionStyle = UIKit.UITableViewCellSelectionStyle.Default;
        TitleLabel.TextAlignment = UIKit.UITextAlignment.Right;
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
        Execute?.Invoke();
    }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell(UITableView tableView)
    {
        base.UpdateCell(tableView);
        if (TitleLabel is null)
            return; // For HotReload

        //UpdateCommand();
        //UpdateTitleAlignment();
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

    internal void UpdateTitleAlignment()
    {
        TitleLabel.TextAlignment = _ButtonCell.TitleAlignment.ToUITextAlignment();
    }

    internal void UpdateCommand()
    {
        if (_command != null)
        {
            _command.CanExecuteChanged -= Command_CanExecuteChanged;
        }

        _command = _ButtonCell.Command;

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
            if (_command.CanExecute(_ButtonCell.CommandParameter))
            {
                _command.Execute(_ButtonCell.CommandParameter);
            }
        };

    }

    /// <summary>
    /// Updates the is enabled.
    /// </summary>
    internal override void UpdateIsEnabled()
    {
        if (_command != null && !_command.CanExecute(_ButtonCell.CommandParameter))
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

        SetEnabledAppearance(_command.CanExecute(_ButtonCell.CommandParameter));
    }
}

