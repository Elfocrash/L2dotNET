using System;
using System.Collections.Generic;
using System.Linq;

namespace L2dotNET.Utility.Geometry
{
    public static class Rnd
    {
        //TODO: RandomThreadSafe.Instance
        private static readonly Random rnd = new Random();

        public static double Get()
        {
            return rnd.NextDouble();
        }

        public static int Get(int a)
        {
            return (int)(rnd.NextDouble() * a);
        }

        public static int Get(int min, int max)
        {
            return min + (int)Math.Floor(rnd.NextDouble() * ((max - min) + 1));
        }

        public static int NextInt(int n)
        {
            return (int)Math.Floor(rnd.NextDouble() * n);
        }

        public static int NextInt()
        {
            return rnd.Next();
        }

        public static double NextDouble()
        {
            return rnd.NextDouble();
        }

        public static bool NextBoolean()
        {
            return Convert.ToBoolean(rnd.Next());
        }

        public static IEnumerable<T> Get<T>(List<T> list)
        {
            if ((list == null) || (list.Count == 0))
                return null;

            return list.Shuffle().Take(list.Count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}