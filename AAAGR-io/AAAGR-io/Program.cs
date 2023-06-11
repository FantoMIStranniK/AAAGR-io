
using AAAGR_io.Engine;

namespace AAAGR_io
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameLoop gameLoop = GameLoop.InitGameLoop();

            gameLoop.LaunchGame();
        }
    }
}