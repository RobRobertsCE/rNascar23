using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rNascar23.Data.LiveFeeds.Ports;
using rNascar23.Service.LiveFeeds;
using System;
using System.Windows.Forms;
using AutoMapper;
using rNascar23.Service.Flags;
using Microsoft.Extensions.Logging;
using Serilog;
using rNascar23.Service.LapTimes;

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
                        .AddTransient<MainForm>();

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
