using System;
using Reactive.Bindings;

namespace Sample.ViewModels;

public class CustomHeaderViewModel:BindableBase
{
    public ReactivePropertySlim<string> Description { get; } = new ReactivePropertySlim<string>();
    public ReactivePropertySlim<bool> IsAgree { get; } = new ReactivePropertySlim<bool>();

    public CustomHeaderViewModel()
    {        
        Description.Value = "Text TextText Text Text Text Text Text Text Text Text Text Text Text Text Text End";
    }
}

