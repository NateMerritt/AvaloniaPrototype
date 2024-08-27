using AvaloniaPrototype.Services;

namespace AvaloniaPrototype.ViewModels;
internal class DesignMainViewModel : MainViewModel
{
    public DesignMainViewModel()
        : base(new DummyAquiferService())
    {
    }
}
