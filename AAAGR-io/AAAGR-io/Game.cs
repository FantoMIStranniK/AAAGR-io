using SFML.Graphics;
using SFML.Window;

namespace Ping_pong
{
    public class Game
    {
        private List<GameObject> gameObjects = new List<GameObject>();

        public void StartGame()
        {
            Init();

            while (Render.window.IsOpen) 
            {
                Time.UpdateSystemTime();

                if (Time.totalTimeBeforeUpdate >= 1/Render.wantedFrameRate)
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
            Time.UpdateTime();

            GetInput();

            UpdateGameObjects();

            Render.RenderWindow(gameObjects);
        }

        #region Init
        private void Init()
        {
            Render.InitRender();

            Time.Start();
            
            gameObjects = new List<GameObject>()
            {

            };

            //Awaken my masters!
            foreach(var gameObject in gameObjects)
                gameObject.Awake();

        }
        #endregion

        #region Input
        private void GetInput()
        {

        }
        #endregion

        #region Update
        private void UpdateGameObjects()
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Update();
            }
        }
        #endregion
    }
}
