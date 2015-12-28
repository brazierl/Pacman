using System;

namespace pacman
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Pacman game = new Pacman())
            {
                game.Run();
            }
        }
    }
#endif
}

