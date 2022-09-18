using System;
using Android.Widget;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Platform;
using ARelativeLayout = Android.Widget.RelativeLayout;

namespace AiForms.Settings.Platforms.Droid;

public class CustomCellView: CellBaseView
{
    protected Action Execute { get; set; }
    protected CustomCell CustomCell => Cell as CustomCell;
    protected ICommand _command;
    protected ImageView _indicatorView;
    protected LinearLayout _coreView;
    FormsViewContainer _container;

    public CustomCellView(Context context, CellBase cell) : base(context, cell)
    {
        if (!CustomCell.ShowArrowIndicator)
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

    public CustomCellView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

    protected override void CreateContentView()
    {
        base.CreateContentView();

        _container = new FormsViewContainer(Context);


        _coreView = FindViewById<LinearLayout>(Resource.Id.CellBody);
        ContentStack.RemoveFromParent();
        DescriptionLabel.RemoveFromParent();

        if (CustomCell.UseFullSize)
        {
            IconView.RemoveFromParent();
            var layout = FindViewById<ARelativeLayout>(Resource.Id.CellLayout);
            var rMargin = CustomCell.ShowArrowIndicator ? _Context.ToPixels(10) : 0;
            layout.SetPadding(0, 0, (int)rMargin, 0);
            _coreView.SetPadding(0, 0, 0, 0);
        }

        _coreView.AddView(_container);
    }

    public void UpdateContent()
    {
        _container.CustomCell = CustomCell;
        _container.ContentView = CustomCell.Content;
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

    public override void RowSelected(SettingsViewRecyclerAdapter adapter, int position)
    {
        if (!CustomCell.IsSelectable)
        {
            return;
        }
        Execute?.Invoke();
        if (CustomCell.KeepSelectedUntilBack)
        {
            adapter.SelectedRow(this, position);
        }
    }

    public override bool RowLongPressed(SettingsViewRecyclerAdapter adapter, int position)
    {
        if (CustomCell.LongCommand == null)
        {
            return false;
        }

        CustomCell.SendLongCommand();

        return true;
    }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell()
    {
        base.UpdateCell();
        UpdateContent();
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
            _indicatorView?.RemoveFromParent();
            _indicatorView?.SetImageDrawable(null);
            _indicatorView?.SetImageBitmap(null);
            _indicatorView?.Dispose();
            _indicatorView = null;

            _coreView?.RemoveFromParent();
            _coreView?.Dispose();
            _coreView = null;

            _container?.RemoveFromParent();
            _container?.Dispose();
            _container = null;
        }
        base.Dispose(disposing);
    }

    internal void UpdateCommand()
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

    void Command_CanExecuteChanged(object sender, EventArgs e)
    {
        if (!Cell.IsEnabled)
        {
            return;
        }

        SetEnabledAppearance(_command.CanExecute(CustomCell.CommandParameter));
    }
}

