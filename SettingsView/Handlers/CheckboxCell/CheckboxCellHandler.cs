using System;
namespace AiForms.Settings.Handlers;

public partial class CheckboxCellHandler
{
    public static CommandMapper<CheckboxCell, CheckboxCellHandler> CheckboxCommandMapper = new(BaseCommandMapper);

    public CheckboxCellHandler() : base(CheckboxMapper, CheckboxCommandMapper)
    {
    }

    public CheckboxCellHandler(IPropertyMapper mapper = null, CommandMapper commandMapper = null) : base(mapper ?? CheckboxMapper, commandMapper ?? CheckboxCommandMapper)
    {
    }
}

