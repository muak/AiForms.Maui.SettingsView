using System;
using System.Runtime.InteropServices;
using UIKit;

namespace AiForms.Settings.Platforms.iOS;

/// <summary>
/// Thickness extensions.
/// </summary>
[Foundation.Preserve(AllMembers = true)]
public static class ThicknessExtensions
{
    /// <summary>
    /// To the UIEdgeinsets.
    /// </summary>
    /// <returns>The UIE dge insets.</returns>
    /// <param name="forms">Forms.</param>
    public static UIEdgeInsets ToUIEdgeInsets(this Thickness forms)
    {
        return new UIEdgeInsets(
            (NFloat)forms.Top,
            (NFloat)forms.Left,
            (NFloat)forms.Bottom,
            (NFloat)forms.Right
        );
    }
}
