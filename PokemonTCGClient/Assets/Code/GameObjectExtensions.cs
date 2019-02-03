using UnityEngine;

namespace Assets.Code
{
    public static class GameObjectExtensions
    {
        public static void DestroyAllChildren(this GameObject gameObject)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                UnityEngine.Object.Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }
    }
}
