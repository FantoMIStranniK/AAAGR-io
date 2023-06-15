using AAAGR_io.Game_Assets.Interfaces;
using SFML.Graphics;

namespace AAAGR_io.Engine
{
    public class Animator
    {
        public SpriteName ThisSpriteName = SpriteName.None;

        private List<Texture> sprites = new List<Texture>();

        private int currentFrame = 0;

        private float animationTicks = 0;

        private Shape animateableShape;

        public Animator(SpriteName spriteName, Shape animateAbleshape) 
        {
            ThisSpriteName = spriteName;

            animateableShape = animateAbleshape;

            string pathToSprites = Game.PathToProject + @"\Sprites\" + ThisSpriteName;

            string[] spriteNames = Directory.GetFiles(pathToSprites);

            foreach(var sprite in spriteNames)
                sprites.Add(new Texture(sprite));
        }
        public void UpdateAnimation()
        {
            if (ThisSpriteName == SpriteName.None)
                return;

            if(animationTicks < 24)
            {
                animationTicks += (Render.wantedFrameRate / 24) * Time.GetTime();
                return;
            }

            if (currentFrame >= sprites.Count - 1)
                currentFrame = 0;

            animationTicks = 0;

            Texture texture = sprites[currentFrame];

            currentFrame++;

            animateableShape.Texture = texture;
        }
    }
}
