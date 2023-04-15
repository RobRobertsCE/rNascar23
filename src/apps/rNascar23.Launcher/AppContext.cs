using Microsoft.IO;
using rNascar23.Patches.AppRegistry;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace rNascar23.Launcher
{
    internal class AppContext : ApplicationContext
    {
        private const string LauncherDirectory = "Launcher";

        Process _process = null;

        public AppContext()
        {
            var installFolder = RegistryHelper.GetInstallFolder();

            var splash = new SplashScreen();

            splash.ShowDialog();

            if (splash.PatchVersion != null)
            {
                var patcherExePath = Path.Combine(installFolder, $"{LauncherDirectory}\\rNascar23.Patcher.exe");

                var process = new Process();

                process.StartInfo.FileName = patcherExePath;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                process.StartInfo.Arguments = splash.PatchVersion.ToString();
                process.StartInfo.WorkingDirectory = installFolder;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.Verb = "runas";

                process.Start();
                process.WaitForExit();
            }

            var exePath = Path.Combine(installFolder, "rNascar23.exe");

            _process = new Process();

            _process.StartInfo.FileName = exePath;
            _process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            _process.EnableRaisingEvents = true;
            _process.Exited += Process_Exited;

            _process.Start();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            ExitThread();
        }
    }
}
