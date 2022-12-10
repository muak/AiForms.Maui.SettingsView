#nullable enable
using System;
using System.Linq.Expressions;
using System.Reflection;
using AiForms.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#if IOS || MACCATALYST
using CellBaseView = AiForms.Settings.Platforms.iOS.CellBaseView;
#elif ANDROID
using CellBaseView = AiForms.Settings.Platforms.Droid.CellBaseView;
#else
using CellBaseView = System.Object;
#endif

namespace AiForms.Settings.Handlers
{
    public partial class CellBaseHandler<TvirtualCell, TnativeCell>
    {
        public static CommandMapper<CellBase, CellBaseHandler<TvirtualCell, TnativeCell>> BaseCommandMapper = new CommandMapper<CellBase, CellBaseHandler<TvirtualCell, TnativeCell>>
        {
            [nameof(CellBase.SetEnabledAppearance)] = MapSetEnabledAppearance
        };

        private static void MapSetEnabledAppearance(CellBaseHandler<TvirtualCell, TnativeCell> handler, CellBase view, object? param)
        {
            handler.PlatformView?.SetEnabledAppearance((bool)param!);
        }

        public CellBaseHandler(): base(BasePropertyMapper, BaseCommandMapper)
        {
        }

        public CellBaseHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper = null): base(mapper ?? BasePropertyMapper, commandMapper ?? BaseCommandMapper)
        {
        }
        

        /// <summary>
        /// Sets up property changed.
        /// </summary>
        /// <param name="nativeCell">Native cell.</param>
        protected void SetUpPropertyChanged(CellBaseView nativeCell)
        {
            var virtualCell = nativeCell.Cell;
            var parentElement = virtualCell.Parent as SettingsView;

            if (parentElement != null)
            {
                parentElement.PropertyChanged += nativeCell.ParentPropertyChanged;
                var section = parentElement.Model.GetSection(SettingsModel.GetPath(virtualCell).Item1);
                if (section != null)
                {
                    virtualCell.Section = section;
                    virtualCell.Section.PropertyChanged += nativeCell.SectionPropertyChanged;
                }
            }
        }

        protected void ClearPropertyChanged(CellBaseView nativeCell)
        {
            var virtualCell = nativeCell.Cell;

            if (virtualCell is null)
                return; // for HotReload

            var parentElement = virtualCell.Parent as SettingsView;

            if (parentElement != null)
            {
                parentElement.PropertyChanged -= nativeCell.ParentPropertyChanged;
                if (virtualCell.Section != null)
                {
                    virtualCell.Section.PropertyChanged -= nativeCell.SectionPropertyChanged;
                }
            }
        }          
    }
}

