
using System.Diagnostics.CodeAnalysis;

namespace AAAGR_io
{
    public struct ListedGameObject
    {
        public (string, GameObject) GameObjectPair;

        public ListedGameObject(string name, GameObject gameObject)
        {
            GameObjectPair = (name, gameObject);
        }

        public static bool operator ==(ListedGameObject? listedGameObject1, ListedGameObject? listedGameObject2)
            => listedGameObject1?.GameObjectPair == listedGameObject2?.GameObjectPair;

        public static bool operator !=(ListedGameObject? listedGameObject1, ListedGameObject? listedGameObject2)
            => !(listedGameObject1?.GameObjectPair == listedGameObject2?.GameObjectPair);
    }
}
