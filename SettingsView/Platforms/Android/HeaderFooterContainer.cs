using System;
using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Widget;
using Android.Graphics.Drawables;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;
using AiForms.Settings.Extensions;

namespace AiForms.Settings.Platforms.Droid;

[Android.Runtime.Preserve(AllMembers = true)]
internal class HeaderFooterContainer : FrameLayout
{   
    public ViewHolder ViewHolder { get; set; }
    public bool IsEmpty => _contentView == null;

    IPlatformViewHandler _viewHandler;
    AView _currentView;

    public HeaderFooterContainer(Context context) : base(context)
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

    public override bool OnTouchEvent(Android.Views.MotionEvent e)
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

        _viewHandler.LayoutVirtualView(l, t, r, b);
    }

    protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
    {
        int pxWidth = MeasureSpec.GetSize(widthMeasureSpec);
        if (_viewHandler == null)
        {
            SetMeasuredDimension(pxWidth, 0);
            return;
        }
        if(ViewHolder.RowInfo.ViewType == ViewType.CustomFooter && !ViewHolder.RowInfo.Section.FooterVisible)
        {
            SetMeasuredDimension(pxWidth, 0);
            return;
        }

        var dpWidth = Context.FromPixels(pxWidth);
        var size = _viewHandler.VirtualView.Measure(dpWidth, double.PositiveInfinity);
        int pxHeight = (int)Context.ToPixels(size.Height);

        SetMeasuredDimension(pxWidth, pxHeight);
    }

    public virtual void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == Cell.IsEnabledProperty.PropertyName)
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

    public void UpdateCell(View view)
    {
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

            if(_viewHandler != view.Handler)
            {
                if (view.Handler?.PlatformView is AView oldCellView &&
                    oldCellView.GetParent().GetParentOfType<HeaderFooterContainer>() is HeaderFooterContainer vc)
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

    public void DisconnectHandler()
    {
        var oldView = _currentView ?? _viewHandler.PlatformView;
        if (oldView != null)
            RemoveView(oldView);

        _contentView.Handler?.DisconnectHandler();
    }

    protected virtual void CreateNewHandler(View view)
    {
        _contentView = view;

        _contentView.MeasureInvalidated += _contentView_MeasureInvalidated;

        var platformView = _contentView.ToPlatform(view.FindMauiContext());
        _viewHandler = (IPlatformViewHandler)_contentView.Handler;
        AddView(platformView);
        UpdateNativeCell();
    }

    private void _contentView_MeasureInvalidated(object sender, EventArgs e)
    {
        ;
    }

    public override void AddView(AView child)
    {
        base.AddView(child);

        _currentView = child;
    }
}
