using AAAGR_io;

namespace AAAGR_io
{
    public class Game
    {
        public static Game Instance { get; private set; }

        public GameObjectsList GameObjectsList { get; private set; } = new GameObjectsList();

        public List<PlayerController> PlayerControllers  {get; private set; } = new List<PlayerController>();

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

        #region Init
        public void InitGame()
        {
            Instance = this;

            Render.InitRender();

            Time.StartTime();

            GameObjectsList.InitSpawn();

            InitPlayerControllers();

            GameObjectsList.AwakeGameObjects();

            AwakeControllers();
        }
        private void AwakeControllers()
        {
            foreach(var controller in PlayerControllers)
            {
                controller.AwakeController();
            }
        }
        private void InitPlayerControllers()
        {
            var players = GameObjectsList.GetPlayerList();

            foreach(var player in players)
            {
                bool isAi = true;

                if(player.GameObjectPair.Item1 == GameObjectsList.CurrentPlayerName)
                    isAi = false;

                PlayerControllers.Add(new PlayerController(player, isAi));
            }
        }
        #endregion

        public void OnObjectDeath(string Id)
        {
            GameObjectsList.AddObjectToDestroyQueue(Id);
        }
        public void AddScore(int addition)
            => Score += addition;
    }
}
