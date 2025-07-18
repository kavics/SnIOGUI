using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets("3ffc4eca-efea-406a-ac9b-287d104967f4")
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.Configure<SnIoGuiSettings>(configuration.GetSection("SnIoGuiSettings"));
            services.AddSingleton<Form1>();
            ServiceProvider = services.BuildServiceProvider();

            ApplicationConfiguration.Initialize();
            var form = ServiceProvider.GetRequiredService<Form1>();
            Application.Run(form);
        }
    }
}