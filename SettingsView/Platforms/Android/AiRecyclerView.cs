using System;
using AiForms.Settings.Platforms.Droid;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Platform;
using View = Microsoft.Maui.Controls.View;
using AView = Android.Views.View;

namespace AiForms.Settings.Platforms.Droid;

[Android.Runtime.Preserve(AllMembers = true)]
public class AiRecyclerView : RecyclerView
{
    SettingsViewRecyclerAdapter _adapter;
    SettingsViewLayoutManager _layoutManager;
    ItemTouchHelper _itemTouchhelper;
    SettingsViewSimpleCallback _simpleCallback;
    SVItemdecoration _itemDecoration;
    Drawable _divider;
    Page _parentPage;
    SettingsView _settingsView;

    List<IPlatformViewHandler> _shouldDisposeHandlers = new List<IPlatformViewHandler>();

    // Fix scrollbar visibility and flash. https://github.com/xamarin/Xamarin.Forms/pull/10893
    public AiRecyclerView(Context context, SettingsView settingsView) : base(new ContextThemeWrapper(context, Resource.Style.settingsViewTheme),null, Resource.Attribute.settingsViewStyle)
    {
        _settingsView = settingsView;
        NestedScrollingEnabled = false;

        _layoutManager = new SettingsViewLayoutManager(Context, settingsView);
        SetLayoutManager(_layoutManager);

        _divider = Context.GetDrawable(Resource.Drawable.divider);
        _itemDecoration = new SVItemdecoration(_divider, settingsView);
        AddItemDecoration(_itemDecoration);

        Focusable = false;
        DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;

        _adapter = new SettingsViewRecyclerAdapter(Context, settingsView, this);
        SetAdapter(_adapter);

        _simpleCallback = new SettingsViewSimpleCallback(settingsView, ItemTouchHelper.Up | ItemTouchHelper.Down, 0);
        _itemTouchhelper = new ItemTouchHelper(_simpleCallback);
        _itemTouchhelper.AttachToRecyclerView(this);

        settingsView.Root.CollectionChanged += RootCollectionChanged;

        Element elm = settingsView;
        while (elm != null)
        {
            elm = elm.Parent;
            if (elm is Page)
            {
                break;
            }
        }

        _parentPage = elm as Page;
        _parentPage.Appearing += ParentPageAppearing;
    }

    public AiRecyclerView(Context context, IAttributeSet attrs) : base(context, attrs)
    {
    }

    public AiRecyclerView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
    {
    }

    protected AiRecyclerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
    {
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var section in _settingsView.Root)
            {
                if (section.HeaderView != null)
                {
                    DisposeChildView(section.HeaderView);
                }
                if (section.FooterView != null)
                {
                    DisposeChildView(section.FooterView);
                }
            }

            foreach (var handler in _shouldDisposeHandlers)
            {
                handler.DisconnectHandler();
                if (handler.PlatformView.Handle != IntPtr.Zero)
                {
                    handler.PlatformView.RemoveFromParent();
                    handler.PlatformView.Dispose();
                }                
            }

            _shouldDisposeHandlers = null;

            RemoveItemDecoration(_itemDecoration);
            _parentPage.Appearing -= ParentPageAppearing;
            _parentPage = null;
            _adapter?.Dispose();
            _adapter = null;
            _layoutManager?.Dispose();
            _layoutManager = null;
            _simpleCallback?.Dispose();
            _simpleCallback = null;
            _itemTouchhelper?.Dispose();
            _itemTouchhelper = null;

            _itemDecoration?.Dispose();
            _itemDecoration = null;
            _divider?.Dispose();
            _divider = null;

            _settingsView.Root.CollectionChanged -= RootCollectionChanged;
            _settingsView = null;
        }
        base.Dispose(disposing);
    }

    void DisposeChildView(View view)
    {
        view.Handler?.DisconnectHandler();
        var platformView = view.Handler?.PlatformView as AView;
        platformView?.RemoveFromParent();
        platformView?.Dispose();
    }

    void ParentPageAppearing(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() => _adapter?.DeselectRow());
    }

    void RootCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems == null)
        {
            return;
        }

        foreach (Section section in e.OldItems)
        {
            if (section.HeaderView != null)
            {
                var header = (IPlatformViewHandler)section.HeaderView.Handler;
                if (header != null)
                {
                    _shouldDisposeHandlers.Add(header);
                }
            }
            if (section.FooterView != null)
            {
                var footer = (IPlatformViewHandler)section.FooterView.Handler;
                if (footer != null)
                {
                    _shouldDisposeHandlers.Add(footer);
                }
            }
        }
    }

    internal void UpdateSeparatorColor()
    {
        _divider.SetTint(_settingsView.SeparatorColor.ToPlatform());
        InvalidateItemDecorations();
    }

    internal void UpdateRowHeight()
    {
        if (_settingsView.RowHeight == -1)
        {
            _settingsView.RowHeight = 60;
        }
        else
        {
            _adapter?.NotifyDataSetChanged();
        }
    }

    internal void UpdateScrollToTop()
    {
        if (_settingsView.ScrollToTop)
        {
            _layoutManager.ScrollToPosition(0);
            _settingsView.ScrollToTop = false;
        }
    }

    internal void UpdateScrollToBottom()
    {
        if (_settingsView.ScrollToBottom)
        {
            if (_adapter != null)
            {
                _layoutManager.ScrollToPosition(_adapter.ItemCount - 1);
            }
            _settingsView.ScrollToBottom = false;
        }
    }

    internal void UpdateBackgroundColor()
    {
        if (_settingsView.BackgroundColor.IsNotDefault())
        {
            SetBackgroundColor(_settingsView.BackgroundColor.ToPlatform());
        }
    }
}

class SettingsViewSimpleCallback : ItemTouchHelper.SimpleCallback
{
    SettingsView _settingsView;
    RowInfo _fromInfo;
    Queue<(RowInfo from, RowInfo to)> _moveHistory = new Queue<(RowInfo from, RowInfo to)>();

    public SettingsViewSimpleCallback(SettingsView settingsView, int dragDirs, int swipeDirs) : base(dragDirs, swipeDirs)
    {
        _settingsView = settingsView;
    }

    public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
    {
        System.Diagnostics.Debug.WriteLine("OnMove");
        if (!(viewHolder is ContentBodyViewHolder fromContentHolder))
        {
            System.Diagnostics.Debug.WriteLine("Cannot move no ContentHolder");
            return false;
        }

        var fromPos = viewHolder.AbsoluteAdapterPosition;
        var toPos = target.AbsoluteAdapterPosition;

        if (fromPos < toPos)
        {
            // disallow a Footer when drag is from up to down.
            if (target is IFooterViewHolder)
            {
                System.Diagnostics.Debug.WriteLine("Up To Down disallow Footer");
                return false;
            }
        }
        else
        {
            // disallow a Header when drag is from down to up.
            if (target is IHeaderViewHolder)
            {
                System.Diagnostics.Debug.WriteLine("Down To Up disallow Header");
                return false;
            }
        }

        var toContentHolder = target as ViewHolder;

        var section = fromContentHolder.RowInfo.Section;
        if (section == null || !section.UseDragSort)
        {
            System.Diagnostics.Debug.WriteLine("From Section Not UseDragSort");
            return false;
        }

        var toSection = toContentHolder.RowInfo.Section;
        if (toSection == null || !toSection.UseDragSort)
        {
            System.Diagnostics.Debug.WriteLine("To Section Not UseDragSort");
            return false;
        }

        var toInfo = toContentHolder.RowInfo;
        System.Diagnostics.Debug.WriteLine($"Set ToInfo Section:{_settingsView.Root.IndexOf(toInfo.Section)} Cell:{toInfo.Section.IndexOf(toInfo.Cell)}");

        var settingsAdapter = recyclerView.GetAdapter() as SettingsViewRecyclerAdapter;

        settingsAdapter.CellMoved(fromPos, toPos); //caches update
        settingsAdapter.NotifyItemMoved(fromPos, toPos); //rows update

        // save moved changes 
        _moveHistory.Enqueue((_fromInfo, toInfo));

        System.Diagnostics.Debug.WriteLine($"Move Completed from:{fromPos} to:{toPos}");

        return true;
    }

    void DataSourceMoved()
    {
        if (!_moveHistory.Any())
        {
            return;
        }

        var cell = _moveHistory.Peek().from.Cell;
        var section = _moveHistory.Last().to.Section;
        while (_moveHistory.Any())
        {
            var pos = _moveHistory.Dequeue();
            DataSourceMoved(pos.from, pos.to);
        }

        _settingsView.SendItemDropped(section, cell);
    }

    void DataSourceMoved(RowInfo from, RowInfo to)
    {
        var fromPos = from.Section.IndexOf(from.Cell);
        var toPos = to.Section.IndexOf(to.Cell);
        if (toPos < 0)
        {
            // if Header, insert the first.s
            toPos = 0;
        }

        if (from.Section.ItemsSource == null)
        {
            System.Diagnostics.Debug.WriteLine($"Update Sections from:{fromPos} to:{toPos}");
            var cell = from.Section.DeleteCellWithoutNotify(fromPos);
            to.Section.InsertCellWithoutNotify(cell, toPos);
        }
        else
        {
            System.Diagnostics.Debug.WriteLine($"UpdateSource from:{fromPos} to:{toPos}");
            var deletedSet = from.Section.DeleteSourceItemWithoutNotify(fromPos);
            to.Section.InsertSourceItemWithoutNotify(deletedSet.Cell, deletedSet.Item, toPos);
        }

        from.Section = to.Section;
    }

    public override void ClearView(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
    {
        System.Diagnostics.Debug.WriteLine("On ClearView");
        base.ClearView(recyclerView, viewHolder);

        viewHolder.ItemView.Alpha = 1.0f;
        viewHolder.ItemView.ScaleX = 1.0f;
        viewHolder.ItemView.ScaleY = 1.0f;

        // DataSource Update
        DataSourceMoved();

        _moveHistory.Clear();
        _fromInfo = null;
    }

    public override int GetDragDirs(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
    {
        var contentHolder = viewHolder as ContentBodyViewHolder;
        if (contentHolder == null)
        {
            return 0;
        }

        var section = contentHolder.RowInfo.Section;
        if (section == null || !section.UseDragSort)
        {
            return 0;
        }

        if (!contentHolder.RowInfo.Cell.IsEnabled)
        {
            return 0;
        }

        // save start info.
        _fromInfo = contentHolder.RowInfo;
        System.Diagnostics.Debug.WriteLine($"DragDirs Section:{_settingsView.Root.IndexOf(_fromInfo.Section)} Cell:{_fromInfo.Section.IndexOf(_fromInfo.Cell)}");
        return base.GetDragDirs(recyclerView, viewHolder);
    }

    public override void OnSelectedChanged(RecyclerView.ViewHolder viewHolder, int actionState)
    {
        base.OnSelectedChanged(viewHolder, actionState);
        if (viewHolder == null)
        {
            return;
        }

        if (actionState == ItemTouchHelper.ActionStateDrag)
        {
            viewHolder.ItemView.Alpha = 0.9f;
            viewHolder.ItemView.ScaleX = 1.04f;
            viewHolder.ItemView.ScaleY = 1.04f;
        }
    }


    public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
    {
        throw new NotImplementedException();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _settingsView = null;
            _moveHistory.Clear();
            _moveHistory = null;
        }
        base.Dispose(disposing);
    }
}

