using AAAGR_io;

namespace AAAGR_io
{
    public class Game : GameLoop
    {
        public static Game Instance { get; private set; }

        public Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();

        public void StartGame()
        {
            Init();

            LaunchGame();
        }
        protected override void DoGameStep()
        {
            Time.UpdateTime();

            GetInput();

            UpdateGameObjects();

            Render.RenderWindow(gameObjects);
        }

        #region Init
        private void Init()
        {
            Instance = this;

            Render.InitRender();

            Time.StartTime();

            Spawn.InitSpawn();

            gameObjects = Spawn.Entities;

            //Awaken my masters!
            foreach(var gameObject in gameObjects.Values)
                gameObject.Awake();
        }
        #endregion

        #region Input
        private void GetInput()
        {
            foreach(var gameObject in gameObjects.Values)
                gameObject.GetInput();
        }
        #endregion

        #region Update
        private void UpdateGameObjects()
        {
            Spawn.TryAddFood();

            foreach (var gameObject in gameObjects.Values)
            {
                gameObject.Update();
            }

            //Check collisions
            foreach(var colliding in gameObjects.Values)
            {
                foreach(var collideable in gameObjects.Values)
                {
                    if (colliding == collideable)
                        continue;

                    collideable.TryEat(collideable.UniversalShape, colliding);
                    colliding.TryEat(colliding.UniversalShape, collideable);
                }
            } 
           
        }
        #endregion
    }
}
