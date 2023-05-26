﻿using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.Utility
{
    public class MyUtility
    {
        public static Random Random { get{ return RandomSingleton.Instance; } } 

        public static int GetRandomInt(int maxInt)
        {
            return (maxInt == 0) ? 0 : Random.Next(maxInt);
        }

        public static T GetRandomElement<T>(List<T> lst)
        {
            return lst[Random.Next(lst.Count)];
        }

        public static Vector2 GetRandomVector2(Vector2 Origin, int radius)
        {
            return Origin + new Vector2(Random.Next(radius), Random.Next(radius));
        }
        public static Vector2 GetRandomVector2(int minX, int maxX, int minY, int maxY)
        {
            return new Vector2(Random.Next(minX, maxX), Random.Next(minY, maxY));
        }


        public static Vector2 GetVector2(float radius, float angleRad, int randomVariance = 0)
        {
            CircleF circle = new CircleF(Vector2.Zero, radius);
            Vector2 pos = circle.BoundaryPointAt(angleRad);
            return pos;
        }

        public static float GetAngle(Vector2 position, Vector2 target)
        {
            Vector2 directionVector = target - position;
            float rotation = (float)MathF.Acos(Vector2.Dot(new Vector2(1, 0), directionVector) / directionVector.Length());
            if (directionVector.Y < 0) { rotation = 2*MathF.PI - MathF.Abs(rotation); }
            return rotation;
        }

        public static Vector2 GetDirection(float angleRad)
        {
            return new Vector2(MathF.Cos(angleRad), MathF.Sin(angleRad));
        }

        public static string ConvertSecondsToTimeUnits(int seconds)
        {
            int days = seconds / 86400;
            seconds %= 86400;
            int hours = seconds / 3600;
            seconds %= 3600;
            int minutes = seconds / 60;
            seconds %= 60;
            return $"{days} Days - {Expand(hours)}:{Expand(minutes)}:{Expand(seconds)}";
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