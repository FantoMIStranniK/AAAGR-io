using AAAGR_io.GameAssets;
using SFML.System;
using SFML.Window;

namespace AAAGR_io.Engine.Input
{
    public struct PlayerController
    {
        public bool IsAi { get; private set; } = false;

        public Eater? ControlledGameObject = null;

        public Vector2f estaminatedPosition = new Vector2f();
        public Vector2f prevPositon = new Vector2f();
        public Vector2f targetPosition = new Vector2f();

        private int countOfTicks = 120;

        public PlayerController(bool isAi)
        {
            IsAi = isAi;
        }

        public void SetNewGameObject(ref Eater gameObject)
            => ControlledGameObject = gameObject;
        public void ResetGameObject()
            => ControlledGameObject = null;
        public void SetPositions(Vector2f bodyPosition)
        {
            prevPositon = bodyPosition;
            estaminatedPosition = bodyPosition;
        }
        public void HumanInput(KeyEventArgs e)
        {
            Vector2f newPosition = ControlledGameObject.body.Position;

            newPosition += KeyBindings.Binds[e.Code];

            if (!IsValidCoordinate(newPosition.X - ControlledGameObject.body.Radius * ControlledGameObject.mass, Render.width - ControlledGameObject.body.Radius * 2 * ControlledGameObject.mass))
                newPosition.X = ControlledGameObject.body.Position.X;
            if (!IsValidCoordinate(newPosition.Y - ControlledGameObject.body.Radius * ControlledGameObject.mass, Render.height - ControlledGameObject.body.Radius * 2 * ControlledGameObject.mass))
                newPosition.Y = ControlledGameObject.body.Position.Y;

            estaminatedPosition = newPosition;

            /*
            if (Keyboard.IsKeyPressed(Keyboard.Key.F) && !changedSoul)
            {
                ChangeSoul();
                changedSoul = true;
            }
            */
        }
        public bool IsValidCoordinate(float coordinate, float limit)
            => coordinate > 0 && coordinate < limit;

        public void AiInput()
        {
            Random rand = new Random();

            if (countOfTicks >= 120)
            {
                prevPositon = ControlledGameObject.UniversalShape.Position;
                targetPosition = new Vector2f(rand.Next(50, (int)Render.width), rand.Next(50, (int)Render.height));
                countOfTicks = 0;
            }
            else
            {
                estaminatedPosition += (targetPosition - prevPositon) / 1000;
                countOfTicks++;
            }
        }
    }
}
