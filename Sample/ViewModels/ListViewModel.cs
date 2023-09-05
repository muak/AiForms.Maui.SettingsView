using System;
using System.Collections.ObjectModel;

namespace Sample.ViewModels;

public class ListViewModel: BindableBase
{
    public ObservableCollection<string> ItemsSource { get; set; }

    public ListViewModel()
    {
        var list = new List<string>
        {
            "Item1","Item2","Item3","Item4","Item5","Item6","Item7","Item8","Item9","Item10",
            "Item11","Item12","Item13","Item14","Item15","Item16","Item17","Item18","Item19","Item20",
        };

        ItemsSource = new ObservableCollection<string>(list);
    }
}

