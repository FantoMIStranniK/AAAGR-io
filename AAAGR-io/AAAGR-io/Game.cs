using AAAGR_io;

namespace AAAGR_io
{
    public class Game : GameLoop
    {
        public static Game Instance { get; private set; }

        public int Score
        {
            get
            {
                return _score;
            }
            private set 
            { 
                _score = value;
                Render.UpdateScoreText(Score);
            }
        }
        private int _score = 0;
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

            Render.RenderWindow(Spawner.Entities);
        }

        #region Init
        private void Init()
        {
            Instance = this;

            Render.InitRender();

            Time.StartTime();

            Spawner.InitSpawn();

            Spawner.AwakeGameObjects();
        }
        #endregion

        #region Input
        private void GetInput()
        {
            foreach(var gameObject in Spawner.Entities.Values)
                gameObject.GetInput();
        }
        #endregion

        #region Update
        private void UpdateGameObjects()
        {
            Spawner.TryAddNeededGameObjects();

            foreach (var gameObject in Spawner.Entities.Values)
            {
                gameObject.Update();
            }

            //Check collisions
            foreach(var colliding in Spawner.Entities.Values)
            {
                foreach(var collideable in Spawner.Entities.Values)
                {
                    if (colliding == collideable)
                        continue;

                    collideable.TryEat(collideable.UniversalShape, colliding);
                    colliding.TryEat(colliding.UniversalShape, collideable);
                }
            }
        }
        #endregion

        public void AddScore(int addition)
            => Score += addition;
    }
}
