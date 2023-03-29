using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Foundation;
using CoreGraphics;
using CoreFoundation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Platform;
using System.Runtime.InteropServices;
using Microsoft.Maui.Controls.Compatibility;
using AiForms.Settings.Handlers;
using System.Security.Cryptography;
using AiForms.Settings.Extensions;
using Microsoft.Maui.Controls.Platform;

namespace AiForms.Settings.Platforms.iOS;

/// <summary>
/// Cell base view.
/// </summary>
[Foundation.Preserve(AllMembers = true)]
public class CellBaseView : UITableViewCell
{
    /// <summary>
    /// Gets the hint label.
    /// </summary>
    /// <value>The hint label.</value>
    public UILabel HintLabel { get; private set; }
    /// <summary>
    /// Gets the title label.
    /// </summary>
    /// <value>The title label.</value>
    public UILabel TitleLabel { get; private set; }
    /// <summary>
    /// Gets the description label.
    /// </summary>
    /// <value>The description label.</value>
    public UILabel DescriptionLabel { get; private set; }
    /// <summary>
    /// Gets the icon view.
    /// </summary>
    /// <value>The icon view.</value>
    public UIImageView IconView { get; private set; }

    /// <summary>
    /// Gets the cell parent.
    /// </summary>
    /// <value>The cell parent.</value>
    public SettingsView CellParent => Cell.Parent as SettingsView;

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

    protected Lazy<IFontManager> _fontManager;
    ImageSourcePartLoader _imageLoader;

    /// <summary>
    /// Gets the content stack.
    /// </summary>
    /// <value>The content stack.</value>
    protected UIStackView ContentStack { get; private set; }

    protected UIStackView StackH;
    protected UIStackView StackV;

    
    Size _iconSize;
    NSLayoutConstraint _iconConstraintHeight;
    NSLayoutConstraint _iconConstraintWidth;
    NSLayoutConstraint _minheightConstraint;
    CancellationTokenSource _iconTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.CellBaseView"/> class.
    /// </summary>
    /// <param name="virtualCell">Forms cell.</param>
    public CellBaseView(CellBase virtualCell) : base(UIKit.UITableViewCellStyle.Default, virtualCell.GetType().FullName)
    {
        Cell = virtualCell;

        SelectionStyle = UITableViewCellSelectionStyle.None;
        SetUpHintLabel();
        SetUpContentView();

        UpdateSelectedColor();

        _fontManager = new Lazy<IFontManager>(() =>
        {
            return virtualCell.FindMauiContext().Services.GetService<IFontManager>();
        });
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
        if (e.PropertyName == SettingsView.CellTitleColorProperty.PropertyName)
        {
            UpdateTitleColor();
        }
        else if (e.PropertyName == SettingsView.CellTitleFontSizeProperty.PropertyName ||
                 e.PropertyName == SettingsView.CellTitleFontFamilyProperty.PropertyName ||
                 e.PropertyName == SettingsView.CellTitleFontAttributesProperty.PropertyName)
        {
            UpdateWithForceLayout(UpdateTitleFont);
        }
        else if (e.PropertyName == SettingsView.CellDescriptionColorProperty.PropertyName)
        {
            UpdateDescriptionColor();
        }
        else if (e.PropertyName == SettingsView.CellDescriptionFontSizeProperty.PropertyName ||
                 e.PropertyName == SettingsView.CellDescriptionFontFamilyProperty.PropertyName ||
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
        else if (e.PropertyName == SettingsView.CellHintFontSizeProperty.PropertyName ||
                 e.PropertyName == SettingsView.CellHintFontFamilyProperty.PropertyName ||
                 e.PropertyName == SettingsView.CellHintFontAttributesProperty.PropertyName)
        {
            UpdateWithForceLayout(UpdateHintFont);
        }
        else if (e.PropertyName == SettingsView.CellIconSizeProperty.PropertyName)
        {
            UpdateWithForceLayout(UpdateIconSize);
        }
        else if (e.PropertyName == SettingsView.CellIconRadiusProperty.PropertyName)
        {
            UpdateWithForceLayout(UpdateIconRadius);
        }
        else if (e.PropertyName == SettingsView.SelectedColorProperty.PropertyName)
        {
            UpdateSelectedColor();
        }
        else if (e.PropertyName == TableView.RowHeightProperty.PropertyName)
        {
            UpdateMinRowHeight();
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
    /// <param name="tableView">Table view.</param>
    /// <param name="indexPath">Index path.</param>
    public virtual void RowSelected(UITableView tableView,NSIndexPath indexPath)
    {
    }

    public virtual bool RowLongPressed(UITableView tableView,NSIndexPath indexPath)
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
        SetNeedsLayout();
    }

    internal void UpdateLayout()
    {
        SetNeedsLayout();
    }

    internal void UpdateSelectedColor()
    {
        if (CellParent != null && CellParent.SelectedColor.IsNotDefault()) {
            if (SelectedBackgroundView != null) {
                SelectedBackgroundView.BackgroundColor = CellParent.SelectedColor.ToPlatform();
            }
            else {
                SelectedBackgroundView = new UIView { BackgroundColor = CellParent.SelectedColor.ToPlatform() };
            }
        }
    }

    internal void UpdateBackgroundColor()
    {
        if (Cell.BackgroundColor.IsNotDefault()) {
            BackgroundColor = Cell.BackgroundColor.ToPlatform();
        }
        else if (CellParent != null && CellParent.CellBackgroundColor.IsNotDefault()) {
            BackgroundColor = CellParent.CellBackgroundColor.ToPlatform();
        }
    }

    internal void UpdateHintText()
    {
        if (HintLabel is null)
            return; // for HotReload

        HintLabel.Text = Cell.HintText;
        HintLabel.Hidden = string.IsNullOrEmpty(Cell.HintText);
    }

    internal void UpdateHintTextColor()
    {
        if (HintLabel is null)
            return; // for HotReload

        if (Cell.HintTextColor.IsNotDefault())
        {
            HintLabel.TextColor = Cell.HintTextColor.ToPlatform();
        }
        else if (CellParent != null && CellParent.CellHintTextColor.IsNotDefault())
        {
            HintLabel.TextColor = CellParent.CellHintTextColor.ToPlatform();
        }
    }

    internal void UpdateHintFont()
    {
        if (HintLabel is null)
            return; // for HotReload

        var family = Cell.HintFontFamily ?? CellParent.CellHintFontFamily;
        var attr = Cell.HintFontAttributes ?? CellParent.CellHintFontAttributes;

        if (Cell.HintFontSize > 0)
        {
            HintLabel.Font = family.ToFont(Cell.HintFontSize, attr).ToUIFont(_fontManager.Value);
        }
        else if (CellParent != null)
        {
            HintLabel.Font = family.ToFont(CellParent.CellHintFontSize, attr).ToUIFont(_fontManager.Value);
        }
    }

    internal void UpdateTitleText()
    {
        if (TitleLabel is null)
            return; // for HotReload

        TitleLabel.Text = Cell.Title;
        //Since Layout breaks when text empty, prevent Label height from resizing 0.
        if (string.IsNullOrEmpty(TitleLabel.Text))
        {
            TitleLabel.Hidden = true;
            TitleLabel.Text = " ";
        }
        else
        {
            TitleLabel.Hidden = false;
        }

    }

    internal void UpdateTitleColor()
    {
        if (TitleLabel is null)
            return; // for HotReload

        if (Cell.TitleColor.IsNotDefault())
        {
            TitleLabel.TextColor = Cell.TitleColor.ToPlatform();
        }
        else if (CellParent != null && CellParent.CellTitleColor.IsNotDefault())
        {
            TitleLabel.TextColor = CellParent.CellTitleColor.ToPlatform();
        }
    }

    internal void UpdateTitleFont()
    {
        if (TitleLabel is null)
            return; // for HotReload

        var family = Cell.TitleFontFamily ?? CellParent.CellTitleFontFamily;
        var attr = Cell.TitleFontAttributes ?? CellParent.CellTitleFontAttributes;

        if (Cell.TitleFontSize > 0)
        {
            TitleLabel.Font = family.ToFont(Cell.TitleFontSize, attr).ToUIFont(_fontManager.Value);
        }
        else if (CellParent != null)
        {
            TitleLabel.Font = family.ToFont(CellParent.CellTitleFontSize, attr).ToUIFont(_fontManager.Value); 
        }
    }

    internal void UpdateDescriptionText()
    {
        if (DescriptionLabel is null)
            return; // for HotReload

        DescriptionLabel.Text = Cell.Description;
        //layout break because of StackView spacing.DescriptionLabel hidden to fix it. 
        DescriptionLabel.Hidden = string.IsNullOrEmpty(DescriptionLabel.Text);

        if (CellParent.HasUnevenRows)
        {
            if (CellParent.Handler.PlatformView is AiTableView tv)
            {                
                tv.BeginUpdates();
                tv.EndUpdates();                
            }
        }       
    }

    internal void UpdateDescriptionFont()
    {
        if (DescriptionLabel is null)
            return; // for HotReload

        var family = Cell.DescriptionFontFamily ?? CellParent.CellDescriptionFontFamily;
        var attr = Cell.DescriptionFontAttributes ?? CellParent.CellDescriptionFontAttributes;

        if (Cell.DescriptionFontSize > 0)
        {
            DescriptionLabel.Font = family.ToFont(Cell.DescriptionFontSize, attr).ToUIFont(_fontManager.Value); 
        }
        else if (CellParent != null)
        {
            DescriptionLabel.Font = family.ToFont(CellParent.CellDescriptionFontSize, attr).ToUIFont(_fontManager.Value);
        }
    }

    internal void UpdateDescriptionColor()
    {
        if (DescriptionLabel is null)
            return; // for HotReload

        if (Cell.DescriptionColor.IsNotDefault()) {
            DescriptionLabel.TextColor = Cell.DescriptionColor.ToPlatform();
        }
        else if (CellParent != null && CellParent.CellDescriptionColor.IsNotDefault()) {
            DescriptionLabel.TextColor = CellParent.CellDescriptionColor.ToPlatform();
        }
    }

    /// <summary>
    /// Updates the is enabled.
    /// </summary>
    internal virtual void UpdateIsEnabled()
    {
        SetEnabledAppearance(Cell.IsEnabled);
    }

    /// <summary>
    /// Updates the IsVisible
    /// </summary>
    internal virtual void UpdateIsVisible()
    {
        // If AccessoryView is set, hide the view because it overflows outside when IsVisible is false. 
        if (AccessoryView != null)
        {
            AccessoryView.Hidden = !Cell.IsVisible;
        }
    }

    /// <summary>
    /// Sets the enabled appearance.
    /// </summary>
    /// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
    internal virtual void SetEnabledAppearance(bool isEnabled)
    {
        if (TitleLabel is null)
            return; // for HotReload

        if (isEnabled)
        {
            UserInteractionEnabled = true;
            TitleLabel.Alpha = 1f;
            DescriptionLabel.Alpha = 1f;
            IconView.Alpha = 1f;
        }
        else
        {
            UserInteractionEnabled = false;
            TitleLabel.Alpha = 0.3f;
            DescriptionLabel.Alpha = 0.3f;
            IconView.Alpha = 0.3f;
        }
    }

    internal void UpdateIconSize()
    {
        Size size;
        if (Cell.IconSize != default(Size)) {
            size = Cell.IconSize;
        }
        else if (CellParent != null && CellParent.CellIconSize != default(Size)) {
            size = CellParent.CellIconSize;
        }
        else {
            size = new Size(32, 32);
        }

        //do nothing when current size is previous size
        if (size == _iconSize) {
            return;
        }

        if (_iconSize != default(Size)) {
            //remove previous constraint
            _iconConstraintHeight.Active = false;
            _iconConstraintWidth.Active = false;
            _iconConstraintHeight?.Dispose();
            _iconConstraintWidth?.Dispose();
        }

        _iconConstraintHeight = IconView.HeightAnchor.ConstraintEqualTo((NFloat)size.Height);
        _iconConstraintWidth = IconView.WidthAnchor.ConstraintEqualTo((NFloat)size.Width);

        _iconConstraintHeight.Priority = 999f; // fix warning-log:Unable to simultaneously satisfy constraints.
        _iconConstraintHeight.Active = true;
        _iconConstraintWidth.Active = true;

        IconView.UpdateConstraints();

        _iconSize = size;
    }

    internal void UpdateIconRadius()
    {
        if (IconView is null)
            return; // for HotReload

        if (Cell.IconRadius >= 0) {
            IconView.Layer.CornerRadius = (float)Cell.IconRadius;
        }
        else if (CellParent != null) {
            IconView.Layer.CornerRadius = (float)CellParent.CellIconRadius;
        }
    }

    internal void UpdateIcon()
    {        
        if (IconView is null)
            return; // for HotReload

        _imageLoader?.Reset();
       
        UpdateIconSize();

        if (IconView.Image != null)
        {           
            IconView.Image = null;
        }

        if (Cell.IconSource != null)
        {
            //image未設定の時はhiddenにしないとstackviewのDistributionが機能しなくなる
            //hide IconView because UIStackView Distribution won't work when a image isn't set.
            IconView.Hidden = false;

        _imageLoader = new ImageSourcePartLoader(Cell.Handler, () => Cell, OnSetImageSource);
        _imageLoader.UpdateImageSourceAsync();                 
    }
        else
        {
            IconView.Hidden = true;
        }
    }

    private void OnSetImageSource(UIImage image)
    {
        IconView.Image = image;
        SetNeedsLayout();
    }


    internal void UpdateMinRowHeight()
    {
        if (_minheightConstraint != null) {
            _minheightConstraint.Active = false;
            _minheightConstraint.Dispose();
            _minheightConstraint = null;
        }

        if (CellParent.HasUnevenRows) {
            _minheightConstraint = StackH.HeightAnchor.ConstraintGreaterThanOrEqualTo(CellParent.RowHeight);
            _minheightConstraint.Priority = 999f;
            _minheightConstraint.Active = true;

        }

        StackH.UpdateConstraints();
    }

    /// <summary>
    /// Updates the cell.
    /// </summary>
    public virtual void UpdateCell(UITableView tableView = null)
    {
        if (TitleLabel is null)
            return; // For HotReload

        //UpdateBackgroundColor();
        //UpdateTitleText();
        //UpdateTitleColor();
        //UpdateTitleFont();
        //UpdateDescriptionText();
        //UpdateDescriptionColor();
        //UpdateDescriptionFont();
        //UpdateHintText();
        //UpdateHintTextColor();
        //UpdateHintFont();

        ////UpdateIcon();
        //UpdateIconRadius();

        //UpdateIsEnabled();
        //UpdateIsVisible();

        //SetNeedsLayout();
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

            


            SelectedBackgroundView?.Dispose();
            SelectedBackgroundView = null;

            BeginInvokeOnMainThread(() =>
            {
                HintLabel.RemoveFromSuperview();
                HintLabel.Dispose();
                HintLabel = null;
                TitleLabel?.Dispose();
                TitleLabel = null;
                DescriptionLabel?.Dispose();
                DescriptionLabel = null;
                IconView.RemoveFromSuperview();
                IconView.Image?.Dispose();
                IconView.Dispose();
                IconView = null;
                _iconTokenSource?.Dispose();
                _iconTokenSource = null;
                _iconConstraintWidth?.Dispose();
                _iconConstraintHeight?.Dispose();
                _iconConstraintHeight = null;
                _iconConstraintWidth = null;
                ContentStack?.RemoveFromSuperview();
                ContentStack?.Dispose();
                ContentStack = null;

                StackV?.RemoveFromSuperview();
                StackV?.Dispose();
                StackV = null;

                StackH.RemoveFromSuperview();
                StackH.Dispose();
                StackH = null;

            });
        }

        base.Dispose(disposing);
    }

    void SetUpHintLabel()
    {
        HintLabel = new UILabel();
        HintLabel.LineBreakMode = UILineBreakMode.Clip;
        HintLabel.Lines = 0;
        HintLabel.TintAdjustmentMode = UIViewTintAdjustmentMode.Automatic;
        HintLabel.AdjustsFontSizeToFitWidth = true;
        HintLabel.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
        HintLabel.TextAlignment = UITextAlignment.Right;
        HintLabel.AdjustsLetterSpacingToFitWidth = true;

        this.AddSubview(HintLabel);

        HintLabel.TranslatesAutoresizingMaskIntoConstraints = false;
        HintLabel.TopAnchor.ConstraintEqualTo(this.TopAnchor, 2).Active = true;
        HintLabel.LeftAnchor.ConstraintEqualTo(this.LeftAnchor, 16).Active = true;
        HintLabel.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor, -10).Active = true;
        HintLabel.BottomAnchor.ConstraintLessThanOrEqualTo(this.BottomAnchor, -12).Active = true;

        HintLabel.SizeToFit();
        BringSubviewToFront(HintLabel);
    }

    /// <summary>
    /// Sets the right margin zero.
    /// </summary>
    protected void SetRightMarginZero()
    {
        if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
        {
            StackH.LayoutMargins = new UIEdgeInsets(6, 16, 6, 0);
        }
    }


    protected virtual void SetUpContentView()
    {
        //remove existing views 
        ImageView.RemoveFromSuperview();
        TextLabel.RemoveFromSuperview();
        ImageView.Hidden = true;
        TextLabel.Hidden = true;

        //外側のHorizontalStackView
        //Outer HorizontalStackView
        StackH = new UIStackView
        {
            Axis = UILayoutConstraintAxis.Horizontal,
            Alignment = UIStackViewAlignment.Center,
            Spacing = 16,
            Distribution = UIStackViewDistribution.Fill
        };
        //set margin
        StackH.LayoutMargins = new UIEdgeInsets(6, 16, 6, 16);
        StackH.LayoutMarginsRelativeArrangement = true;

        IconView = new UIImageView();

        //round corners
        IconView.ClipsToBounds = true;

        StackH.AddArrangedSubview(IconView);

        UpdateIconSize();

        //右に配置するVerticalStackView（コアの部品とDescriptionを格納）
        //VerticalStackView that is arranged at right. ( put main parts and Description ) 
        StackV = new UIStackView
        {
            Axis = UILayoutConstraintAxis.Vertical,
            Alignment = UIStackViewAlignment.Fill,
            Spacing = 4,
            Distribution = UIStackViewDistribution.Fill,
        };

        //右側上段に配置するHorizontalStackView(LabelTextとValueTextを格納）
        //HorizontalStackView that is arranged at upper in right.(put LabelText and ValueText)
        ContentStack = new UIStackView
        {
            Axis = UILayoutConstraintAxis.Horizontal,
            Alignment = UIStackViewAlignment.Fill,
            Spacing = 6,
            Distribution = UIStackViewDistribution.Fill,
        };

        TitleLabel = new UILabel();
        DescriptionLabel = new UILabel();

        DescriptionLabel.Lines = 0;
        DescriptionLabel.LineBreakMode = UILineBreakMode.WordWrap;

        ContentStack.AddArrangedSubview(TitleLabel);

        StackV.AddArrangedSubview(ContentStack);
        StackV.AddArrangedSubview(DescriptionLabel);

        StackH.AddArrangedSubview(StackV);

        //余った領域を広げる優先度の設定（低いものが優先して拡大する）
        IconView.SetContentHuggingPriority(999f, UILayoutConstraintAxis.Horizontal); //if possible, not to expand. 極力広げない
        StackV.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
        ContentStack.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
        TitleLabel.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
        DescriptionLabel.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);


        //縮まりやすさの設定（低いものが優先して縮まる）
        IconView.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal); //if possible, not to shrink. 極力縮めない
        StackV.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
        ContentStack.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
        TitleLabel.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
        DescriptionLabel.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);

        IconView.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Vertical);
        IconView.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Vertical);
        StackV.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Vertical);
        StackV.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Vertical);

        ContentView.AddSubview(StackH);

        StackH.TranslatesAutoresizingMaskIntoConstraints = false;
        StackH.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
        StackH.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
        StackH.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
        StackH.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;


        var minHeight = Math.Max(CellParent?.RowHeight ?? 44, SettingsViewHandler.MinRowHeight);
        _minheightConstraint = StackH.HeightAnchor.ConstraintGreaterThanOrEqualTo(minHeight);
        // fix warning-log:Unable to simultaneously satisfy constraints.
        _minheightConstraint.Priority = 999f; // this is superior to any other view.
        _minheightConstraint.Active = true;
        
        if (!String.IsNullOrEmpty(Cell.AutomationId)) 
        {
            ContentStack.AccessibilityIdentifier = Cell.AutomationId;
        }
    }

}

