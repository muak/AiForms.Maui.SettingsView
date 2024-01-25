using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using ARelativeLayout = Android.Widget.RelativeLayout;
using Resource = global::AiForms.Settings.Resource;
using AiForms.Settings.Extensions;

namespace AiForms.Settings.Platforms.Droid;

/// <summary>
/// Cell base view.
/// </summary>
[Android.Runtime.Preserve(AllMembers = true)]
public class CellBaseView : ARelativeLayout, IImageSourcePartSetter
{
    CellBase _cell;
    public CellBase Cell
    {
        get { return _cell; }
        set
        {
            if (_cell == value)
                return;

            if (_cell != null)
            {
                _cell.PropertyChanged -= CellPropertyChanged;
                _imageLoader?.Reset();
                _imageLoader = null;
            }
            _cell = value;

            if (_cell != null)
            {
                _cell.PropertyChanged += CellPropertyChanged;
            }
        }
    }
    /// <summary>
    /// Gets the cell parent.
    /// </summary>
    /// <value>The cell parent.</value>
    public SettingsView CellParent => Cell.Parent as SettingsView;

    /// <summary>
    /// Gets or sets the title label.
    /// </summary>
    /// <value>The title label.</value>
    public TextView TitleLabel { get; set; }
    /// <summary>
    /// Gets or sets the description label.
    /// </summary>
    /// <value>The description label.</value>
    public TextView DescriptionLabel { get; set; }
    /// <summary>
    /// Gets or sets the icon view.
    /// </summary>
    /// <value>The icon view.</value>
    public ImageView IconView { get; set; }
    /// <summary>
    /// Gets or sets the content stack.
    /// </summary>
    /// <value>The content stack.</value>
    public LinearLayout ContentStack { get; set; }
    /// <summary>
    /// Gets or sets the accessory stack.
    /// </summary>
    /// <value>The accessory stack.</value>
    public LinearLayout AccessoryStack { get; set; }
    /// <summary>
    /// Gets the hint label.
    /// </summary>
    /// <value>The hint label.</value>
    public TextView HintLabel { get; private set; }

    IElementHandler IImageSourcePartSetter.Handler => Cell.Handler;

    IImageSourcePart IImageSourcePartSetter.ImageSourcePart => Cell;

    protected Lazy<IFontManager> _fontManager;
    ImageSourcePartLoader _imageLoader;
    /// <summary>
    /// The context.
    /// </summary>
    protected Context _Context;
    CancellationTokenSource _iconTokenSource;
    Android.Graphics.Color _defaultTextColor;
    ColorDrawable _backgroundColor;
    ColorDrawable _selectedColor;
    RippleDrawable _ripple;
    float _defaultFontSize;
    float _iconRadius;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.CellBaseView"/> class.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="cell">Cell.</param>
    public CellBaseView(Context context, CellBase cell) : base(context)
    {
        _Context = context;
        Cell = cell;

        CreateContentView();

        _fontManager = new Lazy<IFontManager>(() =>
        {
            return cell.FindMauiContext().Services.GetService<IFontManager>();
        });
    }

    public CellBaseView(IntPtr javaReference,JniHandleOwnership transfer) : base(javaReference, transfer) { }

    protected virtual void CreateContentView()
    {
        var layoutInflater = (LayoutInflater)_Context.GetSystemService(Context.LayoutInflaterService);
        var contentView = layoutInflater.Inflate(Resource.Layout.cellbaseview, this, true);

        contentView.LayoutParameters = new ViewGroup.LayoutParams(-1, -1);

        IconView = contentView.FindViewById<ImageView>(Resource.Id.CellIcon);
        TitleLabel = contentView.FindViewById<TextView>(Resource.Id.CellTitle);
        DescriptionLabel = contentView.FindViewById<TextView>(Resource.Id.CellDescription);
        ContentStack = contentView.FindViewById<LinearLayout>(Resource.Id.CellContentStack);
        AccessoryStack = contentView.FindViewById<LinearLayout>(Resource.Id.CellAccessoryView);
        HintLabel = contentView.FindViewById<TextView>(Resource.Id.CellHintText);

        _backgroundColor = new ColorDrawable();
        _selectedColor = new ColorDrawable(Android.Graphics.Color.Argb(125, 180, 180, 180));

        var sel = new StateListDrawable();

        sel.AddState(new int[] { global::Android.Resource.Attribute.StateSelected }, _selectedColor);
        sel.AddState(new int[] { -global::Android.Resource.Attribute.StateSelected }, _backgroundColor);
        sel.SetExitFadeDuration(250);
        sel.SetEnterFadeDuration(250);

        var rippleColor = Android.Graphics.Color.Rgb(180, 180, 180);
        if (CellParent.SelectedColor.IsNotDefault())
        {
            rippleColor = CellParent.SelectedColor.ToPlatform();
        }

        _ripple = DrawableUtility.CreateRipple(rippleColor,sel);

        Background = _ripple;

        _defaultTextColor = new Android.Graphics.Color(TitleLabel.CurrentTextColor);
        _defaultFontSize = TitleLabel.TextSize;
        
        if (!String.IsNullOrEmpty(Cell.AutomationId)) 
        {
            contentView.ContentDescription = Cell.AutomationId;
        }

    }

    /// <summary>
    /// Cells the property changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    public virtual void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
    {        
    }

    /// <summary>
    /// Parents the property changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    public virtual void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        // avoid running the vain process when popping a page.
        if((sender as BindableObject)?.BindingContext == null){
            return;
        }

        if (e.PropertyName == SettingsView.CellTitleColorProperty.PropertyName)
        {
            UpdateTitleColor();
        }
        else if (e.PropertyName == SettingsView.CellTitleFontSizeProperty.PropertyName)
        {
            UpdateWithForceLayout(UpdateTitleFontSize);
        }
        else if (e.PropertyName == SettingsView.CellTitleFontFamilyProperty.PropertyName ||
                 e.PropertyName == SettingsView.CellTitleFontAttributesProperty.PropertyName)
        {
            UpdateWithForceLayout(UpdateTitleFont);
        }
        else if (e.PropertyName == SettingsView.CellDescriptionColorProperty.PropertyName)
        {
            UpdateDescriptionColor();
        }
        else if (e.PropertyName == SettingsView.CellDescriptionFontSizeProperty.PropertyName)
        {
            UpdateWithForceLayout(UpdateDescriptionFontSize);
        }
        else if (e.PropertyName == SettingsView.CellDescriptionFontFamilyProperty.PropertyName ||
                 e.PropertyName == SettingsView.CellDescriptionFontAttributesProperty.PropertyName)
        {
            UpdateWithForceLayout(UpdateDescriptionFont);
        }
        else if (e.PropertyName == SettingsView.CellBackgroundColorProperty.PropertyName)
        {
            UpdateBackgroundColor();
        }
        else if (e.PropertyName == SettingsView.CellHintTextColorProperty.PropertyName)
        {
            UpdateHintTextColor();
        }
        else if (e.PropertyName == SettingsView.CellHintFontSizeProperty.PropertyName)
        {
            UpdateWithForceLayout(UpdateHintFontSize);
        }
        else if (e.PropertyName == SettingsView.CellHintFontFamilyProperty.PropertyName ||
                 e.PropertyName == SettingsView.CellHintFontAttributesProperty.PropertyName)
        {
            UpdateWithForceLayout(UpdateHintFont);
        }
        else if (e.PropertyName == SettingsView.CellIconSizeProperty.PropertyName)
        {
            UpdateIcon();
        }
        else if (e.PropertyName == SettingsView.CellIconRadiusProperty.PropertyName)
        {
            UpdateIconRadius();
            UpdateIcon(true);
        }
        else if (e.PropertyName == SettingsView.SelectedColorProperty.PropertyName)
        {
            UpdateWithForceLayout(UpdateSelectedColor);
        }

    }

    /// <summary>
    /// Sections the property changed.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    public virtual void SectionPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
    }

    /// <summary>
    /// Rows the selected.
    /// </summary>
    /// <param name="adapter">Adapter.</param>
    /// <param name="position">Position.</param>
    public virtual void RowSelected(SettingsViewRecyclerAdapter adapter, int position)
    {
    }

    /// <summary>
    /// Rows the long pressed.
    /// </summary>
    /// <returns><c>true</c>, if long pressed was rowed, <c>false</c> otherwise.</returns>
    /// <param name="adapter">Adapter.</param>
    /// <param name="position">Position.</param>
    public virtual bool RowLongPressed(SettingsViewRecyclerAdapter adapter, int position)
    {
        return false;
    }

    /// <summary>
    /// Updates the with force layout.
    /// </summary>
    /// <param name="updateAction">Update action.</param>
    internal void UpdateWithForceLayout(System.Action updateAction)
    {
        updateAction();
        Invalidate();
    }

    internal void UpdateLayout()
    {
        Invalidate();
    }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public virtual void UpdateCell()
    {
        UpdateBackgroundColor();
        UpdateSelectedColor();
        UpdateTitleText();
        UpdateTitleColor();
        UpdateTitleFontSize();
        UpdateTitleFont();
        UpdateDescriptionText();
        UpdateDescriptionColor();
        UpdateDescriptionFontSize();
        UpdateDescriptionFont();
        UpdateHintText();
        UpdateHintTextColor();
        UpdateHintFontSize();
        UpdateHintFont();

        UpdateIcon();
        UpdateIconRadius();

        UpdateIsEnabled();

        Invalidate();
    }

    internal void UpdateBackgroundColor()
    {
        Selected = false;

        if (Cell.BackgroundColor.IsNotDefault()) {
            _backgroundColor.Color = Cell.BackgroundColor.ToPlatform();
        }
        else if (CellParent != null && CellParent.CellBackgroundColor.IsNotDefault()) {
            _backgroundColor.Color = CellParent.CellBackgroundColor.ToPlatform();
        }
        else {
            _backgroundColor.Color = Android.Graphics.Color.Transparent;
        }
    }

    internal void UpdateSelectedColor()
    {
        if (CellParent != null && CellParent.SelectedColor.IsNotDefault()) {
            _selectedColor.Color = CellParent.SelectedColor.MultiplyAlpha(0.5f).ToPlatform();
            _ripple.SetColor(DrawableUtility.GetPressedColorSelector(CellParent.SelectedColor.ToPlatform()));
        }
        else {
            _selectedColor.Color = Android.Graphics.Color.Argb(125, 180, 180, 180);
            _ripple.SetColor(DrawableUtility.GetPressedColorSelector(Android.Graphics.Color.Rgb(180, 180, 180)));
        }
    }

    internal void UpdateTitleText()
    {
        TitleLabel.Text = Cell.Title;
        //hide TextView right padding when TextView.Text empty.
        TitleLabel.Visibility = string.IsNullOrEmpty(TitleLabel.Text) ? ViewStates.Gone : ViewStates.Visible;
    }

    internal void UpdateTitleColor()
    {
        if (Cell.TitleColor.IsNotDefault()) {
            TitleLabel.SetTextColor(Cell.TitleColor.ToPlatform());
        }
        else if (CellParent != null && CellParent.CellTitleColor.IsNotDefault()) {
            TitleLabel.SetTextColor(CellParent.CellTitleColor.ToPlatform());
        }
        else {
            TitleLabel.SetTextColor(_defaultTextColor);
        }
    }

    internal void UpdateTitleFontSize()
    {
        if (Cell.TitleFontSize > 0)
        {
            TitleLabel.SetTextSize(ComplexUnitType.Sp, (float)Cell.TitleFontSize);
        }
        else if (CellParent != null)
        {
            TitleLabel.SetTextSize(ComplexUnitType.Sp, (float)CellParent.CellTitleFontSize);
        }
        else
        {
            TitleLabel.SetTextSize(ComplexUnitType.Sp, _defaultFontSize);
        }
    }

    internal void UpdateTitleFont()
    {
        var family = Cell.TitleFontFamily ?? CellParent?.CellTitleFontFamily;
        var attr = Cell.TitleFontAttributes ?? CellParent.CellTitleFontAttributes;
        var size = Cell.TitleFontSize > 0 ? Cell.TitleFontSize : CellParent.CellTitleFontSize;
              
        TitleLabel.Typeface = family.ToFont(size, attr).ToTypeface(_fontManager.Value); 
    }

    internal void UpdateDescriptionText()
    {
        DescriptionLabel.Text = Cell.Description;
        DescriptionLabel.Visibility = string.IsNullOrEmpty(DescriptionLabel.Text) ?
            ViewStates.Gone : ViewStates.Visible;
    }

    internal void UpdateDescriptionFontSize()
    {
        if (Cell.DescriptionFontSize > 0) {
            DescriptionLabel.SetTextSize(ComplexUnitType.Sp, (float)Cell.DescriptionFontSize);
        }
        else if (CellParent != null) {
            DescriptionLabel.SetTextSize(ComplexUnitType.Sp, (float)CellParent.CellDescriptionFontSize);
        }
        else {
            DescriptionLabel.SetTextSize(ComplexUnitType.Sp, _defaultFontSize);
        }
    }

    internal void UpdateDescriptionFont()
    {
        var family = Cell.DescriptionFontFamily ?? CellParent?.CellDescriptionFontFamily;
        var attr = Cell.DescriptionFontAttributes ?? CellParent.CellDescriptionFontAttributes;
        var size = Cell.DescriptionFontSize > 0 ? Cell.DescriptionFontSize : CellParent.CellDescriptionFontSize;

        DescriptionLabel.Typeface = family.ToFont(size, attr).ToTypeface(_fontManager.Value);
    }

    internal void UpdateDescriptionColor()
    {
        if (Cell.DescriptionColor.IsNotDefault()) {
            DescriptionLabel.SetTextColor(Cell.DescriptionColor.ToPlatform());
        }
        else if (CellParent != null && CellParent.CellDescriptionColor.IsNotDefault()) {
            DescriptionLabel.SetTextColor(CellParent.CellDescriptionColor.ToPlatform());
        }
        else {
            DescriptionLabel.SetTextColor(_defaultTextColor);
        }
    }

    internal void UpdateHintText()
    {
        var msg = Cell.HintText;
        if (string.IsNullOrEmpty(msg)) {
            HintLabel.Visibility = ViewStates.Gone;
            return;
        }

        HintLabel.Text = msg;
        HintLabel.Visibility = ViewStates.Visible;
    }

    internal void UpdateHintTextColor()
    {
        if (Cell.HintTextColor.IsNotDefault()) {
            HintLabel.SetTextColor(Cell.HintTextColor.ToPlatform());
        }
        else if (CellParent != null && CellParent.CellHintTextColor.IsNotDefault()) {
            HintLabel.SetTextColor(CellParent.CellHintTextColor.ToPlatform());
        }
        else {
            HintLabel.SetTextColor(_defaultTextColor);
        }
    }

    internal void UpdateHintFontSize()
    {
        if (Cell.HintFontSize > 0) {
            HintLabel.SetTextSize(ComplexUnitType.Sp, (float)Cell.HintFontSize);
        }
        else if (CellParent != null) {
            HintLabel.SetTextSize(ComplexUnitType.Sp, (float)CellParent.CellHintFontSize);
        }
        else {
            HintLabel.SetTextSize(ComplexUnitType.Sp, _defaultFontSize);
        }
    }

    internal void UpdateHintFont()
    {
        var family = Cell.HintFontFamily ?? CellParent?.CellHintFontFamily;
        var attr = Cell.HintFontAttributes ?? CellParent.CellHintFontAttributes;
        var size = Cell.HintFontSize > 0 ? Cell.HintFontSize : CellParent.CellHintFontSize;

        HintLabel.Typeface = family.ToFont(size, attr).ToTypeface(_fontManager.Value);
    }

    /// <summary>
    /// Updates the is enabled.
    /// </summary>
    internal virtual void UpdateIsEnabled()
    {
        SetEnabledAppearance(Cell.IsEnabled);
    }

    internal virtual void UpdateIsVisible()
    {

    }

    /// <summary>
    /// Sets the enabled appearance.
    /// </summary>
    /// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
    internal virtual void SetEnabledAppearance(bool isEnabled)
    {
        if (isEnabled) {
            Focusable = false;
            DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;
            TitleLabel.Alpha = 1f;
            DescriptionLabel.Alpha = 1f;
            IconView.Alpha = 1f;
        }
        else {
            // not to invoke a ripple effect and not to selected
            Focusable = true;
            DescendantFocusability = Android.Views.DescendantFocusability.BlockDescendants;
            // to turn like disabled
            TitleLabel.Alpha = 0.3f;
            DescriptionLabel.Alpha = 0.3f;
            IconView.Alpha = 0.3f;
        }
    }

    internal void UpdateIconRadius()
    {
        if (Cell.IconRadius >= 0) {
            _iconRadius = _Context.ToPixels(Cell.IconRadius);
        }
        else if (CellParent != null) {
            _iconRadius = _Context.ToPixels(CellParent.CellIconRadius);
        }
    }

    internal void UpdateIconSize()
    {
        Microsoft.Maui.Graphics.Size size;
        if (Cell.IconSize != default(Microsoft.Maui.Graphics.Size)) {
            size = Cell.IconSize;
        }
        else if (CellParent != null && CellParent.CellIconSize != default(Microsoft.Maui.Graphics.Size)) {
            size = CellParent.CellIconSize;
        }
        else {
            size = new Microsoft.Maui.Graphics.Size(36, 36);
        }

        IconView.LayoutParameters.Width = (int)_Context.ToPixels(size.Width);
        IconView.LayoutParameters.Height = (int)_Context.ToPixels(size.Height);
    }

    internal void UpdateIcon(bool forceLoad = false)
    {
        UpdateIconSize();

        _imageLoader?.Reset();

        if (IconView.Drawable != null)
        {
            IconView.SetImageDrawable(null);
            IconView.SetImageBitmap(null);
        }

        if (Cell.IconSource != null)
        {
            IconView.Visibility = ViewStates.Visible;

            _imageLoader = new ImageSourcePartLoader(this);
            _imageLoader?.UpdateImageSourceAsync();
        }
        else
        {
            IconView.Visibility = ViewStates.Gone;
        }
    }

    void IImageSourcePartSetter.SetImageSource(Drawable platformImage)
    {
        IconView?.SetImageDrawable(platformImage);
    }

    Bitmap CreateRoundImage(Bitmap image)
    {
        var clipArea = Bitmap.CreateBitmap(image.Width, image.Height, Bitmap.Config.Argb8888);
        var canvas = new Canvas(clipArea);
        var paint = new Android.Graphics.Paint(PaintFlags.AntiAlias);
        canvas.DrawARGB(0, 0, 0, 0);
        canvas.DrawRoundRect(new Android.Graphics.RectF(0, 0, image.Width, image.Height), _iconRadius, _iconRadius, paint);


        paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));

        var rect = new Android.Graphics.Rect(0, 0, image.Width, image.Height);
        canvas.DrawBitmap(image, rect, rect, paint);

        image.Recycle();
        image.Dispose();
        image = null;
        canvas.Dispose();
        canvas = null;
        paint.Dispose();
        paint = null;
        rect.Dispose();
        rect = null;

        return clipArea;
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
            _imageLoader?.Reset();
            _imageLoader = null;

            if(_cell != null)
            {
                _cell.PropertyChanged -= CellPropertyChanged;
                if (_cell.Section != null)
                {
                    _cell.Section.PropertyChanged -= SectionPropertyChanged;
                    _cell.Section = null;
                }

                //CellRenderer.SetRealCell(_cell, null);
                _cell = null;
            }
            
            CellParent.PropertyChanged -= ParentPropertyChanged;
                       

            HintLabel?.Dispose();
            HintLabel = null;
            TitleLabel?.Dispose();
            TitleLabel = null;
            DescriptionLabel?.Dispose();
            DescriptionLabel = null;
            IconView?.SetImageDrawable(null);
            IconView?.SetImageBitmap(null);
            IconView?.Dispose();
            IconView = null;
            ContentStack?.Dispose();
            ContentStack = null;
            AccessoryStack?.Dispose();
            AccessoryStack = null;

            _iconTokenSource?.Dispose();
            _iconTokenSource = null;
            _Context = null;

            _backgroundColor?.Dispose();
            _backgroundColor = null;
            _selectedColor?.Dispose();
            _selectedColor = null;
            _ripple?.Dispose();
            _ripple = null;

            Background?.Dispose();
            Background = null;
        }
        base.Dispose(disposing);
    }    
}

