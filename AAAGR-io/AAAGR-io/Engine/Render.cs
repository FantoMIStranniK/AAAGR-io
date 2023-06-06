using AAAGR_io.Engine.GameObjects;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AAAGR_io.Engine
{
    public static class Render
    {
        public static RenderWindow window;
        public static uint wantedFrameRate = 144;

        public static uint width = 1600;
        public static uint height = 900;

        //Text
        private static string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;

        private static Font font;

        private static Text scoreText;
        private static Text massText;

        public static void InitRender()
        {
            window = new RenderWindow(new VideoMode(width, height), "Game window");

            window.SetFramerateLimit(wantedFrameRate);

            font = new Font(projectDirectory + @"\Fonts\Oswald-Medium.ttf");

            scoreText = new Text("Score: 0", font);
            massText = new Text("Mass: 0", font);

            scoreText.Position = new Vector2f(20, 20);
            massText.Position = new Vector2f(width - 100, 20);

            massText.FillColor = Color.Black;
            massText.OutlineColor = Color.Red;

            scoreText.FillColor = Color.Black;
            scoreText.OutlineColor = Color.Red;
        }
        public static void RenderWindow(List<ListedGameObject> gameObjects)
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
        public static void UpdateMassText(float mass)
            => massText = massText.UpdateText($"Mass: {Math.Round(mass, 2)}", new Vector2f(width - 160, 20));
        public static void UpdateScoreText(float score)
            => scoreText = scoreText.UpdateText ($"Score: {score}", new Vector2f(20, 20));
        private static Text UpdateText(this Text text, string message, Vector2f position)
        {
            text = new Text(message, font);
            text.Position = position;

            text.FillColor = Color.Black;
            text.OutlineColor = Color.Red;

            return text;
        }
        private static void DrawGameObjects(List<ListedGameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                window.Draw(gameObject.GameObjectPair.Item2.UniversalShape);
            }

            window.Draw(massText);
            window.Draw(scoreText);
        }
        private static void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
    }
}
