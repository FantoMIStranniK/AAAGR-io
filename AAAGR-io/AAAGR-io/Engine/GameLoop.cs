using SFML.Window;
using System.Diagnostics;
using System.Reflection;

namespace AAAGR_io.Engine
{
    public class GameLoop
    {
        private Game game = new Game();
        public void LaunchGame()
        {
            game.InitGame();

            while (Render.window.IsOpen)
            {
                Time.UpdateSystemTime();

                if (Time.totalTimeBeforeUpdate >= 1 / Render.wantedFrameRate)
                {
                    Time.ResetTimeBeforeUpdate();

                    DoGameStep();

                    Time.UpdateTime();
                }

                Render.TryClose();
            }
        }
        private void DoGameStep()
        {
            Render.window.DispatchEvents();

            GetInput();

            UpdateGameObjects();

            Render.RenderWindow(game.GameObjectsList.GameObjects);
        }
        private void GetInput()
        {
            foreach (var controller in Game.Instance.GameObjectsList.PlayerControllers)
            {
                controller.GetInput();
            }
        }
        private void UpdateGameObjects()
        {
            game.GameObjectsList.TryAddNeededGameObjects();

            game.GameObjectsList.DeleteGameObjects();

            foreach (var gameObject in game.GameObjectsList.GameObjects)
            {
                gameObject.GameObjectPair.Item2.Update();
            }

            //Check collisions
            foreach (var colliding in game.GameObjectsList.GameObjects)
            {
                foreach (var collideable in game.GameObjectsList.GameObjects)
                {
                    if (colliding.GameObjectPair == collideable.GameObjectPair)
                        continue;

                    collideable.GameObjectPair.Item2.TryEat(collideable.GameObjectPair.Item2.UniversalShape, colliding.GameObjectPair.Item2);
                    colliding.GameObjectPair.Item2.TryEat(colliding.GameObjectPair.Item2.UniversalShape, collideable.GameObjectPair.Item2);
                }
            }

            game.GameObjectsList.DeleteGameObjects();
        }
        public static GameLoop? InitGameLoop()
        {
            GameLoop gameLoop = new GameLoop();

            string pathToDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\AAAGR.io";

            if (!File.Exists(pathToDocuments + @"\config.txt"))
                return gameLoop;

            using (StreamReader sr = new StreamReader(pathToDocuments + @"\config.txt"))
            {
                while (!sr.EndOfStream)
                {
                    var input = sr.ReadLine().Split('=');

                    if (input.Length >= 2)
                    {
                        string name = input[0];

                        if (uint.TryParse(input[1], out uint value))
                        {
                            Type type = typeof(Render);

                            var piShared = type.GetField(name);

                            piShared?.SetValue(null, value);
                        }
                    }
                }
            }

            return gameLoop;
        }
    }
}
