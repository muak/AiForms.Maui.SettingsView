using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Windows.Input;
using Microsoft.Maui.Platform;

namespace AiForms.Settings.Platforms.Droid;

/// <summary>
/// Command cell view.
/// </summary>
public class CommandCellView : LabelCellView
{
    internal Action Execute { get; set; }
    CommandCell _CommandCell => Cell as CommandCell;
    ICommand _command;
    ImageView _indicatorView;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.CommandCellView"/> class.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="cell">Cell.</param>
    public CommandCellView(Context context, CellBase cell) : base(context, cell)
    {
        if (!CellParent.ShowArrowIndicatorForAndroid)
        {
            return;
        }
        if (_CommandCell.HideArrowIndicator)
        {
            return;
        }
        _indicatorView = new ImageView(context);
        _indicatorView.SetImageResource(Resource.Drawable.ic_navigate_next);

        var param = new LinearLayout.LayoutParams(
            ViewGroup.LayoutParams.WrapContent,
            ViewGroup.LayoutParams.WrapContent)
        {
        };

        using (param)
        {
            AccessoryStack.AddView(_indicatorView, param);
        }
    }

    public CommandCellView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

    /// <summary>
    /// Cells the property changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        base.CellPropertyChanged(sender, e);        
    }

    public override void RowSelected(SettingsViewRecyclerAdapter adapter, int position)
    {
        Execute?.Invoke();
        if (_CommandCell.KeepSelectedUntilBack)
        {
            adapter.SelectedRow(this, position);
        }
    }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell()
    {
        base.UpdateCell();
        UpdateCommand();
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
            _indicatorView?.RemoveFromParent();
            _indicatorView?.SetImageDrawable(null);
            _indicatorView?.SetImageBitmap(null);
            _indicatorView?.Dispose();
            _indicatorView = null;
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
