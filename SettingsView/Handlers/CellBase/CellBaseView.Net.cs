using System;
using System.ComponentModel;

namespace AiForms.Settings.Handlers;

public class CellBaseView
{
    public CellBase Cell { get; set; }

    public void SetEnabledAppearance(bool enabled)
    {
    }

    public virtual void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
    }

    public virtual void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
    }

    public virtual void SectionPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
    }
}
