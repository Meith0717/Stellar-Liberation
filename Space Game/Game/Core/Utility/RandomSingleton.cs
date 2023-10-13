// RandomSingleton.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using System;

namespace CelestialOdyssey.Game.Core.Utility
{
    internal class RandomSingleton
    {
        private static Random mInstance;

        public static Random Instance { get { return mInstance ??= new(); } }
    }
}
