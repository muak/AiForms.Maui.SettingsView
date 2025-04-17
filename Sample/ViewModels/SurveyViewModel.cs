using System.Collections.ObjectModel;
using Reactive.Bindings;

namespace Sample.ViewModels;

public class SurveyViewModel: BindableBase
{
    public ObservableCollection<string> ItemsSource { get; set; } = new();
    public ObservableCollection<string> ItemsSource2 { get; set; } = new();
    public ReactivePropertySlim<bool> IsShow { get; } = new();

    public AsyncReactiveCommand SubmitCommand { get; } = new();
    
    public SurveyViewModel()
    {
        ItemsSource2.Add("X");
        ItemsSource2.Add("Y");
        ItemsSource2.Add("Z");
        
        SubmitCommand.Subscribe(async () =>
        {
            ItemsSource.Clear();
            await Task.Delay(500);
            ItemsSource = new(new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m","n","o","p","q","r","a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m","n","o","p","q","r" });
            IsShow.Value = true;
            RaisePropertyChanged(nameof(ItemsSource));
        });
    }
}