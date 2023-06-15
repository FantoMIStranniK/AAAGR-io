
namespace AAAGR_io.Engine
{
    public class GameLoop
    {
        private Game game = new Game();

        private static string pathToDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\AAAGR.io";

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

                    Thread.Sleep(1);

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
        public static GameLoop InitGameLoop()
        {
            GameLoop gameLoop = new GameLoop();


            if (File.Exists(pathToDocuments + @"\config.txt"))
                LoadConfigs();

            return gameLoop;
        }

        #region Config loading
        private static void LoadConfigs()
        {
            using (StreamReader sr = new StreamReader(pathToDocuments + @"\config.txt"))
            {
                while (!sr.EndOfStream)
                {
                    var input = sr.ReadLine()?.Split("::");

                    if (input.Length >= 2)
                        ProcessConfigLine(input[0], input[1], input[2]);
                }
            }
        }
        private static void ProcessConfigLine(string className, string varName, string configValue)
        {
            Type type;

            switch(className)
            {
                case "Render":
                    type = typeof(Render);
                    break;
                case "GameObjectsList":
                    type = typeof(GameObjectsList);
                    break;
                default: 
                    return;
            }

            var field = type.GetField(varName);

            var varIype = Convert.GetTypeCode(field?.GetValue(null));

            var parsedValue = Convert.ChangeType(configValue, varIype);

            field?.SetValue(null, parsedValue);
        }
        #endregion
    }
}
