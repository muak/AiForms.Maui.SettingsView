using System;
using Microsoft.Maui;
using Font = Microsoft.Maui.Font;

namespace AiForms.Settings.Extensions;

public static class FontExtension
{
    public static Font ToFont(this string fontFamily, double fontSize, FontAttributes fontAttributes, double? defaultSize = null)
    {
        if (defaultSize.HasValue && (fontSize <= 0 || double.IsNaN(fontSize)))
        {
            fontSize = defaultSize.Value;
        }            

        return Font.OfSize(fontFamily, fontSize, enableScaling: false).WithAttributes(fontAttributes);
    }
}

