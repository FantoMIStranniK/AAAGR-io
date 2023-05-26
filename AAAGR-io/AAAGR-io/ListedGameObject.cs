
namespace AAAGR_io
{
    public struct ListedGameObject
    {
        public (string, GameObject) GameObjectPair;

        public ListedGameObject(string name, GameObject gameObject)
        {
            GameObjectPair = (name, gameObject);
        }
    }
}
