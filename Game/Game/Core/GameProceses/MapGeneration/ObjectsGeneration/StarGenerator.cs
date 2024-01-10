// StarGenerator.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration
{
    internal static class StarGenerator
    {
        private enum StarSizes { Dwarf, Main, Giants, SuperGiants };
        private enum StarTypes { M, K, G, F, A, B, O }

        public static Star Generat(Random seededRandom)
        {
            List<StarSizes> enumList = Enum.GetValues(typeof(StarSizes)).Cast<StarSizes>().ToList();
            var randomSie = enumList[seededRandom.Next(enumList.Count)];

            switch (randomSie)
            {
                case StarSizes.Dwarf:
                    break;
                case StarSizes.Main:
                    break;
                case StarSizes.Giants:
                    break;
                case StarSizes.SuperGiants: 
                    break;
            }

            return null;
        }
    }
}
