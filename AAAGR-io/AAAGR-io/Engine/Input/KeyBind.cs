using SFML.Window;

public enum InputType
{
    Up,
    Down, 
    Left, 
    Right,
    SoulChange,
}
namespace AAAGR_io.Engine.Input
{
    public class KeyBind
    {
        public Keyboard.Key ThisKey;

        private bool isPressed = false;

        public KeyBind(Keyboard.Key key)
        {
            ThisKey = key;

            OnKeyDown = new Action(() => { });
            OnKeyUp = new Action(() => { });
        }
        public void GetInput()
        {
            if (Keyboard.IsKeyPressed(ThisKey))
            {
                OnKeyDown.Invoke();
                isPressed = true;
            }
            else if (isPressed)
            {
                isPressed = false;  
                OnKeyUp.Invoke(); 
            }
        }

        public Action OnKeyDown;
        public Action OnKeyUp;
    }
}
