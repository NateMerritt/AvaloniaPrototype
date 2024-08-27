using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvaloniaPrototype.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaPrototype.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel(IAquiferService aquiferService)
    {
        m_aquiferService = aquiferService;
    }

    public async Task<(string Name, string Content)> GetRandomResourceContentAsync()
    {
        var randomIndex = new Random().Next(0, 9);

        return await m_aquiferService.GetResourceContentAsync(s_contentIds[randomIndex]);
    }

    [ObservableProperty]
    private string _aquiferGreeting = "Aquifer!";

    [ObservableProperty]
    private string _loadingText = "Loading...";

    [ObservableProperty]
    private string _loadResourceButtonText = "Load Resource";

    [ObservableProperty]
    private string _resourceContentLabelText = "Content:";

    [ObservableProperty]
    private string _resourceNameLabelText = "Name:";

    [ObservableProperty]
    private string _wellGreeting = "Bible Well!";

    private static readonly IReadOnlyList<int> s_contentIds =
    [
        6204,
        205035,
        44947,
        205036,
        205055,
        44954,
        44955,
        1526,
        1533,
    ];

    private readonly IAquiferService m_aquiferService;
}
