using System;
using System.Reflection;

namespace AiForms.Settings.Platforms.iOS;

public static class DisposeHelpers
{
    static Type ModalWrapper = typeof(Element).Assembly.GetType("Microsoft.Maui.Controls.Platform.ModalWrapper");
    static MethodInfo ModalWrapperDispose = ModalWrapper.GetMethod("Dispose");
    static MethodInfo ElementDescendants = typeof(Element).GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(x => x.Name == "Descendants" && !x.IsGenericMethod);

    public static void DisposeModalAndChildHandlers(this Microsoft.Maui.IElement view)
    {
        IPlatformViewHandler renderer;
        var desendants = ElementDescendants.Invoke((Element)view, new object[] { }) as IEnumerable<Element>;
        foreach (Element child in desendants)
        {
            if (child is VisualElement ve)
            {
                ve.Handler?.DisconnectHandler();

                if (ve.Handler is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        if (view is VisualElement visualElement)
        {
            renderer = (visualElement.Handler as IPlatformViewHandler);
            if (renderer != null)
            {
                if (renderer.ViewController != null)
                {
                    if (renderer.ViewController.ParentViewController.GetType() == ModalWrapper)
                    {
                        var modalWrapper = Convert.ChangeType(renderer.ViewController.ParentViewController, ModalWrapper);
                        ModalWrapperDispose.Invoke(modalWrapper, new object[] { });
                    }
                }

                renderer.PlatformView?.RemoveFromSuperview();

                if (view.Handler is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }

    public static void DisposeHandlersAndChildren(this IPlatformViewHandler rendererToRemove)
    {
        if (rendererToRemove == null)
        {
            return;
        }

        if (rendererToRemove.VirtualView != null && rendererToRemove.VirtualView.Handler == rendererToRemove)
        {
            rendererToRemove.VirtualView.Handler?.DisconnectHandler();
        }

        if (rendererToRemove.PlatformView != null)
        {
            var subviews = rendererToRemove.PlatformView.Subviews;
            for (var i = 0; i < subviews.Length; i++)
            {
                if (subviews[i] is IPlatformViewHandler childRenderer)
                {
                    DisposeHandlersAndChildren(childRenderer);
                }
            }

            rendererToRemove.PlatformView.RemoveFromSuperview();
        }

        if (rendererToRemove is IDisposable disposable)
            disposable.Dispose();
    }
}

