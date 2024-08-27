using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using AvaloniaPrototype.ViewModels;

namespace AvaloniaPrototype.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private async void LoadResourceButton_Click(object source, RoutedEventArgs args)
    {
        var viewModel = (MainViewModel) (DataContext ?? throw new InvalidOperationException($"Could not load {nameof(DataContext)}."));

        LoadResourceButton.IsEnabled = false;
        LoadResourceButton.Content = viewModel.LoadingText;

        var result = await Dispatcher.UIThread.InvokeAsync(viewModel.GetRandomResourceContentAsync, DispatcherPriority.Background);

        ResourceNameTextBlock.Text = result.Name;
        ResourceContentTextBlock.Text = result.Content;

        LoadResourceButton.Content = viewModel.LoadResourceButtonText;
        LoadResourceButton.IsEnabled = true;
    }
}