using UnityEngine;

namespace DuckReaction.Common
{
    public static class VectorExtension
    {
        public static int GetRandom(this Vector2Int vector)
        {
            return Random.Range(vector.x, vector.y);
        }
    }
}