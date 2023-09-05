using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;
using Reactive.Bindings;

namespace Sample.ViewModels
{
    public class MainViewModel: BindableBase
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(15, ErrorMessage = "Input text less than or equal to 15 characters")]
        public ReactiveProperty<string> InputText { get; }
        public ReadOnlyReactiveProperty<string> InputError { get; }

        public ReactiveProperty<bool> InputSectionVisible { get; } = new ReactiveProperty<bool>(true);
        public ReactivePropertySlim<string> Content { get; } = new ReactivePropertySlim<string>("");
        public ReactivePropertySlim<string> Description { get; } = new ReactivePropertySlim<string>("");

        public ReactiveCommand ToProfileCommand { get; set; } = new ReactiveCommand();
        public AsyncReactiveCommand SectionToggleCommand { get; set; }
        public ReactiveCommand ClearContentCommand { get; } = new ReactiveCommand();
        public ReactiveCommand AddContentCommand { get; } = new ReactiveCommand();

        public ObservableCollection<Person> ItemsSource { get; } = new ObservableCollection<Person>();
        public ObservableCollection<Person> SelectedItems { get; } = new ObservableCollection<Person>();

        public ObservableCollection<string> TextItems { get; } = new ObservableCollection<string>(new List<string> { "Red", "Blue", "Green", "Pink", "Black", "White" });
        public ReactiveProperty<string> SelectedText { get; } = new ReactiveProperty<string>("Green");

        string[] languages = { "Java", "C#", "JavaScript", "PHP", "Perl", "C++", "Swift", "Kotlin", "Python", "Ruby", "Scala", "F#" };

        public MainViewModel(INavigationService navigationService)
        {
            InputText = new ReactiveProperty<string>().SetValidateAttribute(() => this.InputText);

            InputError = InputText.ObserveErrorChanged
                                  .Select(x => x?.Cast<string>()?.FirstOrDefault())
                                  .ToReadOnlyReactiveProperty();

            SectionToggleCommand = InputText.ObserveHasErrors.Select(x => !x).ToAsyncReactiveCommand();
            SectionToggleCommand.Subscribe(async _ => {
                InputSectionVisible.Value = !InputSectionVisible.Value;
                await Task.Delay(250);
            });

            ToProfileCommand.Subscribe(async _ => {
                
                //Description.Value += "01234567890";
                await navigationService.NavigateAsync("ListPage");
            });

            AddContentCommand.Subscribe(_ =>
            {
                Content.Value += "A\n";
            });

            ClearContentCommand.Subscribe(_ =>
            {
                Content.Value = "";
            });

            foreach (var item in languages)
            {
                ItemsSource.Add(new Person()
                {
                    Name = item,
                    Age = 1
                });
            }

            SelectedItems.Add(ItemsSource[1]);
            SelectedItems.Add(ItemsSource[2]);
            SelectedItems.Add(ItemsSource[3]);
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}

