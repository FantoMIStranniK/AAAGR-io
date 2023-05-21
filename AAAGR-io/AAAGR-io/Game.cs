using SFML.Graphics;
using SFML.Window;

namespace Ping_pong
{
    public class Game
    {
        private Ball ball;
        private Player player1;
        private Player player2;

        private List<GameObject> gameObjects = new List<GameObject>();
        private List<Player> players = new List<Player>(); 

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

            Render.RenderWindow(gameObjects, player1.Score, player2.Score);
        }

        #region Init
        private void Init()
        {
            Render.InitRender();

            Time.Start();

            ball = new Ball();
            player1 = new Player(false, (int)Render.width);
            player2 = new Player(false, (int)Render.width);

            Border left = new Border(25, (int)Render.height);
            Border right = new Border(25, (int)Render.height);

            left.SetPosition(0, (int)Render.height);
            right.SetPosition((int)Render.width, (int)Render.height);

            
            //Make gameobjects list
            players = new List<Player> { player1, player2 };

            gameObjects = new List<GameObject>()
            {
                ball,
                player1, 
                player2,
                left,
                right,
            };

            //Awaken my masters!
            foreach(var gameObject in gameObjects)
                gameObject.Awake();

            player2.padleShape.Position = new SFML.System.Vector2f((int)Render.width / 2, Render.height - 120);
        }
        #endregion

        #region Input
        private void GetInput()
        {
            foreach(var player in players)
                player.GetInput();
        }
        #endregion

        #region Update
        private void CheckBallPosition()
        {
            if (BallIsOutOfUpperBounds() || BallIsOutOfBottomBounds())
            {
                if (BallIsOutOfUpperBounds())
                    player2.AddScore();
                if(BallIsOutOfBottomBounds())
                    player1.AddScore();

                ball.ResetBall(Render.width / 2f, Render.height / 2f);
            }
        }
        private bool BallIsOutOfUpperBounds()
            => ball.Shape.Position.Y + ball.Shape.Radius >= Render.height - 100;
        private bool BallIsOutOfBottomBounds()
            =>  ball.Shape.Position.Y - ball.Shape.Radius <= 100;
        private void UpdateGameObjects()
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Update();

                if (gameObject.tag is not "ball")
                    CheckCollisionWithBall(gameObject);
            }

            CheckBallPosition();
        }
        private void CheckCollisionWithBall(GameObject collideable)
            => ball.TryChangeDirectionOnCollision(collideable);
        #endregion
    }
}
