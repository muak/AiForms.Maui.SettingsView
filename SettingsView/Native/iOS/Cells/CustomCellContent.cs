using System;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading;
using AiForms.Settings.Extensions;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using UIKit;

namespace AiForms.Settings.Platforms.iOS;

[Foundation.Preserve(AllMembers = true)]
public class CustomCellContent: UIView
{
    static MethodInfo ElementDescendantsInfo = typeof(Element).GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(x => x.Name == "Descendants" && !x.IsGenericMethod);
    bool _disposed;
    NSLayoutConstraint _heightConstraint;
    View _virtualCell;
    public CustomCell CustomCell { get; set; }
    double _lastFrameWidth = -9999d;
    double _lastMeasureWidth = -9999d;
    double _lastMeasureHeight = -9999d;
    IMauiContext _mauiContext => _virtualCell.FindMauiContext();
    UITableView _tableView;

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
                foreach (var child in CustomCellContent.ElementDescendants(_virtualCell))
                {
                    if (child is Layout layout)
                    {
                        layout.SizeChanged -= OnInnerLayoutSizeChanged;
                    }
                    child.MeasureInvalidated -= OnMeasureInvalidated;
                }
            }

            CustomCell = null;

            _heightConstraint?.Dispose();
            _heightConstraint = null;


            if(_virtualCell.Handler is IPlatformViewHandler handler)
            {
                handler.VirtualView.DisposeModalAndChildHandlers();
                handler?.DisconnectHandler();
            }
            
            _virtualCell = null;
            _tableView = null;
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
        if (e.PropertyName == CellBase.IsEnabledProperty.PropertyName)
        {
            UpdateIsEnabled();
        }
    }

    protected virtual void UpdateIsEnabled()
    {
        UserInteractionEnabled = _virtualCell.IsEnabled;
    }

    public virtual void UpdateCell(View newCell, UITableView tableView)
    {
        // Workaround GestureRecognizer bug
        // TODO: if fix this issue, remove the following code.
        // https://github.com/dotnet/maui/issues/17948
        // newCell.Parent = Application.Current.MainPage;

        _tableView = tableView;       

        var oldCell = _virtualCell;
        // If oldCell and newCell are the same
        if (oldCell == newCell)
        {            
            if (!CustomCell.IsForceLayout && Subviews.Any())
            {
                // do nothing.
                return;
            }
            CustomCell.IsForceLayout = false;
        }


        if (oldCell != null)
        {
            _cts?.Cancel();
            oldCell.PropertyChanged -= CellPropertyChanged;
            oldCell.MeasureInvalidated -= OnMeasureInvalidated;
            // Delete previous child element
            foreach (var subView in Subviews)
            {
                subView.RemoveFromSuperview();
            }

            foreach (var child in CustomCellContent.ElementDescendants(oldCell))
            {
                if (child is Layout layout)
                {
                    layout.SizeChanged -= OnInnerLayoutSizeChanged;
                }
                child.MeasureInvalidated -= OnMeasureInvalidated;
            }
        }

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
        }
        else
        {
            // Workaround GestureRecognizer bug
            // TODO: if fix this issue, remove the following code.
            // https://github.com/dotnet/maui/issues/17948
            // https://github.com/dotnet/maui/issues/1718
            // newCell.Parent = Application.Current.MainPage;

            // If Handler is not generated, generate it.            
            handler = newCell.ToHandler(newCell.FindMauiContext());
        }

        _virtualCell = newCell;
        _virtualCell.PropertyChanged += CellPropertyChanged;
        
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
            handler = _virtualCell.ToHandler(_virtualCell.FindMauiContext());
        }


        if (!CustomCell.IsMeasureOnce || tableView.Frame.Width != _lastFrameWidth)
        {
            _lastFrameWidth = tableView.Frame.Width;
            var height = double.PositiveInfinity;

            var accessoryWidth = CustomCell.ShowArrowIndicator ? 27 : 0;
            var width = tableView.Frame.Width - (CustomCell.UseFullSize ? accessoryWidth : 32); // CellBaseView layout margin
            var result = _virtualCell.Measure(tableView.Frame.Width, height,MeasureFlags.IncludeMargins);
            _lastMeasureWidth = result.Request.Width;
            if (_virtualCell.HorizontalOptions.Alignment == LayoutAlignment.Fill)
            {
                _lastMeasureWidth = width;
            }
            _lastMeasureHeight = result.Request.Height;
        }

        // Add PlatformView as a child
        ArrangeSubView(handler);

        UpdateNativeCell();

        //SetNeedsLayout();

        _virtualCell.MeasureInvalidated += OnMeasureInvalidated;
        foreach (var child in CustomCellContent.ElementDescendants(_virtualCell))
        {
            if(child is Layout layout)
            {
                // Also detects changes in the size of descendant layout(BindableLayout)
                layout.SizeChanged += OnInnerLayoutSizeChanged;
            }
            // Also detects changes in the size of descendant elements
            child.MeasureInvalidated += OnMeasureInvalidated;
        }
    }

    private void OnInnerLayoutSizeChanged(object sender, EventArgs e)
    {
        LayoutDispacher();
    }    

    private void OnMeasureInvalidated(object sender, EventArgs e)
    {       
        LayoutDispacher();
    }

    async Task ForceLayout(CancellationToken token)
    {
        if(_tableView == null)
        {
            return;
        }
        var tableView = _tableView;
        var handler = _virtualCell.Handler as IPlatformViewHandler;

        // Wait a little and execute layout processing only on the last event
        await Task.Delay(100);
        if (token.IsCancellationRequested)
        {
            return;
        }

        _lastFrameWidth = tableView.Frame.Width;
        var height = double.PositiveInfinity;

        var accessoryWidth = CustomCell.ShowArrowIndicator ? 27 : 0;
        var width = tableView.Frame.Width - (CustomCell.UseFullSize ? accessoryWidth : 32); // CellBaseView layout margin
        var result = _virtualCell.Measure(tableView.Frame.Width, height, MeasureFlags.IncludeMargins);
        _lastMeasureWidth = result.Request.Width;
        if (_virtualCell.HorizontalOptions.Alignment == LayoutAlignment.Fill)
        {
            _lastMeasureWidth = width;
        }
        _lastMeasureHeight = result.Request.Height;

        if (_heightConstraint != null)
        {
            _heightConstraint.Active = false;
            _heightConstraint.Priority = 1f;
            _heightConstraint?.Dispose();
        }

        var native = handler.PlatformView;

        _heightConstraint = native.HeightAnchor.ConstraintEqualTo((NFloat)_lastMeasureHeight);
        _heightConstraint.Priority = 999f;
        _heightConstraint.Active = true;

        _virtualCell.Arrange(new Rect(0, 0, _lastMeasureWidth, _lastMeasureHeight));

        native.SetNeedsUpdateConstraints();
        //native.UpdateConstraintsIfNeeded();
        SetNeedsLayout();

        tableView.BeginUpdates();
        tableView.EndUpdates();
    }

    void ArrangeSubView(IPlatformViewHandler handler)
    {
        var native = handler.PlatformView;
        AddSubview(native);
        
        native.TranslatesAutoresizingMaskIntoConstraints = false;

        if (_heightConstraint != null)
        {
            _heightConstraint.Active = false;
            _heightConstraint.Priority = 1f;            
            _heightConstraint?.Dispose();
            //native.SetNeedsUpdateConstraints();
            //native.UpdateConstraintsIfNeeded();
        }

        _heightConstraint = native.HeightAnchor.ConstraintEqualTo((NFloat)_lastMeasureHeight);
        _heightConstraint.Priority = 999f;
        _heightConstraint.Active = true;        

        native.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
        native.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
        native.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
        native.RightAnchor.ConstraintEqualTo(RightAnchor).Active = true;

        //native.SetNeedsUpdateConstraints();
        //native.UpdateConstraintsIfNeeded();
    }

    CancellationTokenSource _cts = new CancellationTokenSource();
    void LayoutDispacher()
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        _ = ForceLayout(_cts.Token);
    }

    internal static IEnumerable<VisualElement> ElementDescendants(Element element)
    {
        return (ElementDescendantsInfo.Invoke(element, new object[] { }) as IEnumerable<Element>).OfType<VisualElement>();
    }
}

