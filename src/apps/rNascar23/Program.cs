using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rNascar23.Common;
using rNascar23.Configuration;
using rNascar23.CustomViews;
using rNascar23.Dialogs;
using rNascar23.Service.Flags;
using rNascar23.Service.LapTimes;
using rNascar23.Service.LiveFeeds;
using rNascar23.Service.LoopData;
using rNascar23.Service.Media;
using rNascar23.Service.PitStops;
using rNascar23.Service.Points;
using Serilog;
using System;
using System.IO;
using System.Windows.Forms;

namespace rNascar23
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
            Application.Run(Services.GetRequiredService<MainForm>());
        }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                        .AddFlagState()
                        .AddSchedules()
                        .AddLiveFeed()
                        .AddLapTimes()
                        .AddPoints()
                        .AddLoopData()
                        .AddPitStops()
                        .AddMedia()                       
                        .AddTransient<MainForm>()
                        .AddTransient<UserSettingsDialog>()
                        .AddTransient<ReplaySelectionDialog>()
                        .AddTransient<AudioPlayer>()
                        .AddTransient<VideoPlayer>()
                        .AddTransient<MultiView>();

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

                    var logFilePath = Path.Combine($"{userSettings.LogDirectory}\\", "rNascar23Log..txt");

                    var appSettingsFilePath = Path.Combine($"{userSettings.DataDirectory}", "appsettings.json");

                    if (File.Exists(appSettingsFilePath))
                    {
                        var userConfiguration = new ConfigurationBuilder()
                           .AddJsonFile(path: appSettingsFilePath, optional: false, reloadOnChange: true)
                           .Build();

                        services.Configure<Features>(userConfiguration.GetSection("Features"));
                    }
                    else
                    {
                        services.Configure<Features>(configuration.GetSection("Features"));
                    }

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
