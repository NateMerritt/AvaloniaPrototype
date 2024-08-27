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

    public async Task GetRandomResourceContentAsync()
    {
        var randomIndex = new Random().Next(0, 9);
        var (name, content) = await m_aquiferService.GetResourceContentAsync(s_contentIds[randomIndex]);
        ResourceNameText = name;
        ResourceContentText = content;
    }

    public void SetState(State state)
    {
        switch (state)
        {
            case State.Ready:
                LoadInternetResourceText = Resources.MainView.LoadInternetResource;
                LoadInternetResourceIsEnabled = true;
                SaveLocalResourceText = Resources.MainView.SaveLocalResource;
                SaveLocalResourceIsEnabled = true;
                LoadLocalResourceText = Resources.MainView.LoadLocalResource;
                LoadLocalResourceIsEnabled = true;
                break;
            case State.LoadingInternetResource:
                LoadInternetResourceText = Resources.MainView.Loading;
                LoadInternetResourceIsEnabled = false;
                SaveLocalResourceText = Resources.MainView.SaveLocalResource;
                SaveLocalResourceIsEnabled = false;
                LoadLocalResourceText = Resources.MainView.LoadLocalResource;
                LoadLocalResourceIsEnabled = false;
                break;
            case State.SaveLocalResource:
                LoadInternetResourceText = Resources.MainView.LoadInternetResource;
                LoadInternetResourceIsEnabled = false;
                SaveLocalResourceText = Resources.MainView.Saving;
                SaveLocalResourceIsEnabled = false;
                LoadLocalResourceText = Resources.MainView.LoadLocalResource;
                LoadLocalResourceIsEnabled = false;
                break;
            case State.LoadLocalResource:
                LoadInternetResourceText = Resources.MainView.LoadInternetResource;
                LoadInternetResourceIsEnabled = false;
                SaveLocalResourceText = Resources.MainView.SaveLocalResource;
                SaveLocalResourceIsEnabled = false;
                LoadLocalResourceText = Resources.MainView.Loading;
                LoadLocalResourceIsEnabled = false;
                break;
            default:
                throw new ArgumentException($"Unexpected \"{nameof(State)}\": \"{state}\".", nameof(state));
        }
    }

    public enum State
    {
        Ready,
        LoadingInternetResource,
        SaveLocalResource,
        LoadLocalResource,
    }

    [ObservableProperty]
    private bool _loadInternetResourceIsEnabled = default;

    [ObservableProperty]
    private string _loadInternetResourceText = "";

    [ObservableProperty]
    private bool _loadLocalResourceIsEnabled = default;

    [ObservableProperty]
    private string _loadLocalResourceText = "";

    [ObservableProperty]
    private string _resourceContentText = "";

    [ObservableProperty]
    private string _resourceNameText = "";

    [ObservableProperty]
    private bool _saveLocalResourceIsEnabled = default;

    [ObservableProperty]
    private string _saveLocalResourceText = "";

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
