using SFML.Graphics;
using SFML.System;
using AAAGR_io.Engine;
using AAAGR_io.Engine.GameObjects;
using AAAGR_io.Engine.Input;
using AAAGR_io.Game_Assets.Interfaces;
using AAAGR_io.GameAssets.Interfaces;
using AAAGR_io.Engine.Audio;

namespace AAAGR_io.GameAssets
{
    public class Eater : GameObject
    {
        public bool IsAI { get; private set; } = false;

        public CircleShape body { get; private set; }

        public PlayerController? MyController = null;

        public Eater(bool isAi, float mass, string name, Color color, bool isAnimated = false, SpriteName sprite = SpriteName.None) 
        {
            //Setting variables
            this.IsAI = isAi;
            this.IsAnimated = isAnimated;

            this.tag = "Eater";
            this.name = name;

            //Shape generation
            body = new CircleShape(25f);

            body.Origin = new Vector2f(body.Radius, body.Radius);

            UniversalShape = body;

            this.mass = mass;

            //Colors
            if (isAi)
                body.OutlineColor = Color.Black;
            else
                body.OutlineColor = Color.Red;

            body.OutlineThickness = 6;


            if (!isAnimated)
                body.FillColor = color;
            else
                animator = new Animator(sprite, UniversalShape);
        }

        #region Overrides
        public override void Awake()
        {
            base.Awake();

            if (awakened)
                return;

            if(IsAnimated)
                animator.InitAnimator();

            awakened = true;

            Random rand = new Random();

            int playerX = rand.Next(100, (int)Render.width - 100);
            int playerY = rand.Next(100, (int)Render.height - 100);

            body.Position = new Vector2f(playerX, playerY);

            MyController?.SetPositions(body.Position);
        }
        public override void Update()
        {
            base.Update();

            if (IsAnimated)
                animator.UpdateAnimation();

            ControlMass();
        }
        public override void Destroy()
        {
            MyController?.ResetGameObject();

            MyController = null;

            if (!IsAI)
            {
                Game.Instance.GameObjectsList.OnMainPlayerLoss();
                Game.Instance.AddScore(-1);
            }

            base.Destroy();
        }
        public override void Eat(GameObject food)
        {
            if (food.tag is "food")
            {
                mass += (food.mass * 0.2f) / mass;
                AudioSystem.PlaySound("EatSound", false, 2, 1f, 0.95f);
            }
            else if (food.tag is "Eater")
            {
                mass += food.mass * 0.025f;
                AudioSystem.PlaySound("DeathSound", true, 3, 1f, 0.95f);
            }

            food.isAlive = false;

            Game.Instance.OnObjectDeath(food.name);
        }
        protected override void OnMassChanged()
        {
            UniversalShape.Scale = new Vector2f(mass, mass);

            if (!IsAI)
                Render.UpdateMassText(mass);
        }
        #endregion

        public void ChangeMode()
            => IsAI = !IsAI;

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

        public override void Move(Vector2f newPosition)
        {
            base.Move(newPosition);

            body.Position = newPosition;
        }
    }
}
