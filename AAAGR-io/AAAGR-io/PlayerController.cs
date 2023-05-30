using SFML.System;
using SFML.Window;

namespace AAAGR_io
{
    public class PlayerController : IControllable
    {
        public bool IsAi = false;

        //Ticks
        private int countOfTicksToChangeDirection = 0;
        private int soulChangeCooldownTicks = 10;

        //Movement
        private Vector2f newPositon;
        private Vector2f prevPositon;
        private Vector2f targetPosition;

        //Soul change
        private bool canChangeSoul = false;
        private bool doSoulChange = false;

        public ListedGameObject MyListedGameObject { get; private set; }

        private GameObject myGameObject;

        public PlayerController(ListedGameObject gameObject, bool isAi)
        {
            IsAi = isAi;
            MyListedGameObject = gameObject;

            myGameObject = MyListedGameObject.GameObjectPair.Item2;
        }

        #region Control methods
        public void SetNewGameObject(ListedGameObject gameObject)
            => MyListedGameObject = gameObject;
        public void AwakeController()
        {
            prevPositon = myGameObject.UniversalShape.Position;
            newPositon = myGameObject.UniversalShape.Position;
        }
        #endregion

        #region Input procession
        public void GetInput()
        {
            if (IsAi)
                AiInput();
            else
                HumanInput();
        }
        public void ProcessInput()
        {
            myGameObject.Move(newPositon);

            if(doSoulChange && soulChangeCooldownTicks >= 250 && canChangeSoul)
            {
                MyListedGameObject = myGameObject.ChangeSoul();
                doSoulChange = false;
                canChangeSoul = false;
            }

            if (!canChangeSoul)
                soulChangeCooldownTicks++;
        }
        #endregion

        #region Input variations
        private void AiInput()
        {
            Random rand = new Random();

            if (countOfTicksToChangeDirection >= 120)
            {
                prevPositon = myGameObject.UniversalShape.Position;
                targetPosition = new Vector2f(rand.Next(50, (int)Render.width), rand.Next(50, (int)Render.height));
                countOfTicksToChangeDirection = 0;
            }
            else
            {
                newPositon += (targetPosition - prevPositon) / 1000;
                countOfTicksToChangeDirection++;
            }
        }

        private void HumanInput()
        {
            Vector2f newPosition = myGameObject.UniversalShape.Position;

            // Get input
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                newPosition = new Vector2f(newPosition.X, newPosition.Y - 3 / myGameObject.mass);

            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                newPosition = new Vector2f(newPosition.X - 3 / myGameObject.mass, newPosition.Y);

            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                newPosition = new Vector2f(newPosition.X, newPosition.Y + 3 / myGameObject.mass);

            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                newPosition = new Vector2f(newPosition.X + 3 / myGameObject.mass, newPosition.Y);

            //Validate coordinates
            if (!IsValidCoordinate(CalculatedPositon(newPosition.X), CalculatedLimit(Render.width)))
                newPosition.X = myGameObject.UniversalShape.Position.X;

            if (!IsValidCoordinate(CalculatedPositon(newPosition.Y), CalculatedLimit(Render.height)))
                newPosition.Y = myGameObject.UniversalShape.Position.Y;

            newPositon = newPosition;

            if (Keyboard.IsKeyPressed(Keyboard.Key.F) && !canChangeSoul)
            {
                doSoulChange = true;
                canChangeSoul = true;
            }
        }
        private float CalculatedPositon(float pos)
            => pos - myGameObject.ActualPlayerShape().Radius * myGameObject.mass;
        private float CalculatedLimit(float limit)
            => limit - myGameObject.ActualPlayerShape().Radius * 2 * myGameObject.mass;
        private bool IsValidCoordinate(float coordinate, float limit)
            => coordinate > 0 && coordinate < limit;
        #endregion
    }
}
