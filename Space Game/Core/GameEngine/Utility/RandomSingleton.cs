﻿using System;

namespace CelestialOdyssey.Core.GameEngine.Utility
{
    internal class RandomSingleton
    {
        private static Random mInstance;

        public static Random Instance { get { return mInstance ??= new(); } }
    }
}
