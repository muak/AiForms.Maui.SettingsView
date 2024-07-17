using System;
using System.Collections.Specialized;
using AiForms.Settings.Platforms.iOS;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Handlers;
using ObjCRuntime;
using UIKit;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Platform;
using MobileCoreServices;
using AiForms.Settings.Extensions;

namespace AiForms.Settings.Handlers;

public partial class SettingsViewHandler: ViewHandler<SettingsView, AiTableView>
{
    public static IPropertyMapper<SettingsView, SettingsViewHandler> Mapper =
    new PropertyMapper<SettingsView, SettingsViewHandler>(ViewMapper)
    {
        [nameof(SettingsView.SeparatorColor)] = MapSeparatorColor,
        [nameof(SettingsView.BackgroundColor)] = MapBackgroundColor,
        [nameof(SettingsView.RowHeight)] = MapRowHeight,
        [nameof(SettingsView.ScrollToTop)] = MapScrollToTop,
        [nameof(SettingsView.ScrollToBottom)] = MapScrollToBottom,
    };

    internal static readonly string TextHeaderId = "textHeaderView";
    internal static readonly string TextFooterId = "textFooterView";
    internal static readonly string CustomHeaderId = "customHeaderView";
    internal static readonly string CustomFooterId = "customFooterView";
    Page _parentPage;
    KeyboardInsetTracker _insetTracker;
    internal static float MinRowHeight = 48;
    AiTableView _tableview;
    IDisposable _contentSizeObserver;

    protected override AiTableView CreatePlatformView()
    {
        _tableview = new AiTableView(VirtualView);

        VirtualView.CollectionChanged += OnCollectionChanged;
        VirtualView.SectionCollectionChanged += OnSectionCollectionChanged;
        VirtualView.SectionPropertyChanged += OnSectionPropertyChanged;
        VirtualView.CellPropertyChanged += OnCellPropertyChanged;

        _insetTracker = new KeyboardInsetTracker(_tableview, () => PlatformView.Window, insets => PlatformView.ContentInset = PlatformView.ScrollIndicatorInsets = insets, point =>
        {
            var offset = PlatformView.ContentOffset;
            offset.Y += point.Y;
            PlatformView.SetContentOffset(offset, true);
        }, this);

        _contentSizeObserver = _tableview.AddObserver("contentSize", NSKeyValueObservingOptions.New, OnContentSizeChanged);

        return _tableview;
    }   

    protected override void ConnectHandler(AiTableView platformView)
    {
        base.ConnectHandler(platformView);

        foreach (var el in VirtualView.GetParentsPath())
        {
            if (el is Page page)
            {
                _parentPage = page;
                _parentPage.Appearing += ParentPageAppearing;
            }
        }

        if (SettingsViewConfiguration.ShouldAutoDisconnect)
        {
            VirtualView.AddCleanUpEvent();
        }
    }

    protected override void DisconnectHandler(AiTableView platformView)
    {
        if (_parentPage is not null)
        {
            _parentPage.Appearing -= ParentPageAppearing;
            _parentPage = null;
        }

        _contentSizeObserver?.Dispose();
        _contentSizeObserver = null;

        VirtualView.CollectionChanged -= OnCollectionChanged;
        VirtualView.SectionCollectionChanged -= OnSectionCollectionChanged;
        VirtualView.SectionPropertyChanged -= OnSectionPropertyChanged;
        VirtualView.CellPropertyChanged -= OnCellPropertyChanged;

        _insetTracker?.Dispose();
        _insetTracker = null;

        _tableview?.Dispose();
        _tableview = null;

        base.DisconnectHandler(platformView);
    }

    void OnContentSizeChanged(NSObservedChange change)
    {
        VirtualView.VisibleContentHeight = PlatformView.ContentSize.Height;
    }

    void ParentPageAppearing(object sender, EventArgs e)
    {
        if(_tableview is null)
        {
            return;
        }

        if(_tableview.IndexPathForSelectedRow != null)
        {
            _tableview.DeselectRow(_tableview.IndexPathForSelectedRow, true);
        }        
    }

    void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        PlatformView.UpdateSections(e);
    }

    void OnSectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        var sectionIdx = VirtualView.Model.GetSectionIndex((Section)sender);
        PlatformView.UpdateItems(e, sectionIdx, false);
    }

    void OnSectionPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == Section.IsVisibleProperty.PropertyName)
        {
            PlatformView.UpdateSectionVisible((Section)sender);
        }
        else if (e.PropertyName == TableSectionBase.TitleProperty.PropertyName ||
                e.PropertyName == Section.HeaderViewProperty.PropertyName ||
                e.PropertyName == Section.HeaderHeightProperty.PropertyName ||
                e.PropertyName == Section.FooterTextProperty.PropertyName ||
                e.PropertyName == Section.FooterViewProperty.PropertyName)
        {
            PlatformView.UpdateSectionNoAnimation((Section)sender);
        }
        else if (e.PropertyName == Section.FooterVisibleProperty.PropertyName)
        {
            PlatformView.UpdateSectionFade((Section)sender);
        }
    }

    void OnCellPropertyChanged(object sender, CellPropertyChangedEventArgs e)
    {
        if (e.PropertyName == CellBase.IsVisibleProperty.PropertyName)
        {
            PlatformView.UpdateCellVisible(e.Section, (CellBase)sender);
        }
    }

    private static void MapScrollToBottom(SettingsViewHandler handler, SettingsView sv)
    {
        if (sv.ScrollToBottom)
        {
            var tv = handler.PlatformView;
            var sectionIdx = tv.NumberOfSections() - 1;
            if (sectionIdx < 0)
            {
                sv.ScrollToBottom = false;
                return;
            }

            var rowIdx = tv.NumberOfRowsInSection(sectionIdx) - 1;

            if (sectionIdx >= 0 && rowIdx >= 0)
            {
                tv.ScrollToRow(NSIndexPath.Create(sectionIdx, rowIdx), UITableViewScrollPosition.Top, false);
            }

            sv.ScrollToBottom = false;
        }
    }

    private static void MapScrollToTop(SettingsViewHandler handler, SettingsView sv)
    {
        if (sv.ScrollToTop)
        {
            var tv = handler.PlatformView;
            if (tv.NumberOfSections() == 0)
            {
                sv.ScrollToTop = false;
                return;
            }
            var sectionIdx = 0;
            var rows = tv.NumberOfRowsInSection(sectionIdx);
            if (rows > 0)
            {
                tv.SetContentOffset(new CGPoint(0, 0), false);
            }

            sv.ScrollToTop = false;
        }
    }

    private static void MapRowHeight(SettingsViewHandler handler, SettingsView sv)
    {
        handler.PlatformView.EstimatedRowHeight = Math.Max((float)sv.RowHeight, MinRowHeight);
        handler.PlatformView.ReloadData();
    }

    private static void MapBackgroundColor(SettingsViewHandler handler, SettingsView sv)
    {
        if (!sv.BackgroundColor.IsDefault())
        {
            handler.PlatformView.BackgroundColor = sv.BackgroundColor.ToPlatform();
        }
    }

    private static void MapSeparatorColor(SettingsViewHandler handler, SettingsView sv)
    {
        handler.PlatformView.SeparatorColor = sv.SeparatorColor.ToPlatform();
    }
}

