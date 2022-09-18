using System;
namespace AiForms.Settings.Handlers;

public partial class CustomCellHandler
{
    public static CommandMapper<CustomCell, CustomCellHandler> CustomCommandMapper = new(BaseCommandMapper);

    public CustomCellHandler() : base(CustomMapper)
    {
    }

    public CustomCellHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? CustomMapper, commandMapper ?? CustomCommandMapper)
    {
    }
}

