using System;
using Microsoft.Maui.Handlers;

namespace AiForms.Settings.Extensions;

public static class HandlerCleanUpHelper
{
    public static void AddCleanUpEvent(this View view)
    {
        if (view is not Element element)
        {
            return;
        }

        Page parentPage;

        void PageUnloaded(object sender, EventArgs e)
        {
            view.Handler?.DisconnectHandler();
            if (parentPage is not null)
            {
                parentPage.Unloaded -= PageUnloaded;
                parentPage = null;
            }
        }        

        foreach (var el in element.GetParentsPath())
        {
            if (el is Page page)
            {
                parentPage = page;
                page.Unloaded += PageUnloaded;
            }
        }        
    }
}

