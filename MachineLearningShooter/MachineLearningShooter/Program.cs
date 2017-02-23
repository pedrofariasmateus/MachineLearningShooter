using System;

namespace MachineLearningShooter
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Jogo game = new Jogo())
            {
                game.Run();
            }
        }
    }
#endif
}

