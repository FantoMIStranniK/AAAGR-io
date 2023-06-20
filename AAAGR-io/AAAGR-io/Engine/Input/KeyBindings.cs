using SFML.Window;

namespace AAAGR_io.Engine.Input
{
    public static class KeyBindings
    {
        public static Dictionary<string, Keyboard.Key> Binds= new Dictionary<string, Keyboard.Key>
        {
            {"Up", Keyboard.Key.W},
            {"Down", Keyboard.Key.S},
            {"Left", Keyboard.Key.A},
            {"Right", Keyboard.Key.D},
        };

        public static Action UpKeyPressed;

        public static void InitInput()
        {

        }
    }
}
