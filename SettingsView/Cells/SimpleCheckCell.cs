using System;
namespace AiForms.Settings;

public class SimpleCheckCell: CellBase
{
    public static BindableProperty CheckedProperty = BindableProperty.Create(
            nameof(Checked),
            typeof(bool),
            typeof(SimpleCheckCell),
            default(bool),
            defaultBindingMode: BindingMode.OneWay
        );

    public bool Checked{
        get { return (bool)GetValue(CheckedProperty); }
        set { SetValue(CheckedProperty, value); }
    }

    /// <summary>
    /// The value property.
    /// </summary>
    public static BindableProperty ValueProperty =
        BindableProperty.Create(
            nameof(Value),
            typeof(object),
            typeof(SimpleCheckCell),
            default(object),
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>The value.</value>
    public object Value
    {
        get { return (object)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    /// <summary>
    /// The accent color property.
    /// </summary>
    public static BindableProperty AccentColorProperty =
        BindableProperty.Create(
            nameof(AccentColor),
            typeof(Color),
            typeof(SimpleCheckCell),
            default(Color),
            defaultBindingMode: BindingMode.OneWay
        );

    /// <summary>
    /// Gets or sets the color of the accent.
    /// </summary>
    /// <value>The color of the accent.</value>
    public Color AccentColor
    {
        get { return (Color)GetValue(AccentColorProperty); }
        set { SetValue(AccentColorProperty, value); }
    }
}

