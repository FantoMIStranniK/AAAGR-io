using SFML.System;

namespace AAAGR_io
{

    public static class Spawn
    {
        public static Dictionary<string, GameObject> Entities =new Dictionary<string, GameObject>();

        private static int neededFoodCount = 0; 
        private static int countOfFoodPoints = 35;
        private static int playerSpawnX;
        private static int playerSpawnY;

        public static void InitSpawn()
        {
            Random rand = new Random();

            playerSpawnX = rand.Next(0, (int)Render.width);
            playerSpawnY = rand.Next(0, (int)Render.height);

            CreatePlayer();

            for(int i = 0; i < countOfFoodPoints; i++)
                SpawnFood();
        }
        public static void OnFoodDecreased()
        {
            neededFoodCount++;
        }
        public static void TryAddFood()
        {
            int lastNeededCountOfFood = neededFoodCount;

            for (int i = 0; i < lastNeededCountOfFood; i++)
                SpawnFood();

            neededFoodCount -= lastNeededCountOfFood;
        }
        private static void SpawnFood()
        {
            Random rand = new Random();

            int foodCordY = 0;
            int foodCordX = 0;

            do
            {
                foodCordX = rand.Next(75, (int)Render.width - 75);
                foodCordY = rand.Next(75, (int)Render.height - 75);
            }
            while (!Utilities.IsValidCoordinate(foodCordX, Render.width) || !Utilities.IsValidCoordinate(foodCordY, Render.height));

            string foodName = "Food" + FreeNames.GetFreeFoodIndex().ToString();

            var newFood = new Food(foodCordX, foodCordY, 0.5f, foodName);

            newFood.Awake();

            Entities.Add(foodName, newFood);
        }
        private static void CreatePlayer()
        {
            string playerName = "player" + FreeNames.GetFreePlayerIndex().ToString();

            Eater player = new Eater(false, 1.5f, playerName);

            player.UniversalShape.Position = new Vector2f(playerSpawnX, playerSpawnY);

            Entities.Add(playerName, player);
        }
    }
}
