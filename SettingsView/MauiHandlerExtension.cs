using System;
using AiForms.Settings.Handlers;

namespace AiForms.Settings
{
    public static class MauiHandlerExtension
    {
        public static void AddSettingsViewHandler(this IMauiHandlersCollection handlers)
        {
            handlers.AddHandler(typeof(SettingsView), typeof(SettingsViewHandler));
            handlers.AddHandler(typeof(LabelCell), typeof(LabelCellHandler));
        }
    }
}

