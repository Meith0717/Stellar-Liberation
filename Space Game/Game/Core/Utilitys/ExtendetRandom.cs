// Utility.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.Utilitys
{
    public static class ExtendetRandom
    {
        public static Random Random { get { return RandomSingleton.Instance; } }

        public static T GetRandomElement<T>(List<T> lst)
        {
            return lst[Random.Next(lst.Count)];
        }

        public static Vector2 NextVectorInCircle(CircleF circle)
        {
            RectangleF rectangle = circle.ToRectangle();
            Vector2 v = GetRandomVector2(rectangle.TopLeft, rectangle.BottomRight);
            if (!circle.Contains(v)) return NextVectorInCircle(circle);
            return v;
        }

        public static Vector2 NextVectorOnBorder(CircleF circle)
        {
            var angle = MathF.PI * 1 * (float)Random.NextDouble();
            return Geometry.GetPointOnCircle(circle, angle);
        }

        public static Vector2 GetRandomVector2(Vector2 star, Vector2 end)
        {
            return new Vector2(Random.Next((int)star.X, (int)end.X), Random.Next((int)star.Y, (int)end.Y));
        }
    }
}
