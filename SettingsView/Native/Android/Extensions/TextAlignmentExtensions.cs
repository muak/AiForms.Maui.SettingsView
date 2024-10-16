﻿using System;
using GravityFlags = Android.Views.GravityFlags;

namespace AiForms.Settings.Platforms.Droid;

/// <summary>
/// Text alignment extensions.
/// </summary>
[Android.Runtime.Preserve(AllMembers = true)]
public static class TextAlignmentExtensions
{
    /// <summary>
    /// Tos the gravity flags.
    /// </summary>
    /// <returns>The gravity flags.</returns>
    /// <param name="forms">Forms.</param>
    public static GravityFlags ToGravityFlags(this TextAlignment forms)
    {
        switch (forms) {
            case TextAlignment.Start:
                return GravityFlags.Left | GravityFlags.CenterVertical;
            case TextAlignment.Center:
                return GravityFlags.Center | GravityFlags.CenterVertical;
            case TextAlignment.End:
                return GravityFlags.Right | GravityFlags.CenterVertical;
            default:
                return GravityFlags.Right | GravityFlags.CenterVertical;
        }
    }
}
