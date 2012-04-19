using System;

namespace Software_Development_I
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game3 game = new Game3())
            {
                //Menu stuff here?

                game.Run();
            }
        }
    }
#endif
}

