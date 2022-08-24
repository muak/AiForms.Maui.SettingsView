using System;
namespace AiForms.Settings.Handlers
{
    public partial class LabelCellBaseHandler<TvirtualCell, TnativeCell>
    {
        public LabelCellBaseHandler(): base(LabelMapper)
        {
        }

        public LabelCellBaseHandler(IPropertyMapper mapper = null) : base (mapper ?? LabelMapper)
        {
        }
    }
}

