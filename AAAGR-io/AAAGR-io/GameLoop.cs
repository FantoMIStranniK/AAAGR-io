
namespace AAAGR_io
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
            UpdateGameObjects();

            GetInput();

            Render.RenderWindow(game.GameObjectsList.GameObjects);
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
        private void GetInput()
        {
            foreach (var gameObject in game.GameObjectsList.GameObjects)
                gameObject.GameObjectPair.Item2.GetInput();
        }
    }
}
