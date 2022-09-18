using System;
using AiForms.Settings.Platforms.Droid;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace AiForms.Settings.Handlers;

[Android.Runtime.Preserve(AllMembers = true)]
public partial class SettingsViewHandler : ViewHandler<SettingsView, AiRecyclerView>
{
    public static IPropertyMapper<SettingsView, SettingsViewHandler> Mapper =
    new PropertyMapper<SettingsView, SettingsViewHandler>(ViewMapper)
    {
        [nameof(SettingsView.SeparatorColor)] = MapSeparatorColor,
        [nameof(SettingsView.BackgroundColor)] = MapBackgroundColor,
        [nameof(SettingsView.RowHeight)] = MapRowHeight,
        [nameof(SettingsView.ScrollToTop)] = MapScrollToTop,
        [nameof(SettingsView.ScrollToBottom)] = MapScrollToBottom,
        [nameof(SettingsView.UseDescriptionAsValue)] = MapDataSetChanged,
        [nameof(SettingsView.HasUnevenRows)] = MapDataSetChanged,
        [nameof(SettingsView.ShowSectionTopBottomBorder)] = MapInvalidateItemDecorations,
    };

    protected override AiRecyclerView CreatePlatformView()
    {        
        return new AiRecyclerView(Context, VirtualView);
    }

    private static void MapSeparatorColor(SettingsViewHandler handler, SettingsView sv)
    {
        handler.PlatformView.UpdateSeparatorColor();
    }

    private static void MapBackgroundColor(SettingsViewHandler handler, SettingsView sv)
    {
        handler.PlatformView.UpdateBackgroundColor();
    }

    private static void MapRowHeight(SettingsViewHandler handler, SettingsView sv)
    {
        handler.PlatformView.UpdateRowHeight();
    }

    private static void MapScrollToTop(SettingsViewHandler handler, SettingsView sv)
    {
        handler.PlatformView.UpdateScrollToTop();
    }

    private static void MapScrollToBottom(SettingsViewHandler handler, SettingsView sv)
    {
        handler.PlatformView.UpdateScrollToBottom();
    }

    private static void MapDataSetChanged(SettingsViewHandler handler, SettingsView sv)
    {
        handler.PlatformView.GetAdapter()?.NotifyDataSetChanged();
    }

    private static void MapInvalidateItemDecorations(SettingsViewHandler handler, SettingsView sv)
    {
        handler.PlatformView.InvalidateItemDecorations();
    }
}

