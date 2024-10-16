﻿using System;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using SpriteKit;
using UIKit;
using PointF = CoreGraphics.CGPoint;
using RectangleF = CoreGraphics.CGRect;

namespace AiForms.Settings.Platforms.iOS;

/// <summary>
/// Copy from
/// https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/Compatibility/Handlers/iOS/KeyboardInsetTracker.cs
/// </summary>
[Foundation.Preserve(AllMembers = true)]
internal class KeyboardInsetTracker : IDisposable
{
    readonly Func<UIWindow> _fetchWindow;
    readonly Action<PointF> _setContentOffset;
    readonly Action<UIEdgeInsets> _setInsetAction;
    readonly UIView _targetView;
    bool _disposed;

    RectangleF _lastKeyboardRect;
    ShellScrollViewTracker _shellScrollViewTracker;

    //public KeyboardInsetTracker(UIView targetView, Func<UIWindow> fetchWindow, Action<UIEdgeInsets> setInsetAction) : this(targetView, fetchWindow, setInsetAction, null)
    //{
    //}

    public KeyboardInsetTracker(UIView targetView, Func<UIWindow> fetchWindow, Action<UIEdgeInsets> setInsetAction, Action<PointF> setContentOffset, IPlatformViewHandler renderer)
    {
        _setContentOffset = setContentOffset;
        _targetView = targetView;
        _fetchWindow = fetchWindow;
        _setInsetAction = setInsetAction;
        KeyboardObserver.KeyboardWillShow += OnKeyboardShown;
        KeyboardObserver.KeyboardWillHide += OnKeyboardHidden;
        //if (renderer != null)
        //{
        //    _shellScrollViewTracker = new ShellScrollViewTracker(renderer);
        //}
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;

        KeyboardObserver.KeyboardWillShow -= OnKeyboardShown;
        KeyboardObserver.KeyboardWillHide -= OnKeyboardHidden;
        //_shellScrollViewTracker?.Dispose();
        //_shellScrollViewTracker = null;
    }

    //This method allows us to update the insets if the Frame changes
    internal void UpdateInsets()
    {
        //being called from LayoutSubviews but keyboard wasn't shown yet
        if (_lastKeyboardRect.IsEmpty)
            return;

        var window = _fetchWindow();
        // Code left verbose to make its operation more obvious
        if (window == null)
        {
            // we are not currently displayed and can safely ignore this
            // most likely this renderer is on a page which is currently not displayed (e.g. in NavController)
            return;
        }

        var field = FindFirstResponder(_targetView);

        //the view that is triggering the keyboard is not inside our UITableView?
        //if (field == null)
        //  return;

        var boundsSize = _targetView.Frame.Size;

        //since our keyboard frame is RVC CoordinateSpace, lets convert it to our targetView CoordinateSpace
        var rect = _targetView.Superview.ConvertRectFromView(_lastKeyboardRect, null);
        //let's see how much does it cover our target view
        var overlay = RectangleF.Intersect(rect, _targetView.Frame);

        _setInsetAction(new UIEdgeInsets(0, 0, overlay.Height, 0));

        if (field is UITextView && _setContentOffset != null)
        {
            var keyboardTop = boundsSize.Height - overlay.Height;
            var fieldPosition = field.ConvertPointToView(field.Frame.Location, _targetView.Superview);
            var fieldBottom = fieldPosition.Y + field.Frame.Height;
            var offset = fieldBottom - keyboardTop;
            if (offset > 0)
                _setContentOffset(new PointF(0, offset));
        }
    }

    public void OnLayoutSubviews() => _shellScrollViewTracker?.OnLayoutSubviews();

    void OnKeyboardHidden(object sender, UIKeyboardEventArgs args)
    {
        //if (_shellScrollViewTracker == null || !_shellScrollViewTracker.Reset())
        //{
        //    _setInsetAction(new UIEdgeInsets(0, 0, 0, 0));
        //}
        _setInsetAction(new UIEdgeInsets(0, 0, 0, 0));
        _lastKeyboardRect = RectangleF.Empty;
    }

    void OnKeyboardShown(object sender, UIKeyboardEventArgs args)
    {
        _lastKeyboardRect = args.FrameEnd;
        UpdateInsets();
    }

    //元は拡張メソッドだけど移植がめんどいので個別切り出し
    UIView FindFirstResponder(UIView view)
    {
        if (view.IsFirstResponder)
            return view;

        foreach (var subView in view.Subviews)
        {
            var firstResponder = FindFirstResponder(subView);
            if (firstResponder != null)
                return firstResponder;
        }

        return null;
    }
}

internal static class KeyboardObserver
{
    static KeyboardObserver()
    {
        UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShown);
        UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHidden);
    }

    public static event EventHandler<UIKeyboardEventArgs> KeyboardWillHide;

    public static event EventHandler<UIKeyboardEventArgs> KeyboardWillShow;

    static void OnKeyboardHidden(object sender, UIKeyboardEventArgs args)
    {
        var handler = KeyboardWillHide;
        if (handler != null)
            handler(sender, args);
    }

    static void OnKeyboardShown(object sender, UIKeyboardEventArgs args)
    {
        var handler = KeyboardWillShow;
        if (handler != null)
            handler(sender, args);
    }
}

