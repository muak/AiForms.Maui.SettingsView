namespace Sample.ViewModels;

public class DynamicHeaderSizeViewModel: BindableBase, IInitializeAsync,IPageLifecycleAware
{
    public List<SampleHeaderModel> Items { get; set; }
    
    public Task InitializeAsync(INavigationParameters parameters)
    {
        Items = new List<SampleHeaderModel>
        {
            new SampleHeaderModel
            {
                HeaderTitle = "Header 1",
                DataList = new List<SampleDataModel>()
                {
                    new SampleDataModel()
                    {
                        Title = "Title 1",
                        Data = "Data 1"
                    },
                    new SampleDataModel()
                    {
                        Title = "Title 2",
                        Data = "Data 2"
                    },
                    new SampleDataModel()
                    {
                        Title = "Title 3",
                        Data = "Data 3"
                    },
                }
            },
            new SampleHeaderModel()
            {
                HeaderTitle = "Header 2",
                DataList = new List<SampleDataModel>()
                {
                    new SampleDataModel()
                    {
                        Title = "Title 1",
                        Data = "Data 1"
                    },
                    new SampleDataModel()
                    {
                        Title = "Title 2",
                        Data = "Data 2"
                    },
                    new SampleDataModel()
                    {
                        Title = "Title 3",
                        Data = "Data 3"
                    },
                }
            },
            new SampleHeaderModel()
            {
                HeaderTitle = "Header 3",
                DataList = new List<SampleDataModel>()
                {
                    new SampleDataModel()
                    {
                        Title = "Title 1",
                        Data = "Data 1"
                    },
                    new SampleDataModel()
                    {
                        Title = "Title 2",
                        Data = "Data 2"
                    },
                    new SampleDataModel()
                    {
                        Title = "Title 3",
                        Data = "Data 3"
                    },
                }
            },
            new SampleHeaderModel()
            {
                HeaderTitle = "Header 4",
                DataList = new List<SampleDataModel>()
                {
                    new SampleDataModel()
                    {
                        Title = "Title 1",
                        Data = "Data 1"
                    },
                    new SampleDataModel()
                    {
                        Title = "Title 2",
                        Data = "Data 2"
                    },
                    new SampleDataModel()
                    {
                        Title = "Title 3",
                        Data = "Data 3"
                    },
                }
            },
        };
        return Task.CompletedTask;
    }

    public async void OnAppearing()
    {
        await Task.Delay(250);
        RaisePropertyChanged(nameof(Items));
    }

    public void OnDisappearing()
    {
    }
}

public class SampleHeaderModel
{
    public List<SampleDataModel> DataList { get; set; }
    public string HeaderTitle { get; set; }
}

public class SampleDataModel
{
    public string Title { get; set; }
    public string Data { get; set; }
}