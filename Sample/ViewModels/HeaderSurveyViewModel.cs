using System;
using Reactive.Bindings;

namespace Sample.ViewModels;

public class HeaderSurveyViewModel: BindableBase
{
    public ReactivePropertySlim<bool> IsShow { get; } = new();

    public HeaderSurveyViewModel()
    {
    }
}

