using System;

namespace Umbra
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Engines.Main Umbra = new Engines.Main())
            {
                Umbra.Run();
            }
        }
    }
#endif
}

