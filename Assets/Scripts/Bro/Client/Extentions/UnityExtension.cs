using UnityEngine;

#if UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_XBOXONE || UNITY_PS4 || UNITY_WEBGL || UNITY_WII
namespace Bro
{
    public static class UnityExtension
    {
        public static bool IsNull(this UnityEngine.Object o)
        {
            return o == null || !o;
        }

        public static T GetComponentInChildren<T>(this GameObject  gameObject) where T: MonoBehaviour
        {
            foreach (Transform child in gameObject.transform)
            {
                var o = child.gameObject.GetComponent<T>();
                if (o != null)
                {
                    return o;
                }
            }

            return null;
        }
    }
}
#endif