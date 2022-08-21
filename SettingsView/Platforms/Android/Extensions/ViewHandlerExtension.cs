#nullable enable
using System;
using Android.Views;
using Microsoft.Maui.Platform;
using static Android.Views.View;

namespace AiForms.Settings.Platforms.Droid;

/// <summary>
/// Copy form Microsoft.Maui.ViewHandlerExtensions.Android.cs
/// </summary>
public static class ViewHandlerExtension
{
    // TODO: Possibly reconcile this code with LayoutViewGroup.OnLayout
    // If you make changes here please review if those changes should also
    // apply to LayoutViewGroup.OnLayout
    public static Size LayoutVirtualView(
            this IPlatformViewHandler viewHandler,
            int l, int t, int r, int b,
            Func<Rect, Size>? arrangeFunc = null)
    {
        var context = viewHandler.MauiContext?.Context;
        var virtualView = viewHandler.VirtualView;
        var platformView = viewHandler.PlatformView;

        if (context == null || virtualView == null || platformView == null)
        {
            return Size.Zero;
        }

        var destination = context.ToCrossPlatformRectInReferenceFrame(l, t, r, b);
        arrangeFunc ??= virtualView.Arrange;
        return arrangeFunc(destination);
    }

    // TODO: Possibly reconcile this code with LayoutViewGroup.OnMeasure
    // If you make changes here please review if those changes should also
    // apply to LayoutViewGroup.OnMeasure
    public static Size MeasureVirtualView(
        this IPlatformViewHandler viewHandler,
        int platformWidthConstraint,
        int platformHeightConstraint,
        Func<double, double, Size>? measureFunc = null)
    {
        var context = viewHandler.MauiContext?.Context;
        var virtualView = viewHandler.VirtualView;
        var platformView = viewHandler.PlatformView;

        if (context == null || virtualView == null || platformView == null)
        {
            return Size.Zero;
        }

        var deviceIndependentWidth = platformWidthConstraint.ToDouble(context);
        var deviceIndependentHeight = platformHeightConstraint.ToDouble(context);

        var widthMode = MeasureSpec.GetMode(platformWidthConstraint);
        var heightMode = MeasureSpec.GetMode(platformHeightConstraint);

        measureFunc ??= virtualView.Measure;
        var measure = measureFunc(deviceIndependentWidth, deviceIndependentHeight);

        // If the measure spec was exact, we should return the explicit size value, even if the content
        // measure came out to a different size
        var width = widthMode == MeasureSpecMode.Exactly ? deviceIndependentWidth : measure.Width;
        var height = heightMode == MeasureSpecMode.Exactly ? deviceIndependentHeight : measure.Height;

        var platformWidth = context.ToPixels(width);
        var platformHeight = context.ToPixels(height);

        // Minimum values win over everything
        platformWidth = Math.Max(platformView.MinimumWidth, platformWidth);
        platformHeight = Math.Max(platformView.MinimumHeight, platformHeight);

        return new Size(platformWidth, platformHeight);
    }
}

