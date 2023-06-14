using AAAGR_io.Engine.GameObjects;
using AAAGR_io.Engine.Input;
using AAAGR_io.Game_Assets.Interfaces;
using AAAGR_io.GameAssets;
using SFML.Graphics;
using System.Diagnostics.SymbolStore;

namespace AAAGR_io.Engine
{

    public class GameObjectsList
    {
        public List<ListedGameObject> GameObjects = new List<ListedGameObject>();

        public List<PlayerController> PlayerControllers = new List<PlayerController>();

        private List<GameObject> GameObjectsToDelete = new List<GameObject>();    

        //Need values
        private  int neededFoodCount = 0;
        private int neededPlayerCount = 0;

        private bool needForPlayer = false;

        //Start values
        public static int countOfFoodPoints = 35;
        public static int countOfOtherPlayers = 5;

        private bool initedPlayers = false; 

        private Random rand = new Random();

        public void InitSpawn()
        {
            //Create main player
            ProcessPlayer(false, Color.Magenta, 1.5f + rand.NextSingle(), "MainPlayer", true, SpriteName.Skull);

            for(int i = 0; i < countOfFoodPoints; i++)
                SpawnFood();

            for(int i = 0; i < countOfOtherPlayers; i++)
                ProcessPlayer(true, GetRandomColor(), 1.5f + rand.NextSingle(), "Player" + FreeNames.GetFreePlayerIndex());

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
                ProcessPlayer(true, GetRandomColor(), 1.5f + rand.NextSingle(), "Player" + FreeNames.GetFreePlayerIndex);

            neededPlayerCount = 0;

            if(needForPlayer)
            {
                ProcessPlayer(false, Color.Magenta, 1.5f + rand.NextSingle(), "MainPlayer", true, SpriteName.Skull);
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

            string foodName = "Food" + FreeNames.GetFreeFoodIndex();

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
        private void ProcessPlayer(bool isAI, Color color, float mass, string name = "default", bool isAnimated = false, SpriteName sprite = SpriteName.None)
        {
            var player = CreatePlayer(isAI, color, mass, name, isAnimated, sprite);

            var controller = new PlayerController(isAI);

            if(initedPlayers)
                controller = GetFreeController(isAI);

            player.MyController = controller;

            ListedGameObject listedGameObject = new ListedGameObject(player.name, player);

            GameObjects.Add(listedGameObject);

            controller.SetNewGameObject((Eater)GetPlayer(player));

            if(!initedPlayers)
                PlayerControllers.Add(controller);
        }
        private Eater CreatePlayer(bool isAI, Color color, float mass, string name, bool isAnimated, SpriteName sprite)
        {
            string playerName = name;

            Eater player = new Eater(isAI, mass, playerName, color, isAnimated, sprite);

            return player;
        }
        private GameObject GetPlayer(GameObject gameObject)
        {
            foreach (var gameObject1 in GameObjects)
            {
                if (gameObject1.GameObjectPair.Item2 == gameObject)
                    return gameObject1.GameObjectPair.Item2;
            }

            return null;
        }
        private PlayerController GetFreeController(bool aiMode)
        {
            foreach(var controller in PlayerControllers)
            {
                if (controller.ControlledGameObject == null && controller.IsAi == aiMode)
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
