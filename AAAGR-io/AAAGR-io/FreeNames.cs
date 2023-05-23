
namespace AAAGR_io
{
    public static class FreeNames
    {
        private static long availablePlayerIndex = 0;
        private static long availableFoodIndex = 0;

        public static long GetFreePlayerIndex()
        {
            var lastAvailableNumber = availablePlayerIndex;

            availablePlayerIndex++;

            return lastAvailableNumber;
        }
        public static long GetFreeFoodIndex()
        {
            var lastAvailableNumber = availableFoodIndex;

            availableFoodIndex++;

            return lastAvailableNumber;
        }
    }
}
