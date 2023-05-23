using SFML.Graphics;

namespace AAAGR_io
{
    public interface IEat
    {
        public bool TryEat(Shape thisShape, Shape collidedShape) { return false; }
    }
}
