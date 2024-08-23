using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaPrototype.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _greeting = "Welcome to Bible Well in Avalonia!";
    }
}
