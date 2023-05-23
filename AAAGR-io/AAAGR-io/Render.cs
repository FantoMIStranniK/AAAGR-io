using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace AAAGR_io
{
    public static class Render
    {
        public static RenderWindow window;
        public static uint wantedFrameRate = 144;

        public static uint width = 1600;
        public static uint height = 900;

        private static Text Scores = new Text();

        public static void InitRender()
        {
            window = new RenderWindow(new VideoMode(width, height), "Game window");

            window.SetFramerateLimit(wantedFrameRate);
        }
        public static void RenderWindow(Dictionary<string, GameObject> gameObjects)
        {
            window.Clear(Color.White);

            window.DispatchEvents();

            DrawGameObjects(gameObjects);

            window.Display();
        }
        public static void TryClose()
        {
            window.Closed += WindowClosed;
        }
        private static void DrawGameObjects(Dictionary<string, GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects.Values)
            {
                window.Draw(gameObject.UniversalShape);
            }

            window.Draw(Scores);
        }
        private static void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
    }
}
