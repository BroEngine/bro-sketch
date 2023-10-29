using System;
using System.Collections.Generic;

namespace Bro.Client
{
    public static class ListExtensions
    {
        public static bool Contains<T>(this List<T> list, Predicate<T> condition)
        {
            for (var index = 0; index < list.Count; index++)
            {
                if(condition(list[index]))
                {
                    return true;
                }
            }
            return false;
        }
        
        public static bool RemoveFirst<T>(this List<T> list, Predicate<T> condition)
        {
            for (var index = 0; index < list.Count; index++)
            {
                if(condition(list[index]))
                {
                    list.RemoveAt(index);
                    return true;
                }
            }

            return false;
        }
        
        public static int RemoveAll<T>(this List<T> list, Predicate<T> condition)
        {
            int deletedItemsCount = 0;
            for (var index = list.Count - 1; index >= 0; )
            {
                if(condition(list[index]))
                {
                    list.RemoveAt(index);
                    ++deletedItemsCount;
                }
                else
                {
                    index--;
                }
            }

            return deletedItemsCount;
        }
        
        public static void FastRemoveAtIndex<T>(this System.Collections.Generic.IList<T> source, int removeElementIndex)
        {
            var lastElemIndex = source.Count - 1;
            source[removeElementIndex] = source[lastElemIndex];
            source.RemoveAt(lastElemIndex);
        }

        public static bool FastRemoveFirst<T>(this System.Collections.Generic.IList<T> source, T element) where T : class
        {
            for (int i = 0, max = source.Count; i < max; ++i)
            {
                if (source[i] == element)
                {
                    FastRemoveAtIndex(source, i);
                    return true;
                }
            }

            return false;
        }
    }
}