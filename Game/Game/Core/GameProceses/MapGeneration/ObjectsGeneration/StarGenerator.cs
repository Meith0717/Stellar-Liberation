// StarGenerator.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using System;

namespace StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration
{
    internal static class StarGenerator
    {
        public static Star Generat(Random seededRandom)
        {
            var kelvin = seededRandom.Next(2000, 50000);
            var color = GetStarColor(kelvin);
            var size = seededRandom.Next(17, 17);
            return new(size, kelvin, color);
        }

        public static Color GetStarColor(float kelvin)
        {
            int threshold1 = 16000;
            int threshold2 = 33000;

            if (kelvin < threshold1)
            {
                float t1 = MathHelper.Clamp((kelvin - 3000) / (threshold1 - 1000), 0, 1);
                return Color.Lerp(Color.Red, Color.Orange, t1);
            }

            if (kelvin < threshold2)
            {
                float t2 = MathHelper.Clamp((-threshold1 + (kelvin - 3000)) / (threshold2 - 1000), 0, 1);
                return Color.Lerp(Color.Orange, Color.LightYellow, t2);
            }

            float t3 = MathHelper.Clamp((-threshold2 + (kelvin - 3000)) / (30000 - 1000), 0, 1);
            return Color.Lerp(Color.LightYellow, Color.Blue, t3);
        }
    }
}
