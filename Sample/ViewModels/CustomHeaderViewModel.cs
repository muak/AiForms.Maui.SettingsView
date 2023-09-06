using System;
using Reactive.Bindings;

namespace Sample.ViewModels;

public class CustomHeaderViewModel:BindableBase
{
    public ReactivePropertySlim<string> Description { get; } = new ReactivePropertySlim<string>();
    public ReactivePropertySlim<bool> IsAgree { get; } = new ReactivePropertySlim<bool>();

    public CustomHeaderViewModel()
    {
        //Description.Value = "退会すると、診察券登録はすべて解除され、利用規約に従いデータは適切に処理されます。";
        Description.Value = "Upon withdrawal, all medical ticket registrations will be cancelled and data will be properly processed in accordance with the Terms of Use.";
    }
}

