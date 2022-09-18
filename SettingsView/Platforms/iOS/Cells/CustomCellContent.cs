using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using AiForms.Settings.Extensions;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Platform;
using UIKit;

namespace AiForms.Settings.Platforms.iOS;

[Foundation.Preserve(AllMembers = true)]
public class CustomCellContent: UIView
{
    WeakReference<IPlatformViewHandler> _handlerRef;
    bool _disposed;
    NSLayoutConstraint _heightConstraint;
    View _virtualCell;
    public CustomCell CustomCell { get; set; }
    double _lastFrameWidth = -9999d;
    double _lastMeasureWidth = -9999d;
    double _lastMeasureHeight = -9999d;
    IMauiContext _mauiContext => _virtualCell.FindMauiContext();

    public CustomCellContent()
    {
    }

    protected override void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            if (_virtualCell != null)
            {
                _virtualCell.PropertyChanged -= CellPropertyChanged;
                _virtualCell.MeasureInvalidated -= OnMeasureInvalidated;
            }

            CustomCell = null;

            _heightConstraint?.Dispose();
            _heightConstraint = null;

            IPlatformViewHandler handler = null;
            if (_handlerRef != null && _handlerRef.TryGetTarget(out handler) && handler.VirtualView != null)
            {
                handler.VirtualView.DisposeModalAndChildHandlers();
                _handlerRef = null;
            }
            handler?.DisconnectHandler();
            _virtualCell = null;
        }

        _disposed = true;

        base.Dispose(disposing);
    }

    private void OnMeasureInvalidated(object sender, EventArgs e)
    {
        SetNeedsLayout();
    }

    public virtual void UpdateNativeCell()
    {
        UpdateIsEnabled();
    }

    public virtual void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == CellBase.IsEnabledProperty.PropertyName)
        {
            UpdateIsEnabled();
        }
    }

    protected virtual void UpdateIsEnabled()
    {
        UserInteractionEnabled = _virtualCell.IsEnabled;
    }

    public virtual void UpdateCell(View cell, UITableView tableView)
    {
        if (_virtualCell == cell && !CustomCell.IsForceLayout)
        {
            return;
        }

        var oldCell = _virtualCell;

        if (oldCell != null)
        {
            oldCell.Handler?.DisconnectHandler();
            oldCell.PropertyChanged -= CellPropertyChanged;
            oldCell.MeasureInvalidated -= OnMeasureInvalidated;
        }

        _virtualCell = cell;
        _virtualCell.PropertyChanged += CellPropertyChanged;

        IPlatformViewHandler handler;
        if (_handlerRef == null || !_handlerRef.TryGetTarget(out handler))
        {
            handler = GetNewHandler();
        }
        else
        {
            if (CustomCell.IsForceLayout)
            {
                handler.PlatformView.RemoveFromSuperview();
                handler.DisposeHandlersAndChildren();                
                handler = GetNewHandler();
                CustomCell.IsForceLayout = false;
            }

            var viewHandlerType = _mauiContext.Handlers.GetHandlerType(_virtualCell.GetType());
            var reflectableType = handler as System.Reflection.IReflectableType;
            var handlerType = reflectableType != null ? reflectableType.GetTypeInfo().AsType() : (handler != null ? handler.GetType() : typeof(System.Object));

            if (handlerType == viewHandlerType)
            {
                handler.SetVirtualView(this._virtualCell);
            }
            else
            {
                //when cells are getting reused the element could be already set to another cell
                //so we should dispose based on the renderer and not the renderer.Element
                handler.DisposeHandlersAndChildren();
                handler = GetNewHandler();
            }
        }

        _virtualCell.MeasureInvalidated += OnMeasureInvalidated;

        if (!CustomCell.IsMeasureOnce || tableView.Frame.Width != _lastFrameWidth)
        {
            _lastFrameWidth = tableView.Frame.Width;
            var height = double.PositiveInfinity;
            var width = tableView.Frame.Width - (CustomCell.UseFullSize ? 0 : 32); // CellBaseView layout margin
            var result = handler.VirtualView.Measure(tableView.Frame.Width, height);
            _lastMeasureWidth = result.Width;
            if (_virtualCell.HorizontalOptions.Alignment == LayoutAlignment.Fill)
            {
                _lastMeasureWidth = width;
            }
            _lastMeasureHeight = result.Height;

            if (_heightConstraint != null)
            {
                _heightConstraint.Active = false;
                _heightConstraint?.Dispose();
            }

            _heightConstraint = handler.PlatformView.HeightAnchor.ConstraintEqualTo((NFloat)_lastMeasureHeight);
            _heightConstraint.Priority = 999f;
            _heightConstraint.Active = true;

            handler.PlatformView.UpdateConstraintsIfNeeded();
        }

        //Layout.LayoutChildIntoBoundingRegion(_virtualCell, new Rectangle(0, 0, _lastMeasureWidth, _lastMeasureHeight));

        UpdateNativeCell();
    }


    protected virtual IPlatformViewHandler GetNewHandler()
    {
        if(_virtualCell == null)
        {
            throw new InvalidOperationException("CustomCell must have a view");
        }

        var newHandler = _virtualCell.ToHandler(_virtualCell.FindMauiContext());
        _handlerRef = new WeakReference<IPlatformViewHandler>(newHandler);
        AddSubview(newHandler.PlatformView);

        var native = newHandler.PlatformView;
        native.TranslatesAutoresizingMaskIntoConstraints = false;

        native.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
        native.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
        native.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
        native.RightAnchor.ConstraintEqualTo(RightAnchor).Active = true;

        return newHandler;
    }
}

