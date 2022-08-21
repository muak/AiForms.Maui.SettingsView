using System;

namespace AiForms.Settings;

public class DropEventArgs:EventArgs
{       
    public Section Section { get; }
    public CellBase Cell { get; }
    public object SectionSource { get; }
    public object CellSource { get; }

    public DropEventArgs(Section section,CellBase cell)
    {
        Section = section;
        Cell = cell;
        SectionSource = section.BindingContext;
        CellSource = cell.BindingContext;
    }
}
