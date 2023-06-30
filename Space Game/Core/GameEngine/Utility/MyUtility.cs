using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.GameEngine.Utility
{
    public class MyUtility
    {
        public static Random Random { get { return RandomSingleton.Instance; } }

        public static int GetRandomInt(int maxInt)
        {
            return maxInt == 0 ? 0 : Random.Next(maxInt);
        }

        public static Vector2 GetPointOnBorder(Rectangle rectangle, float rad)
        {
            float halfWidth = rectangle.Width / 2f;
            float halfHeight = rectangle.Height / 2f;
            float cosAngle = (float)Math.Cos(rad);
            float sinAngle = (float)Math.Sin(rad);
            float x = rectangle.Center.X + cosAngle * halfWidth;
            float y = rectangle.Center.Y + sinAngle * halfHeight;
            x = MathHelper.Clamp(x, rectangle.Left, rectangle.Right);
            y = MathHelper.Clamp(y, rectangle.Top, rectangle.Bottom);
            return new Vector2(x, y);
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
            if (directionVector.Y < 0) { rotation = 2 * MathF.PI - MathF.Abs(rotation); }
            return rotation;
        }

        public static Vector2 GetDirection(float angleRad)
        {
            return new Vector2(MathF.Cos(angleRad), MathF.Sin(angleRad));
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

        public static float LogisticFunction(float x, float k, float x0)
        {
            return 1 / (1 + MathF.Exp(-k * (x - x0)));
        }

        private static string Expand(int str)
        {
            if (str.ToString().Length > 1)
            {
                return str.ToString();
            }
            return $"0{str}";
        }

        public static Matrix GetViewTransforationMatrix(Vector2 cameraPosition, float cameraZoom,
            int screenWidth, int screenHeight)
        {
            return Matrix.CreateTranslation(new Vector3(-cameraPosition.X, -cameraPosition.Y, 0))
                * Matrix.CreateScale(cameraZoom, cameraZoom, 1)
                * Matrix.CreateTranslation(new Vector3(screenWidth / 2f, screenHeight / 2f, 0));
        }

        public static Vector2 ScreenToWorldProjection(Matrix ViewTransformationMatrix, Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(ViewTransformationMatrix));
        }

        public static Vector2 WorldToScreenProjection(Matrix ViewTransformationMatrix, Vector2 position)
        {
            return Vector2.Transform(position, ViewTransformationMatrix);
        }
    }
}
