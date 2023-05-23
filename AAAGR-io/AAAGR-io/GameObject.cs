using SFML.Graphics;

namespace AAAGR_io
{
    public abstract class GameObject : IEat, IMovement, IControllable
    {
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

        public string tag { get; protected set; } = " " ;
        public string name { get; protected set; } = " ";

        public Shape UniversalShape { get; protected set; }

        public virtual void GetInput(){}
        public void TryEat(Shape thisShape, GameObject collided)
        {
            var thisBounds = thisShape.GetGlobalBounds();
            var collidedShapeBounds = collided.UniversalShape.GetGlobalBounds();

            if (thisBounds.Intersects(collidedShapeBounds) && mass > collided.mass)
                Eat(collided);
        }
        public virtual void Eat(GameObject food){}
        public virtual void Move(){}
        public virtual void Awake(){}
        public virtual void Update(){}
        public virtual void Destroy()
        {
            var gameObjectsList = Game.Instance.gameObjects;

            if (gameObjectsList.ContainsKey(name))
                gameObjectsList.Remove(name);

            if (tag is "food")
                Spawn.OnFoodDecreased();
        }
        protected virtual void OnMassChanged(){} 
    }
}
