using SFML.Graphics;
using System.Runtime.CompilerServices;

namespace AAAGR_io
{

    public class GameObjectsList
    {
        public List<ListedGameObject> GameObjects = new List<ListedGameObject>();

        public string CurrentPlayerName;

        private List<GameObject> GameObjectsToDelete = new List<GameObject>();    

        //Need values
        private int neededFoodCount = 0;
        private int neededPlayerCount = 0;

        private bool needForPlayer = false;
        private bool initedPlayers = false;

        //Start values
        private int countOfFoodPoints = 35;
        private int countOfOtherPlayers = 5;

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
        #region Events
        public void OnFoodDecreased()
            => neededFoodCount++;
        public void OnPlayerDecreased()
            => neededPlayerCount++;
        public void OnMainPlayerLoss()
            => needForPlayer = true;
        #endregion

        #region Deletion
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
                        RemoveGameObject(gameObject);
                        break;
                    }
                }
            }

            GameObjectsToDelete.Clear();
        }
        private void RemoveGameObject(ListedGameObject listedGameObject)
        {
            var gameObject = listedGameObject.GameObjectPair.Item2;

            var controller = FindControllerByObject(listedGameObject);

            controller?.ResetGameObject();

            gameObject.Destroy();
            GameObjects.Remove(listedGameObject);

        }
        private PlayerController? FindControllerByObject(ListedGameObject? listedGameObject)
        {
            var controllers = Game.Instance.PlayerControllers;

            foreach(var controller in controllers)
            {
                if (controller.MyListedGameObject == null)
                    return null;

                if(controller.MyListedGameObject == listedGameObject)
                    return controller;
            }

            return null;
        }
        #endregion

        #region Addition
        public void TryAddNeededGameObjects()
        {
            for (int i = 0; i < neededFoodCount; i++)
                SpawnFood();

            neededFoodCount = 0;

            for (int i = 0; i < neededPlayerCount; i++)
            {
                var player = CreatePlayer(true, GetRandomColor(), 1.5f + rand.NextSingle());
                TryAddNewController(player);
            }

            neededPlayerCount = 0;

            if (needForPlayer)
            {
                CreatePlayer(false, Color.Magenta, 1.5f + rand.NextSingle());
                needForPlayer = false;
            }

            AwakeGameObjects();
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
        private void TryAddNewController(ListedGameObject listedGameObject)
        {
            foreach(var controller in  Game.Instance.PlayerControllers)
            {
                if (controller.MyListedGameObject is null && controller.IsAi)
                    controller.SetNewGameObject(listedGameObject);
            }
        }
        #endregion

        #region Spawning
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
        private ListedGameObject CreatePlayer(bool isAI, Color color, float mass)
        {
            string playerName = "player" + FreeNames.GetFreePlayerIndex().ToString();

            if(!isAI)
                CurrentPlayerName = playerName;

            Eater player = new Eater(isAI, mass, playerName, color);

            var newPlayer = new ListedGameObject(playerName, player);

            GameObjects.Add(newPlayer);

            if (initedPlayers) ;
                ProcessNullControllers(newPlayer);

            return newPlayer; 
        }
        private void ProcessNullControllers(ListedGameObject listedGameObject)
        {
            var controllers = Game.Instance.PlayerControllers;

            foreach(var controller in controllers)
            {
                if(controller.MyListedGameObject == null)
                {
                    controller.SetNewGameObject(listedGameObject);
                    return;
                }
            }
        }
        #endregion

        public void AwakeGameObjects()
        {
            foreach (var gameObject in GameObjects)
                gameObject.GameObjectPair.Item2.Awake();
        }
    }
}
