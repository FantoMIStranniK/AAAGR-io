using SFML.Graphics;

namespace Ping_pong
{
    public interface ICollider
    {
        public bool IsCollidingWith(Shape thisShape, Shape collidedShape) { return false; }
    }
}
