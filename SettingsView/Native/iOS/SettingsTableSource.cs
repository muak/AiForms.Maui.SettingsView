﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AiForms.Settings.Extensions;
using AiForms.Settings.Handlers;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using ObjCRuntime;
using UIKit;

namespace AiForms.Settings.Platforms.iOS;

/// <summary>
/// Settings table source.
/// </summary>
[Foundation.Preserve(AllMembers = true)]
public class SettingsTableSource : UITableViewSource
{
    /// <summary>
    /// The table view.
    /// </summary>
    protected UITableView _tableView;
    /// <summary>
    /// The settings view.
    /// </summary>
    protected SettingsView _settingsView;

    bool _disposed;
    HashSet<IElementHandler> _cellHandlers = new HashSet<IElementHandler>();

    Lazy<IFontManager> _fontManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.SettingsTableSource"/> class.
    /// </summary>
    /// <param name="settingsView">Settings view.</param>
    public SettingsTableSource(SettingsView settingsView)
    {
        _settingsView = settingsView;
        _settingsView.ModelChanged += (sender, e) => {
            if (_tableView != null) {
                _tableView.ReloadData();
                // reflect a dynamic cell height
                _tableView.PerformBatchUpdates(null, null);
            }
        };

        _fontManager = new Lazy<IFontManager>(() =>
        {
            return settingsView.FindMauiContext().Services.GetService<IFontManager>();
        });
    }

    /// <summary>
    /// Gets the cell.
    /// </summary>
    /// <returns>The cell.</returns>
    /// <param name="tableView">Table view.</param>
    /// <param name="indexPath">Index path.</param>
    public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
    {
        //get Maui cell
        var cell = _settingsView.Model.GetCell(indexPath.Section, indexPath.Row);

        var id = cell.GetType().FullName;
        //get native recycle cell
        var reusableCell = tableView.DequeueReusableCell(id);

        if(cell.Handler != null)
        {
            _cellHandlers.Remove(cell.Handler);
            // disconnect old nativeview;
            cell.Handler.DisconnectHandler();
            cell.Handler = null;
        }
        
        cell.ReusableCell = reusableCell;
        cell.TableView = tableView;

        // connect the next cell to the reusableNativeView or a new nativeView
        var handler = cell.ToHandler(cell.FindMauiContext());

        _cellHandlers.Add(handler);

        var platformCell = handler.PlatformView as CellBaseView;

        var cellWithContent = platformCell;

        // Sometimes iOS for returns a dequeued cell whose Layer is hidden. 
        // This prevents it from showing up, so lets turn it back on!
        if (cellWithContent.Layer.Hidden)
        {
            cellWithContent.Layer.Hidden = false;
        }

        // Because the layer was hidden we need to layout the cell by hand
        if (cellWithContent != null)
        {
            //cellWithContent.LayoutSubviews();
        }

        return platformCell;           
    }    

    /// <summary>
    /// Gets the height for row.
    /// </summary>
    /// <returns>The height for row.</returns>
    /// <param name="tableView">Table view.</param>
    /// <param name="indexPath">Index path.</param>
    public override NFloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
    {
        var cell = _settingsView.Model.GetCell(indexPath.Section, indexPath.Row);
        if (!cell.IsVisible)
        {
            return NFloat.Epsilon;
        }
        
        if (!_settingsView.HasUnevenRows) {
            return tableView.EstimatedRowHeight;
        }
        
        var h = cell.Height;

        if (h == -1) {
            //automatic height
            return tableView.RowHeight;
        }

        //individual height
        return (NFloat)h;
    }

    /// <summary>
    /// section header height
    /// </summary>
    /// <returns>The height for header.</returns>
    /// <param name="tableView">Table view.</param>
    /// <param name="section">Section.</param>
    public override NFloat GetHeightForHeader(UITableView tableView, nint section)
    {
        var sec = _settingsView.Model.GetSection((int)section);

        if (!sec.IsVisible)
        {
            return NFloat.Epsilon;
        }

        if(sec.HeaderView != null)
        {
            return UITableView.AutomaticDimension; // automatic height
        }

        var individualHeight = sec.HeaderHeight;

        if(individualHeight > 0d){
            return (NFloat)individualHeight;
        }
        if (_settingsView.HeaderHeight == -1d) {
            return UITableView.AutomaticDimension;
        }

        return (NFloat)_settingsView.HeaderHeight;
    }

    /// <summary>
    /// Gets the view for header.
    /// </summary>
    /// <returns>The view for header.</returns>
    /// <param name="tableView">Table view.</param>
    /// <param name="section">Section.</param>
    public override UIView GetViewForHeader(UITableView tableView, nint section)
    {
        var formsView = _settingsView.Model.GetSectionHeaderView((int)section);
        if (formsView != null)
        {
            return GetNativeSectionHeaderFooterView(formsView, tableView, true);
        }


        var headerView = _tableView.DequeueReusableHeaderFooterView(SettingsViewHandler.TextHeaderId) as TextHeaderView;
        if(headerView is null)
        {
            // for HotReload
            return new UIView();
        }

        headerView.Label.Text = _settingsView.Model.GetSectionTitle((int)section);
        headerView.Label.TextColor = _settingsView.HeaderTextColor.IsDefault() ?
            UIColor.Gray : _settingsView.HeaderTextColor.ToPlatform();

        headerView.Label.Font = _settingsView.HeaderFontFamily
            .ToFont(_settingsView.HeaderFontSize, _settingsView.HeaderFontAttributes)
            .ToUIFont(_fontManager.Value);

        headerView.BackgroundView.BackgroundColor = _settingsView.HeaderBackgroundColor.ToPlatform();
        headerView.Label.Padding = _settingsView.HeaderPadding.ToUIEdgeInsets();

        var sec = _settingsView.Model.GetSection((int)section);
        if(sec.HeaderHeight != -1 || _settingsView.HeaderHeight != -1)
        {
            headerView.SetVerticalAlignment(_settingsView.HeaderTextVerticalAlign);
        }

        return headerView;
    }

    /// <summary>
    /// section footer height
    /// </summary>
    /// <returns>The height for footer.</returns>
    /// <param name="tableView">Table view.</param>
    /// <param name="section">Section.</param>
    public override NFloat GetHeightForFooter(UITableView tableView, nint section)
    {
        var sec = _settingsView.Model.GetSection((int)section);

        if (!sec.IsVisible)
        {
            return NFloat.Epsilon;
        }

        if(!sec.FooterVisible)
        {
            return NFloat.Epsilon;
        }

        if (sec.FooterView != null)
        {
            return UITableView.AutomaticDimension; // automatic height
        }

        var footerText = sec.FooterText;

        if (string.IsNullOrEmpty(footerText)) {
            //hide footer
            return NFloat.Epsilon; // must not zero
        }

        return UITableView.AutomaticDimension;
    }

    /// <summary>
    /// Gets the view for footer.
    /// </summary>
    /// <returns>The view for footer.</returns>
    /// <param name="tableView">Table view.</param>
    /// <param name="section">Section.</param>
    public override UIView GetViewForFooter(UITableView tableView, nint section)
    {
        var formsView = _settingsView.Model.GetSectionFooterView((int)section);
        if (formsView != null)
        {
            return GetNativeSectionHeaderFooterView(formsView, tableView, false);
        }

        var text = _settingsView.Model.GetFooterText((int)section);

        if (string.IsNullOrEmpty(text)) {
            return new UIView(CGRect.Empty);
        }

        var footerView = _tableView.DequeueReusableHeaderFooterView(SettingsViewHandler.TextFooterId) as TextFooterView;

        if (footerView is null)
        {
            // for HotReload
            return new UIView();
        }            

        footerView.Label.Text = text;
        footerView.Label.TextColor = _settingsView.FooterTextColor.IsDefault() ?
            UIColor.Gray : _settingsView.FooterTextColor.ToPlatform();

        footerView.Label.Font = _settingsView.FooterFontFamily
            .ToFont(_settingsView.FooterFontSize, _settingsView.FooterFontAttributes)
            .ToUIFont(_fontManager.Value); 

        footerView.BackgroundView.BackgroundColor = _settingsView.FooterBackgroundColor.ToPlatform();
        footerView.Label.Padding = _settingsView.FooterPadding.ToUIEdgeInsets();

        return footerView;
    }

    UIView GetNativeSectionHeaderFooterView(View formsView, UITableView tableView, bool isHeader)
    {
        var idString = isHeader ? SettingsViewHandler.CustomHeaderId : SettingsViewHandler.CustomFooterId;
        var nativeView = tableView.DequeueReusableHeaderFooterView(idString) as CustomHeaderFooterView;
        nativeView.UpdateCell(formsView,tableView);

        return nativeView;
    }

    /// <summary>
    /// Numbers the of sections.
    /// </summary>
    /// <returns>The of sections.</returns>
    /// <param name="tableView">Table view.</param>
    public override nint NumberOfSections(UITableView tableView)
    {
        _tableView = tableView;
        return _settingsView.Model.GetSectionCount();
    }

    /// <summary>
    /// Rowses the in section.
    /// </summary>
    /// <returns>The in section.</returns>
    /// <param name="tableview">Tableview.</param>
    /// <param name="section">Section.</param>
    public override nint RowsInSection(UITableView tableview, nint section)
    {
        var sec = _settingsView.Model.GetSection((int)section);
        return sec.IsVisible ? sec.Count : 0;
    }


    public override bool ShouldShowMenu(UITableView tableView, NSIndexPath rowAtindexPath)
    {
        if(_settingsView.Model.GetSection(rowAtindexPath.Section).UseDragSort)
        {
            return false;
        }

        var ret = false;
        if(tableView.CellAt(rowAtindexPath) is CellBaseView cell)
        {
            System.Diagnostics.Debug.WriteLine("LongTap");
            ret = cell.RowLongPressed(tableView, rowAtindexPath);
        }

        if(ret)
        {
            BeginInvokeOnMainThread(async () => {
                await Task.Delay(250);
                tableView.CellAt(rowAtindexPath).SetSelected(false, true);
            });
        }

        return ret;
    }

    public override bool CanPerformAction(UITableView tableView, Selector action, NSIndexPath indexPath, NSObject sender)
    {
        return false;
    }

    public override void PerformAction(UITableView tableView, Selector action, NSIndexPath indexPath, NSObject sender)
    {
    }

    /// <summary>
    /// Title text string array (unknown what to do ) 
    /// </summary>
    /// <returns>The index titles.</returns>
    /// <param name="tableView">Table view.</param>
    public override string[] SectionIndexTitles(UITableView tableView)
    {
        return _settingsView.Model.GetSectionIndexTitles();
    }

    /// <summary>
    /// processing when row is selected.
    /// </summary>
    /// <param name="tableView">Table view.</param>
    /// <param name="indexPath">Index path.</param>
    public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
    {
        _settingsView.Model.OnRowSelected(indexPath.Section, indexPath.Row);

        if (tableView.CellAt(indexPath) is CellBaseView cell)
        {
            cell.RowSelected(tableView, indexPath);
        }
    }       

    /// <summary>
    /// Dispose the specified disposing.
    /// </summary>
    /// <returns>The dispose.</returns>
    /// <param name="disposing">If set to <c>true</c> disposing.</param>
    protected override void Dispose(bool disposing)
    {
        if (!_disposed){

            foreach(var section in _settingsView.Root)
            {
                foreach(var cell in section)
                {
                    cell.ReusableCell = null;
                    cell.TableView = null;
                }
            }

            foreach(var handler in _cellHandlers)
            {
                handler.DisconnectHandler();
            }
            _cellHandlers.Clear();
            _cellHandlers = null;

            _fontManager = null;
            _settingsView = null;
            _tableView = null;               
        }

        _disposed = true;

        base.Dispose(disposing);
    }
}
