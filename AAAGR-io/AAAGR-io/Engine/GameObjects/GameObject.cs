﻿using SFML.Graphics;
using AAAGR_io.GameAssets.Interfaces;
using SFML.System;
using AAAGR_io.Game_Assets.Interfaces;

namespace AAAGR_io.Engine.GameObjects
{
    public abstract class GameObject : IEat, IMovement, IAnimateable
    {
        #region Mass
        public float mass
        {
            get
            {
                return _mass;
            }
            protected set
            {
                _mass = value;
                OnMassChanged();
            }
        }
        private float _mass = 1;
        #endregion

        #region Main variables
        public string tag { get; protected set; } = " ";
        public string name { get; protected set; } = " ";

        public bool isAlive = true;

        public bool IsAnimated = false;

        public Shape UniversalShape { get; protected set; }

        protected Animator animator;

        protected bool awakened = false;
        #endregion


        #region Methods
        public void TryEat(Shape thisShape, GameObject collided)
        {
            var thisBounds = thisShape.GetGlobalBounds();
            var collidedShapeBounds = collided.UniversalShape.GetGlobalBounds();

            if (CanEat(thisBounds, collidedShapeBounds, collided.mass, collided.isAlive))
                Eat(collided);
        }
        private bool CanEat(FloatRect thisBounds, FloatRect collidedBounds, float collidedMass, bool collidedIsAlive)
        {
            bool intetsects = thisBounds.Intersects(collidedBounds);

            bool massIsBigger = mass - collidedMass > 0.35f;

            return intetsects && massIsBigger && collidedIsAlive;
        }
        public virtual void Eat(GameObject food) { }
        public virtual void Move(Vector2f newPosition) { }
        public virtual void Awake() { }
        public virtual void Update() { }
        public Animator GetAnimator() => animator;
        public virtual void OnSoulChange() { }
        public virtual void Destroy()
        {
            if (tag is "food")
                Game.Instance.GameObjectsList.OnFoodDecreased();
            else if (tag is "Eater" && name != "MainPlayer")
                Game.Instance.GameObjectsList.OnPlayerDecreased();
        }
        protected virtual void OnMassChanged() { }
        #endregion
    }
}
