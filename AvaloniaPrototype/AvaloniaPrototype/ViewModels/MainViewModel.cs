using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaPrototype.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _aquiferGreeting = "Aquifer!";

        [ObservableProperty]
        private string _wellGreeting = "Bible Well!";
    }
}
