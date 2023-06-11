
namespace AAAGR_io.Engine
{
    public static class FreeNames
    {
        private static long availablePlayerIndex = 0;
        private static long availableFoodIndex = 0;

        public static string GetFreePlayerIndex()
        {
            var lastAvailableNumber = availablePlayerIndex;

            availablePlayerIndex++;

            return lastAvailableNumber.ToString();
        }
        public static string GetFreeFoodIndex()
        {
            var lastAvailableNumber = availableFoodIndex;

            availableFoodIndex++;

            return lastAvailableNumber.ToString();
        }
    }
}
