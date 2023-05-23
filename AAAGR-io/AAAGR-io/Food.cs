using SFML.System;
using SFML.Graphics;

namespace AAAGR_io
{
    public class Food : GameObject
    {
        private RectangleShape shape;

        private Vector2f newPosition;

        public Food(float posX, float posY, float mass, string name) 
        { 
            this.mass = mass;

            shape = new RectangleShape(new Vector2f(10, 10));

            shape.FillColor = Color.Green;

            UniversalShape = shape;

            newPosition = new Vector2f(posX, posY);

            this.name = name;

            tag = "food";
        }

        public override void Awake()
        {
            base.Awake();

            shape.Position = newPosition;
        }
    }
}
