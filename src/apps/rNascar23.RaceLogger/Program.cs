using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rNascar23.Sdk;
using rNascar23.Sdk.Service.Flags;
using rNascar23.Sdk.Service.LapTimes;
using rNascar23.Sdk.Service.LiveFeeds;
using rNascar23.Sdk.Service.LoopData;
using rNascar23.Sdk.Service.PitStops;
using rNascar23.Sdk.Service.Points;
using rNascar23.Settings;
using Serilog;
using System;
using System.IO;
using System.Windows.Forms;

namespace rNascar23.RaceLogger
{
    internal static class Program
    {
        public static IServiceProvider Services { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var host = CreateHostBuilder().Build();
            Services = host.Services;
            Application.Run(Services.GetRequiredService<Logger>());
        }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddrNascar23Sdk()
                        .AddTransient<Logger>();

                    /*
                    SERILOG (Set in appsettings.json):
                    Level	Usage
                    Verbose	Verbose is the noisiest level, rarely (if ever) enabled for a production app.
                    Debug	Debug is used for internal system events that are not necessarily observable from the outside, but useful when determining how something happened.
                    Information	Information events describe things happening in the system that correspond to its responsibilities and functions. Generally these are the observable actions the system can perform.
                    Warning	When service is degraded, endangered, or may be behaving outside of its expected parameters, Warning level events are used.
                    Error	When functionality is unavailable or expectations broken, an Error event is used.
                    Fatal	The most critical level, Fatal events demand immediate attention.
                    */
                    var configuration = new ConfigurationBuilder()
                        .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                        .Build();

                    var userSettings = UserSettingsService.LoadUserSettings();

                    var logFilePath = Path.Combine($"{userSettings.LogDirectory}\\", "rNascar23.Logger.Log..txt");

                    configuration["Serilog:WriteTo:1:Args:path"] = logFilePath;

                    var serilogLogger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger();

                    services.AddLogging(x =>
                    {
                        x.AddSerilog(logger: serilogLogger, dispose: true);
                    });
                });
        }
    }
}
