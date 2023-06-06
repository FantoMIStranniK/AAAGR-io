using SFML.Window;

namespace AAAGR_io.Engine
{
    public class GameLoop
    {
        private Game game = new Game();

        public void LaunchGame()
        {
            game.InitGame();

            Render.window.KeyPressed += HumanInput;

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

            BotsInput();

            UpdateGameObjects();

            Render.RenderWindow(game.GameObjectsList.GameObjects);
        }
        private void BotsInput()
        {
            foreach (var controller in Game.Instance.GameObjectsList.Bots)
            {
                controller.AiInput();
            }
        }
        private void HumanInput(object sender, KeyEventArgs e)
        {
            Game.Instance.GameObjectsList.MainPlayerController.HumanInput(e);
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
    }
}
