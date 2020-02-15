using System.Collections.Generic;

namespace FellOfACargoShip.Extensions
{
    internal static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> list)
        {
            int i = list.Count;
            while (i > 1)
            {
                int index = UnityEngine.Random.Range(0, i);
                i--;
                T value = list[i];
                list[i] = list[index];
                list[index] = value;
            }
        }
    }
}
