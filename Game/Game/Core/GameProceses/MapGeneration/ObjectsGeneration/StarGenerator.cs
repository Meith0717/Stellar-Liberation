// StarGenerator.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration
{
    internal static class StarGenerator
    {
        public static Star Generat(Random seededRandom)
        {
            var temperature = seededRandom.Next(2000, 50000);
            var color = GetStarColor(temperature);
            return new(1, color);
        }

        private static Color GetStarColor(float temperature)
        {
            int threshold1 = 16000;
            int threshold2 = 33000;

            if (temperature < threshold1)
            {
                float t1 = MathHelper.Clamp((temperature - 3000) / (threshold1 - 1000), 0, 1);
                return Color.Lerp(Color.Red, new(255, 201, 0), t1);
            }

            if (temperature < threshold2)
            {
                float t2 = MathHelper.Clamp((-threshold1 + (temperature - 3000)) / (threshold2 - 1000), 0, 1);
                return Color.Lerp(new(255, 201, 0), Color.White, t2);
            }

            float t3 = MathHelper.Clamp((-threshold2 + (temperature - 3000)) / (30000 - 1000), 0, 1);
            return Color.Lerp(Color.White, Color.Blue, t3);
        }
    }
}
