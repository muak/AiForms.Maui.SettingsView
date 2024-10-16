﻿using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace AiForms.Settings.Platforms.iOS;

public class TextFooterView : UITableViewHeaderFooterView
{
    public PaddingLabel Label { get; set; }
    List<NSLayoutConstraint> _constraints = new List<NSLayoutConstraint>();

    public TextFooterView()
    {
    }

    public TextFooterView(NSCoder coder) : base(coder)
    {
    }

    protected TextFooterView(NSObjectFlag t) : base(t)
    {
    }

    protected internal TextFooterView(NativeHandle handle) : base(handle)
    {
        Label = new PaddingLabel();
        Label.Lines = 0;
        Label.LineBreakMode = UILineBreakMode.CharacterWrap;
        Label.TranslatesAutoresizingMaskIntoConstraints = false;

        ContentView.AddSubview(Label);

        _constraints.Add(Label.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, 0));
        _constraints.Add(Label.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, 0));
        _constraints.Add(Label.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor, 0));
        _constraints.Add(Label.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor, 0));

        _constraints.ForEach(c => {
            c.Priority = 999f; // fix warning-log:Unable to simultaneously satisfy constraints.
            c.Active = true;
        });

        this.BackgroundView = new UIView();
    }

    public TextFooterView(CGRect frame) : base(frame)
    {
    }

    public TextFooterView(NSString reuseIdentifier) : base(reuseIdentifier)
    {
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _constraints.ForEach(c => c.Dispose());
            Label?.Dispose();
            Label = null;
            BackgroundView?.Dispose();
            BackgroundView = null;
        }
    }
}
