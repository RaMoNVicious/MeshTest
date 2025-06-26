using UnityEngine;

namespace Utils
{
    public static class GameObjectUtils
    {
        public static Vector3 IgnoreY(this Vector3 target)
        {
            return new Vector3(
                target.x,
                0f,
                target.z
            );
        }
    }
}