using SFML.Graphics;

namespace AAAGR_io.GameAssets.Interfaces
{
    public interface IEat
    {
        public bool TryEat(Shape thisShape, Shape collidedShape) { return false; }
    }
}
