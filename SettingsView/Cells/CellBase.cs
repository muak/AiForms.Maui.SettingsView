﻿using System;
using System.ComponentModel;
using Microsoft.Maui.Platform;

namespace AiForms.Settings;

public class CellBase: Element, IImageSourcePart
{
    /// <summary>
    /// Occurs when tapped.
    /// </summary>
    public event EventHandler Tapped;
    internal void OnTapped()
    {
        if (Tapped != null)
            Tapped(this, EventArgs.Empty);
    }       

    /// <summary>
    /// The title property.
    /// </summary>
    public static BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(CellBase),
            default(string),
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    /// <summary>
    /// The title color property.
    /// </summary>
    public static BindableProperty TitleColorProperty =
        BindableProperty.Create(
            nameof(TitleColor),
            typeof(Color),
            typeof(CellBase),
            KnownColor.Default,
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the color of the title.
    /// </summary>
    /// <value>The color of the title.</value>
    public Color TitleColor
    {
        get { return (Color)GetValue(TitleColorProperty); }
        set { SetValue(TitleColorProperty, value); }
    }

    /// <summary>
    /// The title font size property.
    /// </summary>
    public static BindableProperty TitleFontSizeProperty =
        BindableProperty.Create(
            nameof(TitleFontSize),
            typeof(double),
            typeof(CellBase),
            -1.0,
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the size of the title font.
    /// </summary>
    /// <value>The size of the title font.</value>
    [TypeConverter(typeof(FontSizeConverter))]
    public double TitleFontSize
    {
        get { return (double)GetValue(TitleFontSizeProperty); }
        set { SetValue(TitleFontSizeProperty, value); }
    }

    public static BindableProperty TitleFontFamilyProperty = BindableProperty.Create(
        nameof(TitleFontFamily),
        typeof(string),
        typeof(CellBase),
        default(string),
        defaultBindingMode: BindingMode.OneWay
    );

    public string TitleFontFamily
    {
        get { return (string)GetValue(TitleFontFamilyProperty); }
        set { SetValue(TitleFontFamilyProperty, value); }
    }

    public static BindableProperty TitleFontAttributesProperty = BindableProperty.Create(
        nameof(TitleFontAttributes),
        typeof(FontAttributes?),
        typeof(CellBase),
        null,
        defaultBindingMode: BindingMode.OneWay
    );

    public FontAttributes? TitleFontAttributes
    {
        get { return (FontAttributes?)GetValue(TitleFontAttributesProperty); }
        set { SetValue(TitleFontAttributesProperty, value); }
    }

    /// <summary>
    /// The description property.
    /// </summary>
    public static BindableProperty DescriptionProperty =
        BindableProperty.Create(
            nameof(Description),
            typeof(string),
            typeof(CellBase),
            default(string),
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string Description
    {
        get { return (string)GetValue(DescriptionProperty); }
        set { SetValue(DescriptionProperty, value); }
    }

    /// <summary>
    /// The description color property.
    /// </summary>
    public static BindableProperty DescriptionColorProperty =
        BindableProperty.Create(
            nameof(DescriptionColor),
            typeof(Color),
            typeof(CellBase),
            KnownColor.Default,
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the color of the description.
    /// </summary>
    /// <value>The color of the description.</value>
    public Color DescriptionColor
    {
        get { return (Color)GetValue(DescriptionColorProperty); }
        set { SetValue(DescriptionColorProperty, value); }
    }

    /// <summary>
    /// The description font size property.
    /// </summary>
    public static BindableProperty DescriptionFontSizeProperty =
        BindableProperty.Create(
            nameof(DescriptionFontSize),
            typeof(double),
            typeof(CellBase),
            -1.0d,
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the size of the description font.
    /// </summary>
    /// <value>The size of the description font.</value>
    [TypeConverter(typeof(FontSizeConverter))]
    public double DescriptionFontSize
    {
        get { return (double)GetValue(DescriptionFontSizeProperty); }
        set { SetValue(DescriptionFontSizeProperty, value); }
    }

    public static BindableProperty DescriptionFontFamilyProperty = BindableProperty.Create(
        nameof(DescriptionFontFamily),
        typeof(string),
        typeof(CellBase),
        default(string),
        defaultBindingMode: BindingMode.OneWay
    );

    public string DescriptionFontFamily
    {
        get { return (string)GetValue(DescriptionFontFamilyProperty); }
        set { SetValue(DescriptionFontFamilyProperty, value); }
    }

    public static BindableProperty DescriptionFontAttributesProperty = BindableProperty.Create(
        nameof(DescriptionFontAttributes),
        typeof(FontAttributes?),
        typeof(CellBase),
        null,
        defaultBindingMode: BindingMode.OneWay
    );

    public FontAttributes? DescriptionFontAttributes
    {
        get { return (FontAttributes?)GetValue(DescriptionFontAttributesProperty); }
        set { SetValue(DescriptionFontAttributesProperty, value); }
    }

    /// <summary>
    /// The hint text property.
    /// </summary>
    public static BindableProperty HintTextProperty =
        BindableProperty.Create(
            nameof(HintText),
            typeof(string),
            typeof(CellBase),
            default(string),
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the hint text.
    /// </summary>
    /// <value>The hint text.</value>
    public string HintText
    {
        get { return (string)GetValue(HintTextProperty); }
        set { SetValue(HintTextProperty, value); }
    }

    /// <summary>
    /// The hint text color property.
    /// </summary>
    public static BindableProperty HintTextColorProperty =
        BindableProperty.Create(
            nameof(HintTextColor),
            typeof(Color),
            typeof(CellBase),
            KnownColor.Default,
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the color of the hint text.
    /// </summary>
    /// <value>The color of the hint text.</value>
    public Color HintTextColor
    {
        get { return (Color)GetValue(HintTextColorProperty); }
        set { SetValue(HintTextColorProperty, value); }
    }

    /// <summary>
    /// The hint font size property.
    /// </summary>
    public static BindableProperty HintFontSizeProperty =
        BindableProperty.Create(
            nameof(HintFontSize),
            typeof(double),
            typeof(CellBase),
            -1.0d,
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the size of the hint font.
    /// </summary>
    /// <value>The size of the hint font.</value>
    [TypeConverter(typeof(FontSizeConverter))]
    public double HintFontSize
    {
        get { return (double)GetValue(HintFontSizeProperty); }
        set { SetValue(HintFontSizeProperty, value); }
    }

    public static BindableProperty HintFontFamilyProperty = BindableProperty.Create(
        nameof(HintFontFamily),
        typeof(string),
        typeof(CellBase),
        default(string),
        defaultBindingMode: BindingMode.OneWay
    );

    public string HintFontFamily
    {
        get { return (string)GetValue(HintFontFamilyProperty); }
        set { SetValue(HintFontFamilyProperty, value); }
    }

    public static BindableProperty HintFontAttributesProperty = BindableProperty.Create(
        nameof(HintFontAttributes),
        typeof(FontAttributes?),
        typeof(CellBase),
        null,
        defaultBindingMode: BindingMode.OneWay
    );

    public FontAttributes? HintFontAttributes
    {
        get { return (FontAttributes?)GetValue(HintFontAttributesProperty); }
        set { SetValue(HintFontAttributesProperty, value); }
    }

    /// <summary>
    /// The background color property.
    /// </summary>
    public static BindableProperty BackgroundColorProperty =
        BindableProperty.Create(
            nameof(BackgroundColor),
            typeof(Color),
            typeof(CellBase),
            KnownColor.Default,
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the color of the background.
    /// </summary>
    /// <value>The color of the background.</value>
    public Color BackgroundColor
    {
        get { return (Color)GetValue(BackgroundColorProperty); }
        set { SetValue(BackgroundColorProperty, value); }
    }

    /// <summary>
    /// The icon source property.
    /// </summary>
    public static BindableProperty IconSourceProperty =
        BindableProperty.Create(
            nameof(IconSource),
            typeof(ImageSource),
            typeof(CellBase),
            default(ImageSource),
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the icon source.
    /// </summary>
    /// <value>The icon source.</value>
    [TypeConverter(typeof(ImageSourceConverter))]
    public ImageSource IconSource
    {
        get { return (ImageSource)GetValue(IconSourceProperty); }
        set { SetValue(IconSourceProperty, value); }
    }

    /// <summary>
    /// The icon size property.
    /// </summary>
    public static BindableProperty IconSizeProperty =
        BindableProperty.Create(
            nameof(IconSize),
            typeof(Size),
            typeof(CellBase),
            default(Size),
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the size of the icon.
    /// </summary>
    /// <value>The size of the icon.</value>
    [TypeConverter(typeof(SizeConverter))]
    public Size IconSize
    {
        get { return (Size)GetValue(IconSizeProperty); }
        set { SetValue(IconSizeProperty, value); }
    }

    /// <summary>
    /// The icon radius property.
    /// </summary>
    public static BindableProperty IconRadiusProperty =
        BindableProperty.Create(
            nameof(IconRadius),
            typeof(double),
            typeof(CellBase),
            -1.0d,
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the icon radius.
    /// </summary>
    /// <value>The icon radius.</value>
    public double IconRadius
    {
        get { return (double)GetValue(IconRadiusProperty); }
        set { SetValue(IconRadiusProperty, value); }
    }

    /// <summary>
    /// The IsVisible property.
    /// </summary>
    public static BindableProperty IsVisibleProperty =
        BindableProperty.Create(
            nameof(IsVisible),
            typeof(bool),
            typeof(CellBase),
            true,
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="CellBase"/> is visible.
    /// </summary>
    /// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
    public bool IsVisible
    {
        get { return (bool)GetValue(IsVisibleProperty); }
        set { SetValue(IsVisibleProperty, value); }
    }

    public static BindableProperty HeightProperty = BindableProperty.Create(
            nameof(Height),
            typeof(double),
            typeof(CellBase),
            -1d,
            defaultBindingMode: BindingMode.OneWay
        );

    public double Height{
        get { return (double)GetValue(HeightProperty); }
        set { SetValue(HeightProperty, value); }
    }

    public static BindableProperty IsEnabledProperty = BindableProperty.Create(
            nameof(IsEnabled),
            typeof(bool),
            typeof(CellBase),
            true,
            defaultBindingMode: BindingMode.OneWay
        );

    public bool IsEnabled{
        get { return (bool)GetValue(IsEnabledProperty); }
        set { SetValue(IsEnabledProperty, value); }
    }

    /// <summary>
    /// Gets or sets the section.
    /// </summary>
    /// <value>The section.</value>
    public Section Section { get; set; }

    public IImageSource Source => IconSource;
    public bool IsLoading { get; set; }

    public bool IsAnimationPlaying { get; set; }

    public void UpdateIsLoading(bool isLoading)
    {
        IsLoading = isLoading;
    }

    public void SetEnabledAppearance(bool isEnabled)
    {
        Handler?.Invoke(nameof(SetEnabledAppearance));
    }

    public virtual void Reload()
    {
        if (Section == null)
        {
            return;
        }
        var index = Section.IndexOf(this);
        if (index < 0)
        {
            return;
        }

        // raise replase event manually.
        var temp = Section[index];
        Section[index] = temp;
    }

#if ANDROID
    // This is used by ListView to pass data to the GetCell call
    // Ideally we can pass these as arguments to ToHandler
    // But we'll do that in a different smaller more targeted PR
    internal Android.Views.View ConvertView { get; set; }

#elif IOS || MACCATALYST
	internal UIKit.UITableViewCell ReusableCell { get; set; }
    internal Queue<UIKit.UITableViewCell> ReusableCellQueue { get; } = new Queue<UIKit.UITableViewCell>();
	internal UIKit.UITableView TableView { get; set; }
#endif

}

