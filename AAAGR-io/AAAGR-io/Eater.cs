using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace AAAGR_io
{
    public class Eater : GameObject
    {
        public bool IsAI { get; private set; } = false;

        private CircleShape body;

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

        #region Overrides
        public override void Awake()
        {
            base.Awake();

            Random rand = new Random();

            int playerX = rand.Next(100, (int)Render.width);
            int playerY = rand.Next(100, (int)Render.height);

            body.Position = new Vector2f(playerX, playerY);
        }
        public override void GetInput()
        {
            base.GetInput();

            if (!IsAI)
                HumanInput();
            else
                AiInput();
        }
        public override void Eat(GameObject food)
        {
            base.Eat(food);

            if(food.tag is "food")
                mass += (food.mass * 0.2f) / mass;

            food.Destroy();
        }
        protected override void OnMassChanged()
        {
            UniversalShape.Scale = new Vector2f(mass, mass);
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

            if(!Utilities.IsValidCoordinate(newPosition.X - body.Radius * mass, Render.width - body.Radius * 2 * mass))
                newPosition.X = body.Position.X;
            if(!Utilities.IsValidCoordinate(newPosition.Y - body.Radius * mass, Render.height - body.Radius * 2 * mass))
                newPosition.Y = body.Position.Y;

            body.Position = newPosition;
        }
        #endregion

        #region AiInput
        private void AiInput()
        {

        }
        #endregion
    }
}
