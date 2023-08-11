using System;
using System.ComponentModel;
using AiForms.Settings.Extensions;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Platform;
using ObjCRuntime;
using SpriteKit;
using UIKit;

namespace AiForms.Settings.Platforms.iOS;

public class CustomHeaderView : CustomHeaderFooterView
{
    public CustomHeaderView()
    {
    }    

    public CustomHeaderView(NSCoder coder) : base(coder)
    {
    }

    public CustomHeaderView(CGRect frame) : base(frame)
    {
    }

    public CustomHeaderView(NSString reuseIdentifier) : base(reuseIdentifier)
    {
    }

    protected CustomHeaderView(NSObjectFlag t) : base(t)
    {
    }

    protected internal CustomHeaderView(NativeHandle handle) : base(handle)
    {
    }
}

public class CustomFooterView : CustomHeaderFooterView
{
    public CustomFooterView()
    {
    }    

    public CustomFooterView(NSString reuseIdentifier) : base(reuseIdentifier)
    {
    }

    public CustomFooterView(NSCoder coder) : base(coder)
    {
    }

    public CustomFooterView(CGRect frame) : base(frame)
    {
    }

    protected CustomFooterView(NSObjectFlag t) : base(t)
    {
    }

    protected internal CustomFooterView(NativeHandle handle) : base(handle)
    {
    }
}

public class CustomHeaderFooterView:UITableViewHeaderFooterView
{
    WeakReference<IPlatformViewHandler> _handlerRef;
    bool _disposed;
    NSLayoutConstraint _heightConstraint;
    View _virtualCell;
    IMauiContext _mauiContext => _virtualCell.FindMauiContext();    

    public CustomHeaderFooterView()
    {
    }

    public CustomHeaderFooterView(NSCoder coder) : base(coder)
    {
    }

    protected CustomHeaderFooterView(NSObjectFlag t) : base(t)
    {
    }

    protected internal CustomHeaderFooterView(NativeHandle handle) : base(handle)
    {
    }

    public CustomHeaderFooterView(CGRect frame) : base(frame)
    {
    }

    public CustomHeaderFooterView(NSString reuseIdentifier) : base(reuseIdentifier)
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

            _heightConstraint?.Dispose();
            _heightConstraint = null;

            IPlatformViewHandler handler;
            if (_handlerRef != null && _handlerRef.TryGetTarget(out handler) && handler.VirtualView != null)
            {
                handler.VirtualView.DisposeModalAndChildHandlers();
                _handlerRef = null;
            }        
            _virtualCell = null;
        }

        _disposed = true;

        base.Dispose(disposing);
    }

    public virtual void UpdateNativeCell()
    {
        UpdateIsEnabled();
    }

    public virtual void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == Cell.IsEnabledProperty.PropertyName)
        {
            UpdateIsEnabled();
        }
    }

    protected virtual void UpdateIsEnabled()
    {
        UserInteractionEnabled = _virtualCell.IsEnabled;
    }       
      
    public virtual void UpdateCell(View cell,UITableView tableView)
    {
        if(_virtualCell == cell)
        {
            return;
        }

        var oldCell = _virtualCell;

        if(oldCell != null)
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
        

        var height = double.PositiveInfinity;
        var result = handler.VirtualView.Measure(tableView.Frame.Width, height);
        var finalW = result.Width;
        if(_virtualCell.HorizontalOptions.Alignment == LayoutAlignment.Fill)
        {
            finalW = tableView.Frame.Width;
        }
        var finalH = (float)result.Height;           

        UpdateNativeCell();

        if (_heightConstraint != null)
        {
            _heightConstraint.Active = false;
            _heightConstraint?.Dispose();
        }

        _heightConstraint = handler.PlatformView.HeightAnchor.ConstraintEqualTo(finalH);
        _heightConstraint.Priority = 999f;
        _heightConstraint.Active = true;           

        // TODO: 不要になったかも。
        // Layout.LayoutChildIntoBoundingRegion(_virtualCell, new Rectangle(0, 0, finalW, finalH));

        handler.PlatformView.UpdateConstraintsIfNeeded();
    }

    private void OnMeasureInvalidated(object sender, EventArgs e)
    {
        SetNeedsLayout();
    }

    protected virtual IPlatformViewHandler GetNewHandler()
    {
        if(_virtualCell == null)
        {
            throw new InvalidOperationException("Hearder or Footer must have a view");
        }

        var newHandler = _virtualCell.ToHandler(_virtualCell.FindMauiContext());
        _handlerRef = new WeakReference<IPlatformViewHandler>(newHandler);
        ContentView.AddSubview(newHandler.PlatformView);

        var native = newHandler.PlatformView;
        native.TranslatesAutoresizingMaskIntoConstraints = false;

        native.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
        native.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
        native.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
        native.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;

        return newHandler;
    }
}
