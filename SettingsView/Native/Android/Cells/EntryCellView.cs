﻿using System;
using AiForms.Settings.Extensions;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using AiEntryCell = AiForms.Settings.EntryCell;

namespace AiForms.Settings.Platforms.Droid;

/// <summary>
/// Entry cell view.
/// </summary>
[Android.Runtime.Preserve(AllMembers = true)]
public class EntryCellView : CellBaseView, ITextWatcher,
    TextView.IOnFocusChangeListener, TextView.IOnEditorActionListener
{
    AiEntryCell _EntryCell => Cell as AiEntryCell;

    AiEditText _EditText;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.EntryCellView"/> class.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="cell">Cell.</param>
    public EntryCellView(Context context, CellBase cell) : base(context, cell)
    {
        _EditText = new AiEditText(context);

        _EditText.Focusable = true;
        _EditText.ImeOptions = ImeAction.Done;
        _EditText.SetOnEditorActionListener(this);

        _EditText.OnFocusChangeListener = this;
        _EditText.SetSingleLine(true);
        _EditText.Ellipsize = TextUtils.TruncateAt.End;

        _EditText.InputType |= InputTypes.TextFlagNoSuggestions;  //disabled spell check
        _EditText.Background.Alpha = 0;  //hide underline

        _EditText.ClearFocusAction = DoneEdit;
        Click += EntryCellView_Click;

        _EntryCell.Focused += EntryCell_Focused;

        //remove weight and change width due to fill _EditText.
        var titleParam = TitleLabel.LayoutParameters as LinearLayout.LayoutParams;
        titleParam.Weight = 0;
        titleParam.Width = ViewGroup.LayoutParams.WrapContent;
        titleParam = null;

        var lparams = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1f);

        using (lparams)
        {
            ContentStack.AddView(_EditText, lparams);
        }
    }

    public EntryCellView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public override void UpdateCell()
    {
        //UpdateValueText();
        //UpdateValueTextColor();
        //UpdateValueTextFontSize();
        //UpdateValueTextFont();
        //UpdateKeyboard();
        //UpdatePlaceholder();
        //UpdateAccentColor();
        //UpdateTextAlignment();
        //UpdateIsPassword();
        base.UpdateCell();
    }

    /// <summary>
    /// Cells the property changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        base.CellPropertyChanged(sender, e);        
    }

    /// <summary>
    /// Parents the property changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    public override void ParentPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        base.ParentPropertyChanged(sender, e);
        if (e.PropertyName == SettingsView.CellValueTextColorProperty.PropertyName)
        {
            UpdateValueTextColor();
        }
        else if (e.PropertyName == SettingsView.CellValueTextFontSizeProperty.PropertyName)
        {
            UpdateWithForceLayout(UpdateValueTextFontSize);
        }
        else if (e.PropertyName == SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
                 e.PropertyName == SettingsView.CellValueTextFontAttributesProperty.PropertyName)
        {
            UpdateWithForceLayout(UpdateValueTextFont);
        }
        else if (e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName)
        {
            UpdateAccentColor();
        }
    }

    /// <summary>
    /// Dispose the specified disposing.
    /// </summary>
    /// <returns>The dispose.</returns>
    /// <param name="disposing">If set to <c>true</c> disposing.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Click -= EntryCellView_Click;
            _EntryCell.Focused -= EntryCell_Focused;
            _EditText.RemoveFromParent();
            _EditText.SetOnEditorActionListener(null);
            _EditText.RemoveTextChangedListener(this);
            _EditText.OnFocusChangeListener = null;
            _EditText.ClearFocusAction = null;
            _EditText.Dispose();
            _EditText = null;
        }
        base.Dispose(disposing);
    }

    /// <summary>
    /// Sets the enabled appearance.
    /// </summary>
    /// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
    internal override void SetEnabledAppearance(bool isEnabled)
    {
        if (isEnabled)
        {
            _EditText.Enabled = true;
            _EditText.Alpha = 1.0f;
        }
        else
        {
            _EditText.Enabled = false;
            _EditText.Alpha = 0.3f;
        }
        base.SetEnabledAppearance(isEnabled);
    }

    void EntryCellView_Click(object sender, EventArgs e)
    {
        _EditText.RequestFocus();
        ShowKeyboard(_EditText);
    }

    internal void UpdateValueText()
    {
        _EditText.RemoveTextChangedListener(this);
        if (_EditText.Text != _EntryCell.ValueText)
        {
            _EditText.Text = _EntryCell.ValueText;
        }
        _EditText.AddTextChangedListener(this);
    }

    internal void UpdateValueTextFontSize()
    {
        if (_EntryCell.ValueTextFontSize > 0)
        {
            _EditText.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)_EntryCell.ValueTextFontSize);
        }
        else if (CellParent != null)
        {
            _EditText.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)CellParent.CellValueTextFontSize);
        }
    }

    internal void UpdateValueTextFont()
    {
        var family = _EntryCell.ValueTextFontFamily ?? CellParent?.CellValueTextFontFamily;
        var attr = _EntryCell.ValueTextFontAttributes ?? CellParent.CellValueTextFontAttributes;
        var size = _EntryCell.ValueTextFontSize > 0 ? _EntryCell.ValueTextFontSize : CellParent.CellValueTextFontSize;

        _EditText.Typeface = family.ToFont(size, attr).ToTypeface(_fontManager.Value);
    }

    internal void UpdateValueTextColor()
    {
        if (_EntryCell.ValueTextColor.IsNotDefault())
        {
            _EditText.SetTextColor(_EntryCell.ValueTextColor.ToPlatform());
        }
        else if (CellParent != null && CellParent.CellValueTextColor.IsNotDefault())
        {
            _EditText.SetTextColor(CellParent.CellValueTextColor.ToPlatform());
        }
    }

    internal void UpdateKeyboard()
    {
        _EditText.InputType = _EntryCell.Keyboard.ToInputType() | InputTypes.TextFlagNoSuggestions;
    }

    internal void UpdateIsPassword()
    {
        _EditText.TransformationMethod = _EntryCell.IsPassword ? new PasswordTransformationMethod() : null;

    }

    internal void UpdatePlaceholder()
    {
        _EditText.Hint = _EntryCell.Placeholder;

        var placeholderColor = _EntryCell.PlaceholderColor.IsDefault() ?
                                    Android.Graphics.Color.Rgb(210, 210, 210) :
                                    _EntryCell.PlaceholderColor.ToPlatform();
        _EditText.SetHintTextColor(placeholderColor);
    }

    internal void UpdateTextAlignment()
    {
        _EditText.Gravity = _EntryCell.TextAlignment.ToGravityFlags();
    }

    internal void UpdateAccentColor()
    {
        if (_EntryCell.AccentColor.IsNotDefault())
        {
            ChangeTextViewBack(_EntryCell.AccentColor.ToPlatform());
        }
        else if (CellParent != null && CellParent.CellAccentColor.IsNotDefault())
        {
            ChangeTextViewBack(CellParent.CellAccentColor.ToPlatform());
        }
    }

    void ChangeTextViewBack(Android.Graphics.Color accent)
    {
        var colorlist = new ColorStateList(new int[][]
        {
                new int[]{global::Android.Resource.Attribute.StateFocused},
                new int[]{-global::Android.Resource.Attribute.StateFocused},
        },
            new int[] {
                    Android.Graphics.Color.Argb(255,accent.R,accent.G,accent.B),
                    Android.Graphics.Color.Argb(255, 200, 200, 200)
        });
        _EditText.Background.SetTintList(colorlist);
    }


    bool TextView.IOnEditorActionListener.OnEditorAction(TextView v, ImeAction actionId, Android.Views.KeyEvent e)
    {
        if (actionId == ImeAction.Done ||
                (actionId == ImeAction.ImeNull && e.KeyCode == Keycode.Enter))
        {
            HideKeyboard(v);
            DoneEdit();
        }

        return true;
    }

    void DoneEdit()
    {
        var entryCell = (IEntryCellController)Cell;
        //entryCell.SendCompleted();
        _EditText.ClearFocus();
        ClearFocus();
    }

    void HideKeyboard(Android.Views.View inputView)
    {
        using (var inputMethodManager = (InputMethodManager)_Context.GetSystemService(Context.InputMethodService))
        {
            IBinder windowToken = inputView.WindowToken;
            if (windowToken != null)
                inputMethodManager.HideSoftInputFromWindow(windowToken, HideSoftInputFlags.None);
        }
    }
    void ShowKeyboard(Android.Views.View inputView)
    {
        using (var inputMethodManager = (InputMethodManager)_Context.GetSystemService(Context.InputMethodService))
        {

            inputMethodManager.ShowSoftInput(inputView, ShowFlags.Forced);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);

        }
    }

    void ITextWatcher.AfterTextChanged(IEditable s)
    {
    }

    void ITextWatcher.BeforeTextChanged(ICharSequence s, int start, int count, int after)
    {
    }

    void ITextWatcher.OnTextChanged(ICharSequence s, int start, int before, int count)
    {
        if (string.IsNullOrEmpty(_EntryCell.ValueText) && s.Length() == 0)
        {
            return;
        }

        _EntryCell.ValueText = s?.ToString();
    }

    void IOnFocusChangeListener.OnFocusChange(Android.Views.View v, bool hasFocus)
    {
        if (hasFocus)
        {
            //show underline when on focus.
            _EditText.Background.Alpha = 100;
        }
        else
        {
            //hide underline
            _EditText.Background.Alpha = 0;
            // consider as text inpute completed.
            _EntryCell.SendCompleted();
        }
    }

    void EntryCell_Focused(object sender, EventArgs e)
    {
        _EditText.RequestFocus();
        ShowKeyboard(_EditText);
    }

}

internal class AiEditText : EditText
{
    public Action ClearFocusAction { get; set; }
    SoftInput _startingMode;

    public AiEditText(Context context) : base(context)
    {
    }

    protected override void OnFocusChanged(bool gainFocus, [GeneratedEnum] FocusSearchDirection direction, Android.Graphics.Rect previouslyFocusedRect)
    {
        base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);
        if (gainFocus)
        {
            Post(new Runnable(() =>
            {
                SetSelection(Text.Length);
            }));
        }
    }

    public override bool OnKeyPreIme(Keycode keyCode, KeyEvent e)
    {
        if (keyCode == Keycode.Back && e.Action == KeyEventActions.Up)
        {
            ClearFocus();
            ClearFocusAction?.Invoke();
        }
        return base.OnKeyPreIme(keyCode, e);

    }
}
