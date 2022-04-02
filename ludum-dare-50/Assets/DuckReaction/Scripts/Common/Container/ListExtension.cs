using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuckReaction.Common.Container
{
    public static class ListExtension
    {
        public static T Shift<T>(this IList<T> list)
        {
            return list.RemoveAndGet<T>(0);
        }

        public static T Pop<T>(this IList<T> list)
        {
            return list.RemoveAndGet<T>(list.Count - 1);
        }

        public static T RemoveAndGet<T>(this IList<T> list, int index)
        {
            T r = list[index];
            list.RemoveAt(index);
            return r;
        }

        public static T GetRandom<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static void Shuffle<T>(this IList<T> list, int shuffleCount = -1)
        {
            if (list.Count == 0)
                return;
            if (shuffleCount < 0)
                shuffleCount = list.Count;
            while (shuffleCount > 1)
            {
                --shuffleCount;
                int startIndex = Random.Range(0, list.Count);
                int endIndex = Random.Range(0, list.Count);
                // Swap
                (list[startIndex], list[endIndex]) = (list[endIndex], list[startIndex]);
            }
        }
    }
}