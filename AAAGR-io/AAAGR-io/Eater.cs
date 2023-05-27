using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Net;
using System.Net.Http.Headers;

namespace AAAGR_io
{
    public class Eater : GameObject
    {
        public bool IsAI { get; private set; } = false;

        private CircleShape body;

        private int countOfTicks = 0;

        private Vector2f newPositon;
        private Vector2f prevPositon;
        private Vector2f targetPosition;

        private bool changedSoul = false;

        public Eater(bool isAi, float mass, string name, Color color) 
        {
            IsAI = isAi;

            body = new CircleShape(25f);

            body.Origin = new Vector2f(body.Radius, body.Radius);

            UniversalShape = body;

            body.FillColor = color;
            body.OutlineColor = Color.Black;
            body.OutlineThickness = 6;

            this.mass = mass;

            tag = "Eater";
            this.name = name;
        }
        public override void OnSoulChange()
        {
            if(IsAI)
                body.FillColor = Color.Magenta;
            else
                body.FillColor = Color.Black;

            IsAI = !IsAI;
        }
        public void ChangeSoul()
        {
            List<ListedGameObject> players = Game.Instance.GameObjectsList.GetPlayerList();

            Random rand = new Random();

            ListedGameObject chosenOne;

            do
            {
                int index = rand.Next(0, players.Count);

                chosenOne = players[index];
            }
            while (chosenOne.GameObjectPair.Item2 == this);

            OnSoulChange();

            chosenOne.GameObjectPair.Item2.OnSoulChange();
        }

        #region Overrides
        public override void Awake()
        {
            base.Awake();

            if (awakened)
                return;

            awakened = true;

            Random rand = new Random();

            int playerX = rand.Next(100, (int)Render.width - 100);
            int playerY = rand.Next(100, (int)Render.height - 100);

            body.Position = new Vector2f(playerX, playerY);

            prevPositon = body.Position;
            newPositon = body.Position;
        }
        public override void Update()
        {
            base.Update();

            Move();

            ControlMass();
        }
        public override void GetInput()
        {
            base.GetInput();

            if (!IsAI)
                HumanInput();
            else
                AiInput();
        }
        public override void Destroy()
        {
            if (!IsAI)
            {
                Game.Instance.GameObjectsList.OnMainPlayerLoss();
                Game.Instance.AddScore(-1);
            }

            base.Destroy();
        }
        public override void Eat(GameObject food)
        {
            base.Eat(food);

            if (food.tag is "food")
                mass += (food.mass * 0.2f) / mass;
            else
                mass += food.mass * 0.025f;

            Game.Instance.OnObjectDeath(food.name);
        }
        protected override void OnMassChanged()
        {
            UniversalShape.Scale = new Vector2f(mass, mass);

            if (!IsAI)
                Render.UpdateMassText(mass);
        }
        #endregion

        #region MassControl
        private void ControlMass()
        {
            if(mass >= 16.5f)
            {
                Game.Instance.OnObjectDeath(name);
                Game.Instance.AddScore(3);
            }
        }
        #endregion

        #region HumanInput
        private void HumanInput()
        {
            Vector2f newPosition = body.Position;

            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                newPosition = new Vector2f(newPosition.X, newPosition.Y - 3 / mass);

            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                newPosition = new Vector2f(newPosition.X - 3 / mass, newPosition.Y);

            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                newPosition = new Vector2f(newPosition.X, newPosition.Y + 3 / mass);

            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                newPosition = new Vector2f(newPosition.X + 3 / mass, newPosition.Y);

            if(!IsValidCoordinate(newPosition.X - body.Radius * mass, Render.width - body.Radius * 2 * mass))
                newPosition.X = body.Position.X;
            if(!IsValidCoordinate(newPosition.Y - body.Radius * mass, Render.height - body.Radius * 2 * mass))
                newPosition.Y = body.Position.Y;

            newPositon = newPosition;

            if (Keyboard.IsKeyPressed(Keyboard.Key.F) && !changedSoul)
            {
                ChangeSoul();
                changedSoul = true;
            }
        }
        public override void Move()
        {
            base.Move();

            body.Position = newPositon;
        }
        public bool IsValidCoordinate(float coordinate, float limit)
            => coordinate > 0 && coordinate < limit;
        #endregion

        #region AiInput
        private void AiInput()
        {
            Random rand = new Random();

            if (countOfTicks >= 120)
            {
                prevPositon = UniversalShape.Position;
                targetPosition = new Vector2f(rand.Next(50, (int)Render.width), rand.Next(50, (int)Render.height));
                countOfTicks = 0;
            }
            else
            {
                newPositon += (targetPosition - prevPositon) / 1000;
                countOfTicks++;
            }
        }
        #endregion
    }
}
