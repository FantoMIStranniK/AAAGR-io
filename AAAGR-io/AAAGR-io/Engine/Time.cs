using SFML.System;
using System.Diagnostics;


namespace AAAGR_io.Engine
{
    public static class Time
    {
        public static double totalTimeBeforeUpdate { get; private set; } = 0d;

        public static int deltaTime { get; private set; } = 0;

        private static float previousTimeElapsed = 0f;
        private static float systemDeltaTime = 0f;
        private static float totalTimeElapsed = 0f;

        private static Clock clock = new Clock();
        private static Stopwatch timer = new Stopwatch();

        public static void StartTime()
        {
            timer.Start();
            deltaTime = 0;
        }
        public static void UpdateTime()
        {
            deltaTime = timer.Elapsed.Milliseconds;

            timer.Restart();
        }
        public static void UpdateSystemTime()
        {
            totalTimeElapsed = clock.ElapsedTime.AsSeconds();
            systemDeltaTime = totalTimeElapsed - previousTimeElapsed;
            previousTimeElapsed = totalTimeElapsed;

            totalTimeBeforeUpdate += deltaTime;
        }
        public static void ResetTimeBeforeUpdate()
        {
            totalTimeBeforeUpdate = 0;
        }
    }
}
