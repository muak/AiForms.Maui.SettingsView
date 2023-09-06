using System;
using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using AView = Android.Views.View;
using View = Microsoft.Maui.Controls.View;
using Microsoft.Maui.Platform;
using AiForms.Settings.Extensions;

namespace AiForms.Settings.Platforms.Droid;

[Android.Runtime.Preserve(AllMembers = true)]
internal class FormsViewContainer : FrameLayout
{
    public ViewHolder ViewHolder { get; set; }
    public Element Element => ContentView;
    public bool IsEmpty => _contentView == null;
    public bool IsMeasureOnce => CustomCell?.IsMeasureOnce ?? false;
    public CustomCell CustomCell { get; set; }

    IPlatformViewHandler _viewHandler;
    AView _currentView;


    public FormsViewContainer(Context context) : base(context)
    {
        Clickable = true;
    }

    View _contentView;
    public View ContentView
    {
        get { return _contentView; }
        set
        {
            if (_contentView == value)
                return;
            UpdateCell(value);
        }
    }

    public override bool OnTouchEvent(MotionEvent e)
    {
        return false; // pass to parent (ripple effect)
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_contentView != null)
            {
                _contentView.PropertyChanged -= CellPropertyChanged;
                _contentView = null;
            }
            CustomCell = null;

            ViewHolder = null;

            _viewHandler?.DisconnectHandler();
            _viewHandler = null;
        }
        base.Dispose(disposing);
    }

    protected override void OnLayout(bool changed, int l, int t, int r, int b)
    {
        if (IsEmpty)
        {
            return;
        }

        _viewHandler.LayoutVirtualView(l,t,r,b);       
    }

    int _heightCache;

    protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
    {
        int pxWidth = MeasureSpec.GetSize(widthMeasureSpec);

        if (IsMeasureOnce && _heightCache > 0)
        {
            SetMeasuredDimension(pxWidth, _heightCache);
            return;
        }

        var dpWidth = Context.FromPixels(pxWidth);
        var size = _viewHandler.VirtualView.Measure(dpWidth, double.PositiveInfinity);
        // This way can't measure correctly.
        //var measure = _viewHandler.MeasureVirtualView(width, heightMeasureSpec);
        int pxHeight = (int)Context.ToPixels(size.Height);

        SetMeasuredDimension(pxWidth, pxHeight);
        _heightCache = pxHeight;
    }

    public virtual void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == CellBase.IsEnabledProperty.PropertyName)
        {
            UpdateIsEnabled();
        }
    }

    public virtual void UpdateNativeCell()
    {
        UpdateIsEnabled();
    }

    public void UpdateIsEnabled()
    {
        Enabled = _contentView.IsEnabled;
    }

    protected virtual void CreateNewHandler(View view)
    {
        _contentView = view;
        var platformView = _contentView.ToPlatform(view.FindMauiContext());
        _viewHandler = (IPlatformViewHandler)_contentView.Handler;
        AddView(platformView);       

        _contentView.IsPlatformEnabled = true;
        UpdateNativeCell();
    }

    public void DisconnectHandler()
    {
        var oldView = _currentView ?? _viewHandler.PlatformView;
        if (oldView != null)
            RemoveView(oldView);

        _contentView.Handler?.DisconnectHandler();
    }

    public void UpdateCell(View view)
    {
        if (_contentView == view && !CustomCell.IsForceLayout)
        {
            return;
        }
        CustomCell.IsForceLayout = false;

        if (_contentView != null)
        {
            _contentView.PropertyChanged -= CellPropertyChanged;
        }

        view.PropertyChanged += CellPropertyChanged;

        if (_viewHandler == null)
        {
            CreateNewHandler(view);
            return;
        }

        var viewHandlerType = _viewHandler.MauiContext.Handlers.GetHandlerType(view.GetType());
        var reflectableType = _viewHandler as System.Reflection.IReflectableType;
        var rendererType = reflectableType != null ? reflectableType.GetTypeInfo().AsType() : (_viewHandler != null ? _viewHandler.GetType() : typeof(System.Object));
        if (_viewHandler != null && rendererType == viewHandlerType)
        {
            _contentView = view;

            if (_viewHandler != view.Handler)
            {
                if (view.Handler?.PlatformView is AView oldCellView &&
                    oldCellView.GetParent().GetParentOfType<FormsViewContainer>() is FormsViewContainer vc)
                {
                    vc.DisconnectHandler();
                }

                var oldView = _currentView ?? _viewHandler.PlatformView;
                if (oldView != null)
                    RemoveView(oldView);

                _contentView.Handler?.DisconnectHandler();
                _viewHandler.SetVirtualView(_contentView);
                AddView(_viewHandler.PlatformView);
            }

            UpdateNativeCell();
            Invalidate();

            return;
        }

        RemoveView(_currentView ?? _viewHandler.PlatformView);
        _contentView.Handler?.DisconnectHandler();
        _contentView.IsPlatformEnabled = false;
        _viewHandler.DisconnectHandler();

        CreateNewHandler(view);
    }


}
