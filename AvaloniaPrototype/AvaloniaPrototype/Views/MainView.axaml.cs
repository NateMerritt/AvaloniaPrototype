using System;
using System.Globalization;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using AvaloniaPrototype.ViewModels;

namespace AvaloniaPrototype.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        AttachedToVisualTree += MainView_AttachedToVisualTree;
    }
    private void MainView_AttachedToVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
    {
        CultureComboBox.SelectedIndex = Thread.CurrentThread.CurrentUICulture.Name switch
        {
            "en-US" => 0,
            "es-ES" => 1,
            _ => throw new NotImplementedException($"Unexpected {nameof(Thread.CurrentThread)}.{nameof(Thread.CurrentThread.CurrentUICulture)}: {Thread.CurrentThread.CurrentUICulture.Name}."),
        };
    }

    private void CultureComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        var comboBox = sender as ComboBox
            ?? throw new InvalidOperationException($"Could not cast {nameof(sender)}.");

        // TODO bind CultureInfo instead like this: https://docs.avaloniaui.net/docs/0.10.x/controls/combobox
        var selectedCultureInfo = comboBox.SelectedIndex switch
        {
            0 => CultureInfo.GetCultureInfo("en-US"),
            1 => CultureInfo.GetCultureInfo("es-ES"),
            _ => throw new InvalidOperationException($"Unexpected {nameof(CultureComboBox)}.{nameof(CultureComboBox.SelectedIndex)}: {CultureComboBox.SelectedIndex}."),
        };

        if (Thread.CurrentThread.CurrentUICulture != selectedCultureInfo)
        {
            // Reload the view to force refresh the relevant culture text.
            // TODO is there a better way to do this?
            Thread.CurrentThread.CurrentUICulture = selectedCultureInfo;
            AvaloniaXamlLoader.Load(this);

            // view model can be null during initialization
            var viewModel = DataContext as MainViewModel;
            viewModel?.SetState(MainViewModel.State.Ready);
        }
    }

    private async void LoadInternetResourceButton_Click(object sender, RoutedEventArgs args)
    {
        var viewModel = DataContext as MainViewModel
            ?? throw new InvalidOperationException($"Could not load {nameof(DataContext)}.");

        viewModel.SetState(MainViewModel.State.LoadingInternetResource);
        await Dispatcher.UIThread.InvokeAsync(viewModel.GetRandomResourceContentAsync, DispatcherPriority.Background);
        viewModel.SetState(MainViewModel.State.Ready);
    }

    private async void LoadLocalResourceButton_Click(object sender, RoutedEventArgs args)
    {
        var viewModel = DataContext as MainViewModel
            ?? throw new InvalidOperationException($"Could not load {nameof(DataContext)}.");

        viewModel.SetState(MainViewModel.State.LoadLocalResource);
        await Dispatcher.UIThread.InvokeAsync(viewModel.GetRandomResourceContentAsync, DispatcherPriority.Background);
        viewModel.SetState(MainViewModel.State.Ready);
    }

    private async void SaveLocalResourceButton_Click(object sender, RoutedEventArgs args)
    {
        var viewModel = DataContext as MainViewModel
            ?? throw new InvalidOperationException($"Could not load {nameof(DataContext)}.");

        viewModel.SetState(MainViewModel.State.SaveLocalResource);
        await Dispatcher.UIThread.InvokeAsync(viewModel.GetRandomResourceContentAsync, DispatcherPriority.Background);
        viewModel.SetState(MainViewModel.State.Ready);
    }
}