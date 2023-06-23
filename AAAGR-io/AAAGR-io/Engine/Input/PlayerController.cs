using AAAGR_io.Engine.GameObjects;
using AAAGR_io.GameAssets;
using SFML.System;
using SFML.Window;
using System.Diagnostics.CodeAnalysis;

namespace AAAGR_io.Engine.Input
{
    public class PlayerController
    {
        public Dictionary<InputType, KeyBind> KeyBinds = new Dictionary<InputType, KeyBind>
        {
            {InputType.Up, new KeyBind(Keyboard.Key.W)},
            {InputType.Down, new KeyBind(Keyboard.Key.S)},
            {InputType.Left, new KeyBind(Keyboard.Key.A)},
            {InputType.Right, new KeyBind(Keyboard.Key.D)},
            {InputType.SoulChange, new KeyBind(Keyboard.Key.F)},
        };

        public bool IsAi { get; private set; } = false;

        public Eater? ControlledGameObject { get; private set; } = null;

        public Vector2f estaminatedPosition = new Vector2f();
        public Vector2f prevPositon = new Vector2f();
        public Vector2f targetPosition = new Vector2f();

        private int countOfTicks = 120;

        private Random rand = new Random();

        public PlayerController(bool isAi)
        {
            IsAi = isAi;

            ControlledGameObject = null;

            BindActions();
        }
        private void BindActions()
        {
            KeyBinds[InputType.Up].OnKeyDown += 
                () => ProcessNewCoordinate(new Vector2f(0, -3));

            KeyBinds[InputType.Down].OnKeyDown +=
                () => ProcessNewCoordinate(new Vector2f(0, 3));

            KeyBinds[InputType.Right].OnKeyDown +=
                () => ProcessNewCoordinate(new Vector2f(3, 0));

            KeyBinds[InputType.Left].OnKeyDown +=
                () => ProcessNewCoordinate(new Vector2f(-3, 0));

            KeyBinds[InputType.SoulChange].OnKeyDown +=
                () => ChangeSoul();
        }

        public void SetNewGameObject(Eater gameObject)
        {
            ControlledGameObject = gameObject;

            estaminatedPosition = gameObject.UniversalShape.Position;

            prevPositon = estaminatedPosition;
            targetPosition = estaminatedPosition;

            countOfTicks = 120;
        }
        public void ChangeMode()
        {
            IsAi = !IsAi;

            ControlledGameObject?.ChangeMode();
        }
        public void ResetGameObject()
            => ControlledGameObject = null;
        public void SetPositions(Vector2f bodyPosition)
        {
            prevPositon = bodyPosition;
            estaminatedPosition = bodyPosition;

            countOfTicks = 120;
        }
        public void GetInput()
        {
            if (ControlledGameObject == null)
                return;

            if (IsAi)
                AiInput();
            else
                foreach (var key in KeyBinds.Values)
                    key.GetInput();

            ControlledGameObject?.Move(estaminatedPosition);
        }
        private void ProcessNewCoordinate(Vector2f shit)
        {
            Vector2f newPosition = ControlledGameObject.UniversalShape.Position;

            newPosition += new Vector2f(shit.X * Time.GetTime() / ControlledGameObject.mass, shit.Y * Time.GetTime() / ControlledGameObject.mass);

            if (!IsValidCoordinate(newPosition.X - ControlledGameObject.body.Radius * ControlledGameObject.mass, Render.width - ControlledGameObject.body.Radius * 2 * ControlledGameObject.mass))
                newPosition.X = ControlledGameObject.body.Position.X;
            if (!IsValidCoordinate(newPosition.Y - ControlledGameObject.body.Radius * ControlledGameObject.mass, Render.height - ControlledGameObject.body.Radius * 2 * ControlledGameObject.mass))
                newPosition.Y = ControlledGameObject.body.Position.Y;

            estaminatedPosition = newPosition;
        }
        public bool IsValidCoordinate(float coordinate, float limit)
            => coordinate > 0 && coordinate < limit;
        private void ChangeSoul()
        {
            PlayerController randomController;

            int randomIndex = 0;

            var controllers = Game.Instance.GameObjectsList.PlayerControllers;

            do
            {
                randomIndex = rand.Next(0, controllers.Count);

                randomController = controllers[randomIndex];
            }
            while (IsInvalidController(randomController));

            var formerControlledObject = ControlledGameObject;

            formerControlledObject.MyController = randomController;
            randomController.ControlledGameObject.MyController = this;

            SetNewGameObject(randomController.ControlledGameObject);
            ChangeMode();

            randomController.SetNewGameObject(formerControlledObject);
            randomController.ChangeMode();
        }
        private bool IsInvalidController(PlayerController controller)
        {
            if(controller.ControlledGameObject == null)
                return true;

            if (!controller.ControlledGameObject.isAlive)
                return true;

            if (controller.ControlledGameObject?.name == ControlledGameObject?.name)
                return true;

            return false;
        }

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
                estaminatedPosition += (targetPosition - prevPositon) * Time.GetTime() / 1000;
                countOfTicks++;
            }
        }
    }
}
