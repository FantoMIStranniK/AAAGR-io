using SFML.Graphics;

namespace AAAGR_io
{
    public abstract class GameObject : IEat, IMovement, IControllable
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

        public string tag { get; protected set; } = " " ;
        public string name { get; protected set; } = " ";

        public Shape UniversalShape { get; protected set; }

        protected bool awakened = false;

        #region Methods
        public void TryEat(Shape thisShape, GameObject collided)
        {
            var thisBounds = thisShape.GetGlobalBounds();
            var collidedShapeBounds = collided.UniversalShape.GetGlobalBounds();

            if (thisBounds.Intersects(collidedShapeBounds) && MathF.Abs(mass - collided.mass) > 0.35f && mass > collided.mass)
                Eat(collided);
        }
        public virtual void GetInput(){}
        public virtual void Eat(GameObject food){}
        public virtual void Move(){}
        public virtual void Awake(){}
        public virtual void Update(){}
        public virtual void OnSoulChange(){}
        public virtual void Destroy()
        {
            if (tag is "food")
                Game.Instance.GameObjectsList.OnFoodDecreased();
            else if (tag is "Eater")
                Game.Instance.GameObjectsList.OnPlayerDecreased();
        }
        protected virtual void OnMassChanged(){}
        #endregion
    }
}
