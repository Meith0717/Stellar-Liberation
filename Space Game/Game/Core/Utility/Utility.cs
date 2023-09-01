using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.Utility
{
    public static class Utility
    {
        public static Random Random { get { return RandomSingleton.Instance; } }

        public static T GetRandomElement<T>(List<T> lst)
        {
            return lst[Random.Next(lst.Count)];
        }

        public static float GetMaxFromVector2(Vector2 vector)
        {
            return MathF.Max(vector.X, vector.Y);
        }

        public static Vector2 GetRandomVector2(Vector2 Origin, int radius)
        {
            return Origin + new Vector2(Random.Next(radius), Random.Next(radius));
        }

        public static Vector2 GetRandomVector2(int minX, int maxX, int minY, int maxY)
        {
            return new Vector2(Random.Next(minX, maxX), Random.Next(minY, maxY));
        }
        public static Vector2 GetRandomVector2(Vector2 star, Vector2 end)
        {
            return new Vector2(Random.Next((int)star.X, (int)end.X), Random.Next((int)star.Y, (int)end.Y));
        }
        public static Vector2 GetRandomVector2(CircleF circle)
        {
            RectangleF rectangle = circle.ToRectangle();
            Vector2 v = GetRandomVector2(rectangle.TopLeft, rectangle.BottomRight);
            if (!circle.Contains(v)) return GetRandomVector2(circle);
            return v;
        }


        public static string ConvertSecondsToGameTimeUnits(int seconds)
        {
            int jears = seconds / 356;
            seconds %= 356;
            int months = seconds / 31;
            seconds %= 31;
            return $"{2050 + jears}.{Expand(1 + months)}.{Expand(1 + seconds)}";
        }

        public static string ConvertSecondsToTimeUnits(int seconds)
        {
            int jears = seconds / 356;
            seconds %= 356;
            int months = seconds / 31;
            seconds %= 31;
            return $"{jears}J {Expand(months)}M {Expand(seconds)}D";
        }

        private static string Expand(int str)
        {
            if (str.ToString().Length > 1)
            {
                return str.ToString();
            }
            return $"0{str}";
        }
    }
}
