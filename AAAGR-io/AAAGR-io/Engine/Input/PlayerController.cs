using AAAGR_io.GameAssets;
using SFML.System;
using SFML.Window;

namespace AAAGR_io.Engine.Input
{
    public class PlayerController
    {
        public bool IsAi { get; private set; } = false;

        public Eater? ControlledGameObject { get; private set; } = null;

        public Vector2f estaminatedPosition = new Vector2f();
        public Vector2f prevPositon = new Vector2f();
        public Vector2f targetPosition = new Vector2f();

        private int countOfTicks = 120;

        public PlayerController(bool isAi)
        {
            IsAi = isAi;

            ControlledGameObject = null;
        }

        public void SetNewGameObject(Eater gameObject)
        {
            ControlledGameObject = gameObject;

            estaminatedPosition = gameObject.UniversalShape.Position;

            prevPositon = estaminatedPosition;
            targetPosition = estaminatedPosition;

            countOfTicks = 120;
        }
        public void ResetGameObject()
            => ControlledGameObject = null;
        public void SetPositions(Vector2f bodyPosition)
        {
            prevPositon = bodyPosition;
            estaminatedPosition = bodyPosition;
        }
        public void GetInput()
        {
            if (ControlledGameObject == null)
                return;

            if (IsAi) ;
            //AiInput();
            else
                HumanInput();

            ControlledGameObject?.Move(estaminatedPosition);
        }
        public void HumanInput()
        {
            Vector2f newPosition = ControlledGameObject.body.Position;

            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                newPosition = new Vector2f(newPosition.X, newPosition.Y - 3 / ControlledGameObject.mass);

            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                newPosition = new Vector2f(newPosition.X - 3 / ControlledGameObject.mass, newPosition.Y);

            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                newPosition = new Vector2f(newPosition.X, newPosition.Y + 3 / ControlledGameObject.mass);

            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                newPosition = new Vector2f(newPosition.X + 3 / ControlledGameObject.mass, newPosition.Y);

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
