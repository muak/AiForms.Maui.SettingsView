using AiForms.Settings;
using Sample.ViewModels;

namespace Sample.Views;

public partial class MainPage : ContentPage
{    
    public MainPage()
    {
        InitializeComponent();        
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if(BindingContext is MainViewModel vm)
        {
            vm.Content.Subscribe(async _ =>
            {
                //await Task.Delay(500);
                //MainThread.BeginInvokeOnMainThread(() => testCell.Reload());
                //MainThread.BeginInvokeOnMainThread(() => testCell.Reload());
                //Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(250),()=>testCell.Reload());
            });            
        }
    }

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() => testCell.Reload());
    }
}


