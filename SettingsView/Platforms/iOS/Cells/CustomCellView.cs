using System;
using System.Windows.Input;
using Foundation;
using UIKit;

namespace AiForms.Settings.Platforms.iOS;

[Foundation.Preserve(AllMembers = true)]
public class CustomCellView: CellBaseView
{
    protected CustomCell CustomCell => Cell as CustomCell;
    protected Action Execute { get; set; }
    protected ICommand _command;
    protected CustomCellContent _coreView;
    Dictionary<UIView, UIColor> _colorCache = new Dictionary<UIView, UIColor>();

    public CustomCellView(CellBase virtualCell) : base(virtualCell)
    {
        SelectionStyle = CustomCell.IsSelectable ? UITableViewCellSelectionStyle.Default : UITableViewCellSelectionStyle.None;
    }

    public override void UpdateConstraints()
    {
        base.UpdateConstraints();
        LayoutIfNeeded(); // let the layout immediately reflect when update constraints.
    }

    protected override void SetUpContentView()
    {
        base.SetUpContentView();

        if (CustomCell.ShowArrowIndicator)
        {
            Accessory = UITableViewCellAccessory.DisclosureIndicator;
            EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;

            SetRightMarginZero();
        }

        StackV.RemoveArrangedSubview(ContentStack);
        StackV.RemoveArrangedSubview(DescriptionLabel);
        ContentStack.RemoveFromSuperview();
        DescriptionLabel.RemoveFromSuperview();

        _coreView = new CustomCellContent();

        if (CustomCell.UseFullSize)
        {
            StackH.RemoveArrangedSubview(IconView);
            IconView.RemoveFromSuperview();

            StackH.LayoutMargins = new UIEdgeInsets(0, 0, 0, 0);
            StackH.Spacing = 0;
        }

        StackV.AddArrangedSubview(_coreView);
    }

    internal virtual void UpdateContent(UITableView tableView)
    {
        if (_coreView is null)
            return; // for HotReload;

        _coreView.CustomCell = CustomCell;
        _coreView.UpdateCell(CustomCell.Content, tableView);        
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
        if (!CustomCell.IsSelectable)
        {
            return;
        }
        Execute?.Invoke();
        if (!CustomCell.KeepSelectedUntilBack)
        {
            tableView.DeselectRow(indexPath, true);
        }
    }

    public override bool RowLongPressed(UITableView tableView, NSIndexPath indexPath)
    {
        if (CustomCell.LongCommand == null)
        {
            return false;
        }

        CustomCell.SendLongCommand();

        return true;
    }

    public override void SetHighlighted(bool highlighted, bool animated)
    {
        if (!highlighted)
        {
            base.SetHighlighted(highlighted, animated);
            return;
        }

        // https://stackoverflow.com/questions/6745919/uitableviewcell-subview-disappears-when-cell-is-selected

        BackupSubviewsColor(_coreView.Subviews[0], _colorCache);

        base.SetHighlighted(highlighted, animated);

        RestoreSubviewsColor(_coreView.Subviews[0], _colorCache);
    }

    public override void SetSelected(bool selected, bool animated)
    {
        if (!selected)
        {
            base.SetSelected(selected, animated);
            return;
        }

        base.SetSelected(selected, animated);

        RestoreSubviewsColor(_coreView.Subviews[0], _colorCache);
    }

    void BackupSubviewsColor(UIView view, Dictionary<UIView, UIColor> colors)
    {
        colors[view] = view.BackgroundColor;

        foreach (UIView subView in view.Subviews)
        {
            BackupSubviewsColor(subView, colors);
        }
    }

    void RestoreSubviewsColor(UIView view, Dictionary<UIView, UIColor> colors)
    {
        if (colors.TryGetValue(view, out var color))
        {
            view.BackgroundColor = color;
        }

        foreach (UIView subView in view.Subviews)
        {
            RestoreSubviewsColor(subView, colors);
        }
    }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell(UITableView tableView)
    {
        base.UpdateCell(tableView);
        UpdateContent(tableView);
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

            _colorCache.Clear();
            _colorCache = null;

            _coreView?.RemoveFromSuperview();
            _coreView?.Dispose();
            _coreView = null;
        }

        base.Dispose(disposing);
    }

    internal virtual void UpdateCommand()
    {
        if (_command != null)
        {
            _command.CanExecuteChanged -= Command_CanExecuteChanged;
        }

        _command = CustomCell.Command;

        if (_command != null)
        {
            _command.CanExecuteChanged += Command_CanExecuteChanged;
            Command_CanExecuteChanged(_command, System.EventArgs.Empty);
        }

        Execute = () => {
            if (_command == null)
            {
                return;
            }
            if (_command.CanExecute(CustomCell.CommandParameter))
            {
                _command.Execute(CustomCell.CommandParameter);
            }
        };

    }

    /// <summary>
    /// Updates the is enabled.
    /// </summary>
    internal override void UpdateIsEnabled()
    {
        if (_command != null && !_command.CanExecute(CustomCell.CommandParameter))
        {
            return;
        }
        base.UpdateIsEnabled();
    }

    protected virtual void Command_CanExecuteChanged(object sender, EventArgs e)
    {
        if (!Cell.IsEnabled)
        {
            return;
        }

        SetEnabledAppearance(_command.CanExecute(CustomCell.CommandParameter));
    }
}

