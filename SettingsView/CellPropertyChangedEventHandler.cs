using System;
using System.ComponentModel;

namespace AiForms.Settings;

public class CellPropertyChangedEventArgs : PropertyChangedEventArgs
{
    public Section Section { get; }

    public CellPropertyChangedEventArgs(string propertyName, Section section) : base(propertyName)
    {
        Section = section;
    }
}
