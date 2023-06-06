using AAAGR_io.Engine.GameObjects;
using AAAGR_io.Engine.Input;
using AAAGR_io.GameAssets;
using SFML.Graphics;

namespace AAAGR_io.Engine
{

    public class GameObjectsList
    {
        public List<ListedGameObject> GameObjects = new List<ListedGameObject>();

        public PlayerController MainPlayerController;

        public List<PlayerController> Bots = new List<PlayerController>();

        private List<GameObject> GameObjectsToDelete = new List<GameObject>();    

        //Need values
        private int neededFoodCount = 0;
        private int neededPlayerCount = 0;

        private bool needForPlayer = false;

        //Start values
        private int countOfFoodPoints = 35;
        private int countOfOtherPlayers = 5;

        private bool initedPlayers = false; 

        private Random rand = new Random();

        public void InitSpawn()
        {
            //Create main player
            CreatePlayer(false, Color.Magenta, 1.5f + rand.NextSingle());

            for(int i = 0; i < countOfFoodPoints; i++)
                SpawnFood();

            for(int i = 0; i < countOfOtherPlayers; i++)
                CreatePlayer(true, GetRandomColor(), 1.5f + rand.NextSingle());

            initedPlayers = true;
        }
        public void OnFoodDecreased()
            => neededFoodCount++;
        public void OnPlayerDecreased()
            => neededPlayerCount++;
        public void OnMainPlayerLoss()
            => needForPlayer = true;
        public void TryAddNeededGameObjects()
        {
            for (int i = 0; i < neededFoodCount; i++)
                SpawnFood();

            neededFoodCount = 0;

            for (int i = 0; i < neededPlayerCount; i++)
                CreatePlayer(true, GetRandomColor(), 1.5f + rand.NextSingle());

            neededPlayerCount = 0;

            if(needForPlayer)
            {
                CreatePlayer(false, Color.Magenta, 1.5f + rand.NextSingle());
                needForPlayer = false;
            }

            AwakeGameObjects();
        }
        public void AddObjectToDestroyQueue(string Id)
        {
            for(int i = 0; i < GameObjects.Count; i++)
            {
                if (GameObjects[i].GameObjectPair.Item1 == Id)
                {
                    GameObjectsToDelete.Add(GameObjects[i].GameObjectPair.Item2);
                    return;
                }
            }
        }
        public void DeleteGameObjects()
        {      
            foreach(var gameObjectToDelete in GameObjectsToDelete)
            {
                foreach(var gameObject in GameObjects)
                {
                    if (gameObject.GameObjectPair.Item2 == gameObjectToDelete)
                    {
                        gameObject.GameObjectPair.Item2.Destroy();
                        GameObjects.Remove(gameObject);
                        break;
                    }
                }
            }

            GameObjectsToDelete.Clear();
        }
        public List<ListedGameObject> GetPlayerList()
        {
            List<ListedGameObject> players = new List<ListedGameObject>();  

            foreach(var gameObjectPair in GameObjects)
            {
                if (gameObjectPair.GameObjectPair.Item2.tag == "Eater")
                    players.Add(gameObjectPair);
            }

            return players;
        }
        private void SpawnFood()
        {
            int foodCordX = rand.Next(75, (int)Render.width - 75);
            int foodCordY = rand.Next(75, (int)Render.height - 75);

            string foodName = "Food" + FreeNames.GetFreeFoodIndex().ToString();

            var newFood = new Food(foodCordX, foodCordY, 0.5f, foodName);

            GameObjects.Add(new ListedGameObject(foodName, newFood));
        }
        private Color GetRandomColor()
        {
            List<Color> colors = new List<Color>()
            {
                Color.Black,
                Color.Red,
                Color.Green,
                Color.Blue,
                Color.Yellow,
                Color.Cyan,
            };

            return colors[rand.Next(colors.Count)];
        }
        private void CreatePlayer(bool isAI, Color color, float mass)
        {
            string playerName = "player" + FreeNames.GetFreePlayerIndex().ToString();

            Eater player = new Eater(isAI, mass, playerName, color);

            PlayerController controller;

            if(!initedPlayers)
            {
                controller = new PlayerController(isAI);

                controller.SetNewGameObject(ref player);

                player.MyController = controller;

                if (isAI)
                    Bots.Add(controller);
                else
                    MainPlayerController = controller;
            }
            else
            {
                PlayerController? freeController;

                if (isAI)
                {
                    freeController = NulledBotsController();

                    if (freeController == null)
                        return;
                    else
                        player.MyController = freeController;
                }
                else
                {
                    player.MyController = MainPlayerController;
                }
            }

            GameObjects.Add(new ListedGameObject(playerName, player));
        }
        private PlayerController? NulledBotsController()
        {
            foreach(var controller in Bots)
            {
                if(controller.ControlledGameObject == null)
                    return controller;
            }

            return null;
        }
        public void AwakeGameObjects()
        {
            foreach (var gameObject in GameObjects)
                gameObject.GameObjectPair.Item2.Awake();
        }
    }
}
