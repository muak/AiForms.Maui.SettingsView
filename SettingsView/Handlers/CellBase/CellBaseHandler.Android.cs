using System;
using System.Linq.Expressions;
using System.Reflection;
using AiForms.Settings.Platforms.Droid;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Microsoft.Maui.Handlers;
using AView = Android.Views.View;

namespace AiForms.Settings.Handlers
{
    public partial class CellBaseHandler<TvirtualCell, TnativeCell> 
    {
        public static IPropertyMapper<CellBase, CellBaseHandler<TvirtualCell, TnativeCell>> BasePropertyMapper =
            new PropertyMapper<CellBase, CellBaseHandler<TvirtualCell, TnativeCell>>(ElementMapper)
            {
                [nameof(CellBase.Title)] = MapTitleText,
                [nameof(CellBase.TitleColor)] = MapTitleColor,
                [nameof(CellBase.TitleFontSize)] = MapTitleFontSize,
                [nameof(CellBase.TitleFontAttributes)] = MapTitleFont,
                [nameof(CellBase.TitleFontFamily)] = MapTitleFont,
                [nameof(CellBase.Description)] = MapDescriptionText,
                [nameof(CellBase.DescriptionColor)] = MapDescriptionColor,
                [nameof(CellBase.DescriptionFontSize)] = MapDescriptionFontSize,
                [nameof(CellBase.DescriptionFontAttributes)] = MapDescriptionFont,
                [nameof(CellBase.DescriptionFontFamily)] = MapDescriptionFont,
                [nameof(CellBase.HintText)] = MapHintText,
                [nameof(CellBase.HintTextColor)] = MapHintTextColor,
                [nameof(CellBase.HintFontSize)] = MapHintFontSize,
                [nameof(CellBase.HintFontAttributes)] = MapHintFont,
                [nameof(CellBase.HintFontFamily)] = MapHintFont,
                [nameof(CellBase.BackgroundColor)] = MapBackgroundColor,
                [nameof(CellBase.IconSource)] = MapIconSource,
                [nameof(CellBase.IconSize)] = MapIconSource,
                [nameof(CellBase.IconRadius)] = MapIconRadius,
                [nameof(CellBase.IsEnabled)] = MapIsEnabled,
            };

        internal static class InstanceCreator<T1, T2, TInstance>
        {
            public static Func<T1, T2, TInstance> Create { get; } = CreateInstance();

            private static Func<T1, T2, TInstance> CreateInstance()
            {
                var argsTypes = new[] { typeof(T1), typeof(T2) };
                var constructor = typeof(TInstance).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder,
                       argsTypes, null);
                var args = argsTypes.Select(Expression.Parameter).ToArray();
                return Expression.Lambda<Func<T1, T2, TInstance>>(Expression.New(constructor, args), args).Compile();
            }
        }

        protected override TnativeCell CreatePlatformElement()
        {
            var convertView = VirtualView.ConvertView;
            VirtualView.ConvertView = null;

            return GetCell(VirtualView, convertView, null, MauiContext.Context);
        }

        public virtual TnativeCell GetCell(CellBase item, AView convertView, ViewGroup parent, Context context)
        {
            TnativeCell nativeCell = convertView as TnativeCell;
            if (nativeCell == null)
            {
                nativeCell = InstanceCreator<Context, CellBase, TnativeCell>.Create(context, item);
            }

            ClearPropertyChanged(nativeCell);

            nativeCell.Cell = item;

            SetUpPropertyChanged(nativeCell);

            nativeCell.UpdateCell(); // Initialization process not handled by Mapper

            return nativeCell;
        }        

        private static void MapTitleText(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView)handler.PlatformView)?.UpdateTitleText();
        }

        private static void MapTitleColor(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView)handler.PlatformView)?.UpdateTitleColor();
        }

        private static void MapTitleFontSize(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView)handler.PlatformView)?.UpdateTitleFontSize();
        }

        private static void MapTitleFont(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeView = handler.PlatformView as CellBaseView;
            nativeView?.UpdateTitleFont();
            nativeView?.UpdateLayout();
        }

        private static void MapDescriptionText(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView)handler.PlatformView)?.UpdateDescriptionText();
        }

        private static void MapDescriptionColor(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView)handler.PlatformView)?.UpdateDescriptionColor();
        }

        private static void MapDescriptionFontSize(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView)handler.PlatformView)?.UpdateDescriptionFontSize();
        }

        private static void MapDescriptionFont(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeView = handler.PlatformView as CellBaseView;
            nativeView?.UpdateDescriptionFont();
        }

        private static void MapHintText(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeView = handler.PlatformView as CellBaseView;
            nativeView?.UpdateHintText();
            nativeView?.UpdateLayout();
        }

        private static void MapHintTextColor(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView)handler.PlatformView)?.UpdateHintTextColor();
        }

        private static void MapHintFontSize(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeView = handler.PlatformView as CellBaseView;
            nativeView?.UpdateHintFontSize();
            nativeView?.UpdateLayout();
        }

        private static void MapHintFont(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeView = handler.PlatformView as CellBaseView;
            nativeView?.UpdateHintFont();
            nativeView?.UpdateLayout();
        }

        private static void MapIconSource(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeView = handler.PlatformView as CellBaseView;
            nativeView?.UpdateIcon();
        }

        private static void MapIconRadius(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            var nativeView = handler.PlatformView as CellBaseView;
            nativeView?.UpdateIconRadius();
            nativeView?.UpdateIcon(true);
        }

        private static void MapIsEnabled(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView)handler.PlatformView)?.UpdateIsEnabled();
        }

        private static void MapBackgroundColor(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase cell)
        {
            ((CellBaseView)handler.PlatformView)?.UpdateBackgroundColor();
        }
    }
}

