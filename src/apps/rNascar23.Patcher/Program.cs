using System;
using System.Windows.Forms;

namespace rNascar23.Patcher
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Version version = args.Length > 0 ? new Version(args[0]) : new Version();

            Application.Run(new Patcher(version));
        }
    }
}
