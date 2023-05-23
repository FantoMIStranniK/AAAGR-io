using SFML.Graphics;
using SFML.System;

namespace AAAGR_io
{

    public static class Spawn
    {
        public static Dictionary<string, GameObject> Entities =new Dictionary<string, GameObject>();

        //Food values
        private static int neededFoodCount = 0; 
        private static int countOfFoodPoints = 35;
        private static int countOfOtherPlayers = 5;

        public static void InitSpawn()
        {
            //Create main player
            CreatePlayer(false, Color.Magenta);

            for(int i = 0; i < countOfFoodPoints; i++)
                SpawnFood();

            for(int i = 0; i < countOfOtherPlayers; i++)
                CreatePlayer(true, Utilities.GetRandomColor());
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

            int foodCordX = rand.Next(75, (int)Render.width - 75);
            int foodCordY = rand.Next(75, (int)Render.height - 75);

            string foodName = "Food" + FreeNames.GetFreeFoodIndex().ToString();

            var newFood = new Food(foodCordX, foodCordY, 0.5f, foodName);

            newFood.Awake();

            Entities.Add(foodName, newFood);
        }
        private static void CreatePlayer(bool isAI, Color color)
        {
            string playerName = "player" + FreeNames.GetFreePlayerIndex().ToString();

            Eater player = new Eater(isAI, 1.5f, playerName, color);

            Entities.Add(playerName, player);
        }
    }
}
