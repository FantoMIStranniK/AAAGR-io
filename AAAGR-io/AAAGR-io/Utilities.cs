using SFML.Graphics;
using SFML.System;


namespace AAAGR_io
{
    public static class Utilities
    {
        public static bool IsValidCoordinate(float coordinate, float limit)
            => coordinate > 0 && coordinate < limit;
        public static Color GetRandomColor()
        {
            List<Color> colors = new List<Color>()
            {
                Color.Black, 
                Color.Red, 
                Color.Green, 
                Color.Blue, 
                Color.Yellow,
                Color.Cyan,
                Color.Yellow,
            };

            Random rand = new Random();

            return colors[rand.Next(colors.Count)];
        }
        public static bool ApproximatelyEqual(float a, float b)
        {
            return MathF.Abs(a - b) <= 0.5f;
        }
    }
}
