
using SFML.Graphics;

namespace Ping_pong
{
    public abstract class GameObject : ICollider, IMovement
    {
        public string tag { get; protected set; } = " " ;

        public Shape DrawableShape { get; protected set; }

        public bool IsCollidingWith(Shape thisShape, Shape collidedShape)
        {
            var thisBounds = thisShape.GetGlobalBounds();
            var collidedShapeBounds = collidedShape.GetGlobalBounds();

            if (thisBounds.Intersects(collidedShapeBounds))
                return true;

            return false;
        }
        public virtual void Move(){}
        public virtual void Awake(){}
        public virtual void Update(){}
        public virtual void Destroy(){}
    }
}
