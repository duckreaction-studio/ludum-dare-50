using UnityEngine;

namespace DuckReaction.Common
{
    public static class VectorExtension
    {
        public static int GetRandom(this Vector2Int vector)
        {
            return Random.Range(vector.x, vector.y);
        }

        public static float GetRandom(this Vector2 vector2)
        {
            return Random.Range(vector2.x, vector2.y);
        }
    }
}