using System;
using System.ComponentModel;
using AiForms.Settings.Extensions;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Handlers;
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
                _virtualCell.DisposeModalAndChildHandlers();
            }

            _heightConstraint?.Dispose();
            _heightConstraint = null;
     
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
      
    public virtual void UpdateCell(View newCell,UITableView tableView)
    {
        // Workaround GestureRecognizer bug
        // TODO: if fix this issue, remove the following code.
        // https://github.com/dotnet/maui/issues/17948
        // https://github.com/dotnet/maui/issues/1718
        newCell.Parent = Application.Current.MainPage;

        if (_virtualCell == newCell)
        {
            return;
        }

        var oldCell = _virtualCell;

        if(oldCell != null)
        {
            // Do not disconnect the Handler here as it may not redraw.
            //oldCell.Handler?.DisconnectHandler();
            oldCell.PropertyChanged -= CellPropertyChanged;
            oldCell.MeasureInvalidated -= OnMeasureInvalidated;
            // Delete previous child element
            foreach (var subView in ContentView.Subviews)
            {
                subView.RemoveFromSuperview();
            }
        }

        _virtualCell = newCell;
        _virtualCell.PropertyChanged += CellPropertyChanged;

        IPlatformViewHandler handler;
        if (newCell.Handler != null)
        {
            // TODO:
            // Currently, the performance is not good because the number of
            // NativieView is kept as many as the number of Cells. To improve this,
            // it is necessary to virtualize the Content part of CustomCell as well.
            // This can probably be achieved by replacing the Handler and
            // resetting the VirtualView.

            // If it has already been generated, use it as is.
            handler = newCell.Handler as IPlatformViewHandler;
            // If the incoming Cell belongs to another parent, peel it off.
            handler.PlatformView?.RemoveFromSuperview();
            ArrangeSubView(handler);            
        }
        else
        {
            // If Handler is not generated, generate it.            
            handler = GetNewHandler();
        }        

        var viewHandlerType = _mauiContext.Handlers.GetHandlerType(_virtualCell.GetType());
        var reflectableType = handler as System.Reflection.IReflectableType;
        var handlerType = reflectableType != null ? reflectableType.GetTypeInfo().AsType() : (handler != null ? handler.GetType() : typeof(System.Object));

        if (handlerType == viewHandlerType)
        {
            handler.SetVirtualView(_virtualCell);
            System.Diagnostics.Debug.WriteLine("SetVirtualCell");
        }
        else
        {
            //when cells are getting reused the element could be already set to another cell
            //so we should dispose based on the renderer and not the renderer.Element
            handler.DisposeHandlersAndChildren();
            handler = GetNewHandler();
            System.Diagnostics.Debug.WriteLine("Already set another cell");
        }        

        _virtualCell.MeasureInvalidated += OnMeasureInvalidated;        

        var height = double.PositiveInfinity;
        var result = _virtualCell.Measure(tableView.Frame.Width, height,MeasureFlags.IncludeMargins);
        var finalW = result.Request.Width;
        if(_virtualCell.HorizontalOptions.Alignment == LayoutAlignment.Fill)
        {
            finalW = tableView.Frame.Width;
        }
        var finalH = (float)result.Request.Height;           

        UpdateNativeCell();

        if (_heightConstraint != null)
        {
            _heightConstraint.Active = false;
            _heightConstraint?.Dispose();
        }

        _heightConstraint = handler.PlatformView.HeightAnchor.ConstraintEqualTo(finalH);
        _heightConstraint.Priority = 999f;
        _heightConstraint.Active = true;

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

        ArrangeSubView(newHandler);

        return newHandler;
    }

    protected virtual void ArrangeSubView(IPlatformViewHandler handler)
    {
        ContentView.AddSubview(handler.PlatformView);

        var native = handler.PlatformView;
        native.TranslatesAutoresizingMaskIntoConstraints = false;

        native.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
        native.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
        native.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
        native.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;
    }
}
