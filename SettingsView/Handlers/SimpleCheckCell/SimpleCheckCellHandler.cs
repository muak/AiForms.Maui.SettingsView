using System;

namespace AiForms.Settings.Handlers;

public partial class SimpleCheckCellHandler
{
    public static IPropertyMapper<SimpleCheckCell, SimpleCheckCellHandler> SimpleCheckMapper =
        new PropertyMapper<SimpleCheckCell, SimpleCheckCellHandler>(BasePropertyMapper)
        {
            [nameof(SimpleCheckCell.AccentColor)] = MapAccentColor,
            [nameof(SimpleCheckCell.Checked)] = MapChecked,
        };

    public static CommandMapper<SimpleCheckCell, SimpleCheckCellHandler> SimpleCheckCommandMapper = new(BaseCommandMapper);

    public SimpleCheckCellHandler() : base(SimpleCheckMapper)
    {
    }

    public SimpleCheckCellHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? SimpleCheckMapper, commandMapper ?? SimpleCheckCommandMapper)
    {
    }
}

