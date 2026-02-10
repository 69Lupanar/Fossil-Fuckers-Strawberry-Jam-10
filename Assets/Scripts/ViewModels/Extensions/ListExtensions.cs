using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Collections;

namespace Assets.Scripts.ViewModels.Extensions
{
    /// <summary>
    /// Extensions pour les listes
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Mélange une liste
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="list">liste</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new();
            int n = list.Count;
            while (n > 1)
            {
                NativeArray<byte> box = new(1, Allocator.Temp);
                do
                    provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                --n;
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}