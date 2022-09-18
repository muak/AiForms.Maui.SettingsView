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
            handlers.AddHandler(typeof(ButtonCell), typeof(ButtonCellHandler));
            handlers.AddHandler(typeof(CheckboxCell), typeof(CheckboxCellHandler));
            handlers.AddHandler(typeof(CommandCell), typeof(CommandCellHandler));
            handlers.AddHandler(typeof(DatePickerCell), typeof(DatePickerCellHandler));
            handlers.AddHandler(typeof(EntryCell), typeof(EntryCellHandler));
            handlers.AddHandler(typeof(NumberPickerCell), typeof(NumberPickerCellHandler));
            handlers.AddHandler(typeof(RadioCell), typeof(RadioCellHandler));
            handlers.AddHandler(typeof(SwitchCell), typeof(SwitchCellHandler));
            handlers.AddHandler(typeof(TextPickerCell), typeof(TextPickerCellHandler));
            handlers.AddHandler(typeof(TimePickerCell), typeof(TimePickerCellHandler));
            handlers.AddHandler(typeof(CustomCell), typeof(CustomCellHandler));
            handlers.AddHandler(typeof(SimpleCheckCell), typeof(SimpleCheckCellHandler));
            //handlers.AddHandler(typeof(PickerCell), typeof(CommandCellHandler));
        }
    }
}

