using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Ping_pong
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
        public static void RenderWindow(List<GameObject> gameObjects)
        {
            window.Clear(Color.Black);

            window.DispatchEvents();

            DrawGameObjects(gameObjects);

            window.Display();
        }
        public static void TryClose()
        {
            window.Closed += WindowClosed;
        }
        private static void DrawGameObjects(List<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                window.Draw(gameObject.DrawableShape);
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
