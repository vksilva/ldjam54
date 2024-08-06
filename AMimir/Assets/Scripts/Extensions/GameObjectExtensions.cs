using UnityEngine;

namespace Busta.Extensions
{
    public static class GameObjectExtensions
    {
        public static void SetParent(this GameObject gameObject, GameObject parent)
        {
            gameObject.transform.SetParent(parent.transform);
        }

        public static GameObject CreateChildObject(this GameObject parent, string name)
        {
            var gameObject = new GameObject(name);
            gameObject.SetParent(parent);
            return gameObject;
        }

        public static T CreateChildObject<T>(this GameObject parent, string name) where T : Component
        {
            var gameObject = parent.CreateChildObject(name);
            var component = gameObject.AddComponent<T>();
            return component;
        }
    }
}