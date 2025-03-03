#nullable enable
using System;
using Android.Views;

namespace AiForms.Settings.Platforms.Droid;

[Android.Runtime.Preserve(AllMembers = true)]
public static class ViewExtension
{
    public static T? GetParentOfType<T>(this Android.Views.View view)
       where T : class
    {
        if (view is T t)
            return t;

        return view.GetParent().GetParentOfType<T>();
    }
    internal static T? GetParentOfType<T>(this Android.Views.IViewParent? view)
            where T : class
    {
        if (view is T t)
            return t;

        while (view != null)
        {
            T? parent = view?.GetParent() as T;
            if (parent != null)
                return parent;

            view = view?.GetParent();
        }

        return default;
    }
    public static IViewParent? GetParent(this Android.Views.View view)
    {
        return view.Parent;
    }

    public static IViewParent? GetParent(this IViewParent? view)
    {
        return view?.Parent;
    }
}

