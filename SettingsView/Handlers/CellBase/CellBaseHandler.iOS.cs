#nullable enable
using System;
using System.Linq.Expressions;
using System.Reflection;
using AiForms.Settings.Platforms.iOS;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using UIKit;

namespace AiForms.Settings.Handlers
{
    [Foundation.Preserve(AllMembers = true)]
    public partial class CellBaseHandler<TvirtualCell, TnativeCell> : ElementHandler<TvirtualCell, TnativeCell>, IRegisterable
        where TvirtualCell : CellBase
        where TnativeCell : CellBaseView
    {
        public static IPropertyMapper<CellBase, CellBaseHandler<TvirtualCell, TnativeCell>> BasePropertyMapper =
            new PropertyMapper<CellBase, CellBaseHandler<TvirtualCell, TnativeCell>>(ElementMapper)
            {
                [nameof(CellBase.Title)] = MapTitleText,
                [nameof(CellBase.TitleColor)] = MapTitleColor,
                [nameof(CellBase.TitleFontSize)] = MapTitleFont,
                [nameof(CellBase.TitleFontAttributes)] = MapTitleFont,
                [nameof(CellBase.TitleFontFamily)] = MapTitleFont,
                [nameof(CellBase.Description)] = MapDescriptionText,
                [nameof(CellBase.DescriptionColor)] = MapDescriptionColor,
                [nameof(CellBase.DescriptionFontSize)] = MapDescriptionFont,
                [nameof(CellBase.DescriptionFontAttributes)] = MapDescriptionFont,
                [nameof(CellBase.DescriptionFontFamily)] = MapDescriptionFont,
                [nameof(CellBase.HintText)] = MapHintText,
                [nameof(CellBase.HintTextColor)] = MapHintTextColor,
                [nameof(CellBase.HintFontSize)] = MapHintFont,
                [nameof(CellBase.HintFontAttributes)] = MapHintFont,
                [nameof(CellBase.HintFontFamily)] = MapHintFont,
                [nameof(CellBase.BackgroundColor)] = MapBackgroundColor,
                [nameof(CellBase.IconSource)] = MapIconSource,
                [nameof(CellBase.IconSize)] = MapIconSize,
                [nameof(CellBase.IconRadius)] = MapIconRadius,
                [nameof(CellBase.IsVisible)] = MapIsVisible,
                [nameof(CellBase.IsEnabled)] = MapIsEnabled,
            };

        /// <summary>
        /// Refer to 
        /// http://qiita.com/Temarin/items/d6f00428743b0971ec95
        /// http://neue.cc/2014/09/16_478.html
        /// </summary>
        internal static class InstanceCreator<T1, TInstance>
        {
            public static Func<T1, TInstance> Create { get; } = CreateInstance();

            private static Func<T1, TInstance> CreateInstance()
            {
                var constructor = typeof(TInstance).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder,
                    new[] { typeof(T1) }, null);
                var handler = Expression.Parameter(typeof(T1));
                return Expression.Lambda<Func<T1, TInstance>>(Expression.New(constructor!, handler), handler).Compile();
            }
        }

        protected override TnativeCell CreatePlatformElement()
        {
            var reusableCell = VirtualView.ReusableCell;
            var tv = VirtualView.TableView;
            VirtualView.ReusableCell = null;
            VirtualView.TableView = null;

            return GetCell(VirtualView, reusableCell, tv);
        }

        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="item">Item.</param>
        /// <param name="reusableCell">Reusable cell.</param>
        /// <param name="tv">Tv.</param>
        public virtual TnativeCell GetCell(CellBase item, UITableViewCell reusableCell, UITableView tv)
        {
            var nativeCell = reusableCell as TnativeCell;
            if (nativeCell == null)
            {
                nativeCell = InstanceCreator<CellBase, TnativeCell>.Create(item);
            }

            ClearPropertyChanged(nativeCell);

            nativeCell.Cell = item;

            SetUpPropertyChanged(nativeCell);

            nativeCell.UpdateCell(tv); // Initialization process not handled by Mapper

            return nativeCell;
        }

        private static void MapTitleText(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView?)handler.PlatformView)?.UpdateTitleText();
        }

        private static void MapTitleColor(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView?)handler.PlatformView)?.UpdateTitleColor();
        }

        private static void MapTitleFont(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeView = handler.PlatformView as CellBaseView;
            nativeView?.UpdateTitleFont();
            nativeView?.UpdateLayout();
        }

        private static void MapDescriptionText(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeView = handler.PlatformView as CellBaseView;
            nativeView?.UpdateDescriptionText();
            nativeView?.UpdateLayout();
        }

        private static void MapDescriptionColor(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView?)handler.PlatformView)?.UpdateDescriptionColor();
        }

        private static void MapDescriptionFont(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeView = handler.PlatformView as CellBaseView;
            nativeView?.UpdateDescriptionFont();
            nativeView?.UpdateLayout();
        }

        private static void MapHintText(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeCell = handler.PlatformView as CellBaseView;
            nativeCell?.UpdateHintText();
            nativeCell?.UpdateLayout();
        }

        private static void MapHintTextColor(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView?)handler.PlatformView)?.UpdateHintTextColor();
        }

        private static void MapHintFont(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeView = handler.PlatformView as CellBaseView;
            nativeView?.UpdateHintFont();
            nativeView?.UpdateLayout();
        }

        private static void MapIconRadius(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeView = handler.PlatformView as CellBaseView;
            nativeView?.UpdateIconRadius();
            nativeView?.UpdateLayout();
        }

        private static void MapIconSize(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeView = handler.PlatformView as CellBaseView;
            nativeView?.UpdateIconSize();
            nativeView?.UpdateLayout();
        }

        private static void MapIconSource(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeView = handler.PlatformView as CellBaseView;
            nativeView?.UpdateIcon();
        }

        private static void MapIsVisible(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView?)handler.PlatformView)?.UpdateIsVisible();
        }

        private static void MapIsEnabled(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView?)handler.PlatformView)?.UpdateIsEnabled();
        }

        private static void MapBackgroundColor(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView?)handler.PlatformView)?.UpdateBackgroundColor();
        }
    }
}

