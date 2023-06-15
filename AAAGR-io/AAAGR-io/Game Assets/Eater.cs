using SFML.Graphics;
using SFML.System;
using AAAGR_io.Engine;
using AAAGR_io.Engine.GameObjects;
using AAAGR_io.Engine.Input;
using AAAGR_io.Game_Assets.Interfaces;

namespace AAAGR_io.GameAssets
{
    public class Eater : GameObject
    {
        public bool IsAI { get; private set; } = false;

        public CircleShape body { get; private set; }

        public PlayerController? MyController = null;

        public Eater(bool isAi, float mass, string name, Color color, bool isAnimated = false, SpriteName sprite = SpriteName.None) 
        {
            IsAI = isAi;

            body = new CircleShape(25f);

            body.Origin = new Vector2f(body.Radius, body.Radius);

            UniversalShape = body;

            this.mass = mass;

            tag = "Eater";
            this.name = name;

            IsAnimated = isAnimated;

            body.OutlineColor = Color.Black;
            body.OutlineThickness = 6;

            if (!isAnimated)
                body.FillColor = color;
            else
                animator = new Animator(sprite, UniversalShape);
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
            while (chosenOne.GameObjectPair.Item1 == this.name || !chosenOne.GameObjectPair.Item2.isAlive);

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
            base.Eat(food);

            if (food.tag is "food")
                mass += (food.mass * 0.2f) / mass;
            else if (food.tag is "Eater")
                mass += food.mass * 0.025f;

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
