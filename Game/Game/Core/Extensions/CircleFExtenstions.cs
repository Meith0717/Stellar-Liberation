﻿// CircleFExtenstions.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace StellarLiberation.Game.Core.Extensions
{
    public static class CircleFExtensions
    {
        public static Vector2[] GetPolygone(this CircleF circle)
        {
            var sides = 20;
            var radius = circle.Radius;

            Vector2[] array = new Vector2[sides];
            double num = Math.PI * 2.0 / (double)sides;
            double num2 = 0.0;
            for (int i = 0; i < sides; i++)
            {
                array[i] = new Vector2((float)(radius * Math.Cos(num2)), (float)(radius * Math.Sin(num2)));
                num2 += num;
            }

            return array;
        }
    }
}
