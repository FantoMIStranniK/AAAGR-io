﻿using AAAGR_io.Engine.Input;

namespace AAAGR_io.Engine
{
    public class Game
    {
        public static Game Instance { get; private set; }

        public GameObjectsList GameObjectsList { get; private set; } = new GameObjectsList();

        public static string PathToProject = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

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

            GameObjectsList.AwakeGameObjects();
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
