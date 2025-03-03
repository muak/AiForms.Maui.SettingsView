﻿using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Controls.Compatibility;
using AView = Android.Views.View;
using Resource = global::AiForms.Settings.Resource;

namespace AiForms.Settings.Platforms.Droid;

[Android.Runtime.Preserve(AllMembers = true)]
internal class ViewHolder : RecyclerView.ViewHolder
{
    public RowInfo RowInfo { get; set; }

    public ViewHolder(AView view) : base(view) { }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ItemView?.Dispose();
            ItemView = null;
        }
        base.Dispose(disposing);
    }
}

[Android.Runtime.Preserve(AllMembers = true)]
internal interface IHeaderViewHolder
{ 
}

[Android.Runtime.Preserve(AllMembers = true)]
internal interface IFooterViewHolder
{
}

[Android.Runtime.Preserve(AllMembers = true)]
internal class HeaderViewHolder :ViewHolder, IHeaderViewHolder
{
    public TextView TextView { get; private set; }

    public HeaderViewHolder(AView view) : base(view)
    {
        TextView = view.FindViewById<TextView>(Resource.Id.HeaderCellText);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TextView?.Dispose();
            TextView = null;
        }
        base.Dispose(disposing);
    }
}

[Android.Runtime.Preserve(AllMembers = true)]
internal class FooterViewHolder :ViewHolder, IFooterViewHolder
{
    public TextView TextView { get; private set; }

    public FooterViewHolder(AView view) : base(view)
    {
        TextView = view.FindViewById<TextView>(Resource.Id.FooterCellText);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TextView?.Dispose();
            TextView = null;
        }
        base.Dispose(disposing);
    }
}

[Android.Runtime.Preserve(AllMembers = true)]
internal class CustomHeaderViewHolder :ViewHolder, IHeaderViewHolder
{
    public CustomHeaderViewHolder(AView view) : base(view)
    {
        view.LayoutParameters = new ViewGroup.LayoutParams(-1, -2);
    }       
}

[Android.Runtime.Preserve(AllMembers = true)]
internal class CustomFooterViewHolder :ViewHolder, IFooterViewHolder
{
    public CustomFooterViewHolder(AView view) : base(view)
    {
        view.LayoutParameters = new ViewGroup.LayoutParams(-1, -2);
    }
}

[Android.Runtime.Preserve(AllMembers = true)]
internal class ContentBodyViewHolder : ViewHolder
{
    public LinearLayout Body { get; private set; }
    //public RowInfo RowInfo { get; set; }

    public ContentBodyViewHolder(AView view) : base(view)
    {
        Body = view.FindViewById<LinearLayout>(Resource.Id.ContentCellBody);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            var nativeCell = Body.GetChildAt(0);            
            nativeCell?.Dispose();
            Body?.Dispose();
            Body = null;
            ItemView.SetOnClickListener(null);
            ItemView.SetOnLongClickListener(null);
        }
        base.Dispose(disposing);
    }
}
