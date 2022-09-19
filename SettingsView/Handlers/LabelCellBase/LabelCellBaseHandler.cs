using System;
namespace AiForms.Settings.Handlers
{
    public partial class LabelCellBaseHandler<TvirtualCell, TnativeCell>
    {
        public static CommandMapper<LabelCell, LabelCellBaseHandler<TvirtualCell, TnativeCell>> LabelCommandMapper = new(BaseCommandMapper);

        public LabelCellBaseHandler(): base(LabelMapper, LabelCommandMapper)
        {
        }

        public LabelCellBaseHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base (mapper ?? LabelMapper, commandMapper ?? LabelCommandMapper)
        {
        }
    }
}

