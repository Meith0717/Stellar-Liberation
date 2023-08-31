using System;

namespace CelestialOdyssey.Game.Core.Utility
{
    internal class RandomSingleton
    {
        private static Random mInstance;

        public static Random Instance { get { return mInstance ??= new(); } }
    }
}
