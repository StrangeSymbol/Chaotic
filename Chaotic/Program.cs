using System;
using System.Windows.Forms;

namespace Chaotic
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameMenu game = new GameMenu())
            {
                game.Run();
            }
        }
    }
#endif
}

