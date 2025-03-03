using System;
using System.Collections.Specialized;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using MobileCoreServices;
using ObjCRuntime;
using UIKit;

namespace AiForms.Settings.Platforms.iOS;

public class AiTableView: UITableView, IUITableViewDragDelegate, IUITableViewDropDelegate
{
    internal static readonly string TextHeaderId = "textHeaderView";
    internal static readonly string TextFooterId = "textFooterView";
    internal static readonly string CustomHeaderId = "customHeaderView";
    internal static readonly string CustomFooterId = "customFooterView";
    internal static float MinRowHeight = 48;

    SettingsView _settingsView;
    SettingsModel _model => _settingsView.Model;

    public AiTableView(SettingsView settingsView) : base(CGRect.Empty, UITableViewStyle.Grouped)
    {
        _settingsView = settingsView;

        DragDelegate = this;
        DropDelegate = this;

        DragInteractionEnabled = true;
        Source = new SettingsTableSource(settingsView);

        ScrollEnabled = true;
        RowHeight = UITableView.AutomaticDimension;
        KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

        CellLayoutMarginsFollowReadableWidth = false;

        SectionHeaderHeight = UITableView.AutomaticDimension;
        EstimatedSectionHeaderHeight = UITableView.AutomaticDimension;

        SectionFooterHeight = UITableView.AutomaticDimension;
        EstimatedSectionFooterHeight = UITableView.AutomaticDimension;

        RegisterClassForHeaderFooterViewReuse(typeof(TextHeaderView), TextHeaderId);
        RegisterClassForHeaderFooterViewReuse(typeof(TextFooterView), TextFooterId);
        RegisterClassForHeaderFooterViewReuse(typeof(CustomHeaderView), CustomHeaderId);
        RegisterClassForHeaderFooterViewReuse(typeof(CustomFooterView), CustomFooterId);               
        
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Source?.Dispose();
            Source = null;
            foreach (UIView subview in Subviews)
            {
                DisposeSubviews(subview);
            }
            _settingsView = null;
        }
        base.Dispose(disposing);
    }   

    void DisposeSubviews(UIView view)
    {
        var ver = view as IVisualElementRenderer;

        if (ver == null)
        {
            foreach (UIView subView in view.Subviews)
            {
                DisposeSubviews(subView);
            }

            view.RemoveFromSuperview();
        }

        view.Dispose();
    }

    internal void UpdateSections(NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewStartingIndex == -1)
                {
                    goto case NotifyCollectionChangedAction.Reset;
                }
                BeginUpdates();
                InsertSections(NSIndexSet.FromIndex(e.NewStartingIndex), UITableViewRowAnimation.Automatic);
                EndUpdates();
                break;
            case NotifyCollectionChangedAction.Remove:
                if (e.OldStartingIndex == -1)
                {
                    goto case NotifyCollectionChangedAction.Reset;
                }
                BeginUpdates();
                DeleteSections(NSIndexSet.FromIndex(e.OldStartingIndex), UITableViewRowAnimation.Automatic);
                EndUpdates();
                break;

            case NotifyCollectionChangedAction.Replace:
                if (e.OldStartingIndex == -1)
                {
                    goto case NotifyCollectionChangedAction.Reset;
                }
                BeginUpdates();
                ReloadSections(NSIndexSet.FromIndex(e.OldStartingIndex), UITableViewRowAnimation.Automatic);
                EndUpdates();

                break;

            case NotifyCollectionChangedAction.Move:
            case NotifyCollectionChangedAction.Reset:

                ReloadData();
                return;
        }
    }

    internal void UpdateItems(NotifyCollectionChangedEventArgs e, int section, bool resetWhenGrouped)
    {
        // This means the UITableView hasn't rendered any cells yet
        // so there's no need to synchronize the rows on the UITableView
        if (IndexPathsForVisibleRows == null && e.Action != NotifyCollectionChangedAction.Reset)
            return;

        // If the section is not visible, do nothing.
        if (!_model.GetSection(section).IsVisible)
        {
            return;
        }

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewStartingIndex == -1)
                {
                    goto case NotifyCollectionChangedAction.Reset;
                }

                BeginUpdates();
                InsertRows(GetPaths(section, e.NewStartingIndex, e.NewItems.Count), UITableViewRowAnimation.Automatic);
                EndUpdates();

                break;

            case NotifyCollectionChangedAction.Remove:
                if (e.OldStartingIndex == -1)
                {
                    goto case NotifyCollectionChangedAction.Reset;
                }

                BeginUpdates();
                DeleteRows(GetPaths(section, e.OldStartingIndex, e.OldItems.Count), UITableViewRowAnimation.Automatic);
                EndUpdates();

                break;

            case NotifyCollectionChangedAction.Move:
                if (e.OldStartingIndex == -1 || e.NewStartingIndex == -1)
                {
                    goto case NotifyCollectionChangedAction.Reset;
                }

                BeginUpdates();
                for (var i = 0; i < e.OldItems.Count; i++)
                {
                    var oldi = e.OldStartingIndex;
                    var newi = e.NewStartingIndex;

                    if (e.NewStartingIndex < e.OldStartingIndex)
                    {
                        oldi += i;
                        newi += i;
                    }

                    MoveRow(NSIndexPath.FromRowSection(oldi, section), NSIndexPath.FromRowSection(newi, section));
                }
                EndUpdates();

                break;

            case NotifyCollectionChangedAction.Replace:
                if (e.OldStartingIndex == -1)
                {
                    goto case NotifyCollectionChangedAction.Reset;
                }

                BeginUpdates();
                ReloadRows(GetPaths(section, e.OldStartingIndex, e.OldItems.Count), UITableViewRowAnimation.None);
                EndUpdates();

                break;

            case NotifyCollectionChangedAction.Reset:
                ReloadData();
                return;
        }
    }   

    internal void UpdateSectionVisible(Section section)
    {
        var secIndex = _model.GetSectionIndex(section);
        BeginUpdates();
        ReloadSections(NSIndexSet.FromIndex(secIndex), UITableViewRowAnimation.Automatic);
        EndUpdates();
    }

    internal void UpdateCellVisible(Section section, CellBase cell)
    {
        var secIndex = _model.GetSectionIndex(section);
        var rowIndex = section.IndexOf(cell);
        BeginUpdates();
        ReloadRows(GetPaths(secIndex, rowIndex, 1), UITableViewRowAnimation.Automatic);
        EndUpdates();
    }

    internal void ReloadCell(CellBase cell)
    {
        var secIndex = _model.GetSectionIndex(cell.Section);
        var rowIndex = cell.Section.IndexOf(cell);
        BeginUpdates();
        ReloadRows(GetPaths(secIndex, rowIndex, 1), UITableViewRowAnimation.None);
        EndUpdates();
    }

    internal void UpdateSectionNoAnimation(Section section)
    {
        var secIndex = _model.GetSectionIndex(section);
        BeginUpdates();
        ReloadSections(NSIndexSet.FromIndex(secIndex), UITableViewRowAnimation.None);
        EndUpdates();
    }

    internal void UpdateSectionFade(Section section)
    {
        var secIndex = _model.GetSectionIndex(section);
        BeginUpdates();
        ReloadSections(NSIndexSet.FromIndex(secIndex), UITableViewRowAnimation.Fade);
        EndUpdates();
    }

    public UIDragItem[] GetItemsForBeginningDragSession(UITableView tableView, IUIDragSession session, NSIndexPath indexPath)
    {
        var section = _model.GetSection(indexPath.Section);
        if (!section.UseDragSort)
        {
            return new UIDragItem[] { };
        }

        var cell = _model.GetCell(indexPath.Section, indexPath.Row);
        if (!cell.IsEnabled)
        {
            return new UIDragItem[] { };
        }

        // set "sectionIndex,rowIndex" as string
        var data = NSData.FromString($"{indexPath.Section},{indexPath.Row}");

        var itemProvider = new NSItemProvider();
        itemProvider.RegisterDataRepresentation(UTType.PlainText, NSItemProviderRepresentationVisibility.All, (completionHandler) =>
        {
            completionHandler(data, null);
            return null;
        });

        return new UIDragItem[] { new UIDragItem(itemProvider) };
    }

    public void PerformDrop(UITableView tableView, IUITableViewDropCoordinator coordinator)
    {
        var destinationIndexPath = coordinator.DestinationIndexPath;
        if (destinationIndexPath == null)
        {
            return;
        }

        coordinator.Session.LoadObjects<NSString>(items => {
            var path = items[0].ToString().Split(new char[] { ',' }, StringSplitOptions.None).Select(x => int.Parse(x)).ToList();
            var secIdx = path[0];
            var rowIdx = path[1];


            var section = _model.GetSection(secIdx);
            var destSection = _model.GetSection(destinationIndexPath.Section);
            if (!destSection.UseDragSort)
            {
                return;
            }

            // save scroll position
            var offset = ContentOffset;
            var fromCell = CellAt(NSIndexPath.FromRowSection(rowIdx, secIdx));

            if (section.ItemsSource == null)
            {
                // Don't use PerformBatchUpdates. Because can't cancel animations well.
                BeginUpdates();

                var cell = section.DeleteCellWithoutNotify(rowIdx);
                destSection.InsertCellWithoutNotify(cell, destinationIndexPath.Row);
                DeleteRows(GetPaths(secIdx, rowIdx, 1), UITableViewRowAnimation.None);
                InsertRows(GetPaths(destinationIndexPath.Section, destinationIndexPath.Row, 1), UITableViewRowAnimation.None);

                EndUpdates();

                _settingsView.SendItemDropped(destSection, cell);
            }
            else
            {
                // Don't use PerformBatchUpdates. Because can't cancel animations well.
                BeginUpdates();

                var deletedSet = section.DeleteSourceItemWithoutNotify(rowIdx);
                destSection.InsertSourceItemWithoutNotify(deletedSet.Cell, deletedSet.Item, destinationIndexPath.Row);
                DeleteRows(GetPaths(secIdx, rowIdx, 1), UITableViewRowAnimation.None);
                InsertRows(GetPaths(destinationIndexPath.Section, destinationIndexPath.Row, 1), UITableViewRowAnimation.None);

                EndUpdates();
                _settingsView.SendItemDropped(destSection, deletedSet.Cell);
            }

            // Cancel animations and restore the scroll position.
            var toCell = CellAt(destinationIndexPath);
            toCell?.Layer?.RemoveAllAnimations();
            fromCell?.Layer?.RemoveAllAnimations();
            Layer.RemoveAllAnimations();
            SetContentOffset(offset, false);

            // nothing occur, even if use the following code.
            //coordinator.DropItemToRow(coordinator.Items.First().DragItem, destinationIndexPath);
        });
    }

    protected virtual NSIndexPath[] GetPaths(int section, int index, int count)
    {
        var paths = new NSIndexPath[count];
        for (var i = 0; i < paths.Length; i++)
        {
            paths[i] = NSIndexPath.FromRowSection(index + i, section);
        }

        return paths;
    }

    /// <summary>
    /// Ensure that the drop session contains a drag item with a data representation that the view can consume.
    /// </summary>
    [Export("tableView:canHandleDropSession:")]
    public bool CanHandleDropSession(UITableView tableView, IUIDropSession session)
    {
        return session.CanLoadObjects(typeof(NSString));
    }

    [Export("tableView:dropSessionDidEnter:")]
    public void DropSessionDidEnter(UITableView tableView, IUIDropSession session)
    {
    }

    [Export("tableView:dropSessionDidEnd:")]
    public void DropSessionDidEnd(UITableView tableView, IUIDropSession session)
    {
    }

    [Export("tableView:dropSessionDidExit:")]
    public void DropSessionDidExit(UITableView tableView, IUIDropSession session)
    {
    }

    /// <summary>
    /// A drop proposal from a table view includes two items: a drop operation,
    /// typically .move or .copy; and an intent, which declares the action the
    /// table view will take upon receiving the items. (A drop proposal from a
    /// custom view does includes only a drop operation, not an intent.)
    /// </summary>
    [Export("tableView:dropSessionDidUpdate:withDestinationIndexPath:")]
    public UITableViewDropProposal DropSessionDidUpdate(UITableView tableView, IUIDropSession session, NSIndexPath destinationIndexPath)
    {
        if (destinationIndexPath == null)
        {
            return new UITableViewDropProposal(UIDropOperation.Cancel);
        }

        // this dragging is from UITableView.
        if (tableView.HasActiveDrag)
        {
            if (session.Items.Length > 1)
            {
                return new UITableViewDropProposal(UIDropOperation.Cancel);
            }
            else
            {
                return new UITableViewDropProposal(UIDropOperation.Move, UITableViewDropIntent.Automatic);
            }
        }

        return new UITableViewDropProposal(UIDropOperation.Cancel);
    }
}

