using System;

namespace XOGame.Core
{
    public static class RandomHelper
    {
        private static readonly Random Rand = new Random(DateTime.Now.Second);
        public static int RandomInteger()
        {
            return 1 + (int) (Rand.NextDouble() * 100000000);
        }
    }
}