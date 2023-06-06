using SFML.System;
using SFML.Window;

namespace AAAGR_io.Engine.Input
{
    public static class KeyBindings
    {
        public static Dictionary<Keyboard.Key, Vector2f> Binds= new Dictionary<Keyboard.Key, Vector2f>
        {
            {Keyboard.Key.W, new Vector2f(0f, 1f)},
            {Keyboard.Key.S, new Vector2f(0f, -1f)},
            {Keyboard.Key.W, new Vector2f(1f, 0f)},
            {Keyboard.Key.W, new Vector2f(-1f, 0f)},
        };
    }
}
