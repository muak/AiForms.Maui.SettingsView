﻿using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace AiForms.Settings.Platforms.iOS;

public class TextHeaderView : UITableViewHeaderFooterView
{
    public PaddingLabel Label { get; set; }
    List<NSLayoutConstraint> _constraints = new List<NSLayoutConstraint>();
    LayoutAlignment _curAlignment;
    bool _isInitialized;    

    public TextHeaderView()
    {
    }

    public TextHeaderView(NSCoder coder) : base(coder)
    {
    }

    protected TextHeaderView(NSObjectFlag t) : base(t)
    {
    }

    protected internal TextHeaderView(NativeHandle handle) : base(handle)
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

    public TextHeaderView(CGRect frame) : base(frame)
    {
    }

    public TextHeaderView(NSString reuseIdentifier) : base(reuseIdentifier)
    {
    }

    public void SetVerticalAlignment(LayoutAlignment align)
    {
        if(_isInitialized && align == _curAlignment)
        {
            return;
        }

        foreach (var c in _constraints)
        {
            c.Active = false;
            c.Dispose();
        }
        _constraints.Clear();

        _constraints.Add(Label.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor, 0));
        _constraints.Add(Label.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor, 0));

        if (align == LayoutAlignment.Start)
        {
            _constraints.Add(Label.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, 0));
        }
        else if (align == LayoutAlignment.End)
        {
            _constraints.Add(Label.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, 0));
        }
        else
        {
            _constraints.Add(Label.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor, 0));
        }

        _constraints.ForEach(c => {
            c.Priority = 999f; // fix warning-log:Unable to simultaneously satisfy constraints.
            c.Active = true;
        });

        _curAlignment = align;
        _isInitialized = true;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if(disposing)
        {
            _constraints.ForEach(c => c.Dispose());
            Label?.Dispose();
            Label = null;
            BackgroundView?.Dispose();
            BackgroundView = null;
        }
    }
}
