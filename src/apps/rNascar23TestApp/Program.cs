using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using rNascar23.Service.DriverStatistics;
using rNascar23.Service.Flags;
using rNascar23.Service.LapTimes;
using rNascar23.Service.LiveFeeds;
using rNascar23.Service.Points;
using rNascar23TestApp.CustomViews;
using rNascar23TestApp.Dialogs;
using Serilog;
using System;
using System.Windows.Forms;

namespace rNascar23TestApp
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
                        .AddRaceLists()
                        .AddLiveFeed()
                        .AddLapTimes()
                        .AddPoints()
                        .AddDriverStatistics()
                        .AddTransient<MainForm>()
                        .AddTransient<GridSettingsDialog>()
                        .AddTransient<CustomGridViewFactory>()
                        .AddTransient<CustomViewSettingsService>();                    

                    //Add Serilog
                    var serilogLogger = new LoggerConfiguration()
                    .WriteTo.File("rNascar23.LogFile.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                    .CreateLogger();
                    services.AddLogging(x =>
                    {
                        x.SetMinimumLevel(LogLevel.Information);
                        x.AddSerilog(logger: serilogLogger, dispose: true);
                    });
                });
        }
    }
}
