// RandomSingleton.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using System;

namespace StellarLiberation.Game.Core.Utilitys
{
    internal class RandomSingleton
    {
        private static Random mInstance;

        public static Random Instance { get { return mInstance ??= new(); } }
    }
}
