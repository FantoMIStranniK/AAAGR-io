using SFML.System;

namespace AAAGR_io
{
    public static class Utilities
    {
        public static float SquareModule(this Vector2f vector)
        {
            return vector.X * vector.X + vector.Y*vector.Y;
        }
        public static bool IsValidCoordinate(float coordinate, float limit)
            => coordinate > 0 && coordinate < limit;
    }
}
