using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaPrototype.Services;
using AvaloniaPrototype.ViewModels;
using AvaloniaPrototype.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace AvaloniaPrototype;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Load configuration.
        // Note that appsettings.json must be configured as an embedded resource for the build action in order to be accessible across all platforms.
        Settings settings;
        var appSettingsEmbeddedResourceFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.appsettings.json";
        using (var configurationStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(appSettingsEmbeddedResourceFileName))
        {
            var builder = new ConfigurationBuilder()
                .AddJsonStream(configurationStream ?? throw new InvalidOperationException($"The embedded resource \"{appSettingsEmbeddedResourceFileName}\" was not found."));

            settings = builder.Build().GetSection(nameof(Settings)).Get<Settings>()
                ?? throw new InvalidOperationException($"\"{appSettingsEmbeddedResourceFileName}\" is missing a \"{nameof(Settings)}\" section.");
        }

        // configure dependency injection
        var services = new ServiceCollection();
        ConfigureServices(services, settings);
        var serviceProvider = services.BuildServiceProvider();

        // configure Avalonia app main window
        var mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel,
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = mainViewModel,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    internal static void ConfigureServices(IServiceCollection services, Settings settings)
    {
        services.AddLocalization(options => options.ResourcesPath = "/Resources");
        services.AddSingleton(settings);
        services.AddTransient<MainViewModel>();
        services.AddSingleton<IAquiferService, AquiferService>();
        services.AddHttpClient<IAquiferService, AquiferService>(
            client =>
            {
                client.BaseAddress = new Uri(settings.AquiferEndpoint);
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue(Assembly.GetExecutingAssembly().GetName().Name ?? "", Assembly.GetExecutingAssembly().GetName().Version?.ToString())));
                // client.DefaultRequestHeaders.Add("api-key", ""); // only needed for non-public API?
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(GetRetryPolicy());
    }
    private static AsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(0.5), retryCount: 2));
    }
}
