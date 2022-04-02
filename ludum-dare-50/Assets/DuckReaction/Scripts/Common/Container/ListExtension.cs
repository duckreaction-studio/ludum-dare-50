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
    }
}