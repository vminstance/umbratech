using System;
using System.Windows.Forms;

namespace Umbra
{
#if WINDOWS || XBOX
    static class Program
    {
        static public bool CodeClose = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Umbra.Definitions.Globals.Constants.Launcher.Enabled)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Form1 Launcher = new Form1();
                Application.Run(Launcher);
            }
            else
            {
                CodeClose = true;
            }

            if (CodeClose)
            {
                Engines.Main UmbraEngine = new Engines.Main();
                UmbraEngine.Run();
            }
        }
    }
#endif
}

