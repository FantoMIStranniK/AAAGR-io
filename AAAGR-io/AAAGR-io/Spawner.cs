using SFML.Graphics;
using SFML.System;

namespace AAAGR_io
{

    public static class Spawner
    {
        public static Dictionary<string, GameObject> Entities =new Dictionary<string, GameObject>();

        //Need values
        private static int neededFoodCount = 0;
        private static int neededPlayerCount = 0;

        private static bool needForPlayer = false;

        //Start values
        private static int countOfFoodPoints = 35;
        private static int countOfOtherPlayers = 5;

        private static Random rand = new Random();

        public static void InitSpawn()
        {
            //Create main player
            CreatePlayer(false, Color.Magenta, 1.5f + rand.NextSingle());

            for(int i = 0; i < countOfFoodPoints; i++)
                SpawnFood();

            for(int i = 0; i < countOfOtherPlayers; i++)
                CreatePlayer(true, GetRandomColor(), 1.5f + rand.NextSingle());
        }
        public static void OnFoodDecreased()
            => neededFoodCount++;
        public static void OnPlayerDecreased()
            => neededPlayerCount++;
        public static void OnMainPlayerLoss()
            => needForPlayer = true;
        public static void TryAddNeededGameObjects()
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
        private static void SpawnFood()
        {
            int foodCordX = rand.Next(75, (int)Render.width - 75);
            int foodCordY = rand.Next(75, (int)Render.height - 75);

            string foodName = "Food" + FreeNames.GetFreeFoodIndex().ToString();

            var newFood = new Food(foodCordX, foodCordY, 0.5f, foodName);

            Entities.Add(foodName, newFood);
        }
        private static Color GetRandomColor()
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
        private static void CreatePlayer(bool isAI, Color color, float mass)
        {
            string playerName = "player" + FreeNames.GetFreePlayerIndex().ToString();

            Eater player = new Eater(isAI, mass, playerName, color);

            Entities.Add(playerName, player);
        }
        public static void AwakeGameObjects()
        {
            foreach (var gameObject in Entities.Values)
                gameObject.Awake();
        }
    }
}
