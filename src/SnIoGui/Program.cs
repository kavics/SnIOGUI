using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SenseNet.Extensions.DependencyInjection;
using SnIoGui.Services;
using System;
using System.Windows.Forms;

namespace SnIoGui
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            var services = new ServiceCollection();
            
            // Register RuntimeSettingsManager as singleton
            services.AddSingleton<IRuntimeSettingsManager, RuntimeSettingsManager>();
            
            // Register settings accessor
            services.AddSingleton(sp => 
            {
                var settingsManager = sp.GetRequiredService<IRuntimeSettingsManager>();
                return Options.Create(settingsManager.Settings);
            });

            // Register HttpClient and health service
            services.AddSingleton<HttpClient>();
            services.AddScoped<IHealthService, HealthService>();
            
            // Register SenseNet clients BEFORE building ServiceProvider
            // Create a temporary provider to access settings
            var tempProvider = services.BuildServiceProvider();
            RegisterSenseNetClient(services, tempProvider);
            
            // Register forms AFTER SenseNet clients are registered
            services.AddSingleton<Form1>();
            services.AddSingleton<Form2>();
            services.AddTransient<SettingsEditorForm>();
            
            // Build the final ServiceProvider with all registrations
            ServiceProvider = services.BuildServiceProvider();

            ApplicationConfiguration.Initialize();
            var form = ServiceProvider.GetRequiredService<Form1>();

            Application.Run(form);
        }

        private static void RegisterSenseNetClient(ServiceCollection services, IServiceProvider tempProvider)
        {
            var settingsManager = tempProvider.GetRequiredService<IRuntimeSettingsManager>();
            var senseNetClientBuilder = services.AddSenseNetClient();

            // Get the targets from settings
            var targets = settingsManager.Settings.Targets;
            
            if (targets != null)
            {
                // Register each target as a SenseNet repository
                foreach (var target in targets)
                {
                    if (!string.IsNullOrWhiteSpace(target.Name) && 
                        !string.IsNullOrWhiteSpace(target.Url) && 
                        !string.IsNullOrWhiteSpace(target.ApiKey))
                    {
                        senseNetClientBuilder.ConfigureSenseNetRepository(
                            target.Name,
                            repositoryOptions =>
                            {
                                repositoryOptions.Url = target.Url;
                                repositoryOptions.Authentication.ApiKey = target.ApiKey;
                            },
                            registeredContentTypes => { });
                    }
                }
            }
        }
    }
}