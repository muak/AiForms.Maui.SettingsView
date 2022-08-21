using System;
using System.ComponentModel;
using System.Globalization;

namespace AiForms.Settings;

/// <summary>
/// Size converter.
/// </summary>
public class SizeConverter : TypeConverter
{
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string valueStr)
        {
            var size = valueStr.Split(',');

            switch (size.Length)
            {
                case 1:
                    var w = double.Parse(size[0]);
                    return new Size(w, w);
                case 2:
                    return new Size(double.Parse(size[0]), double.Parse(size[1]));
            }
        }
        throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Size)}");
    }    
}
