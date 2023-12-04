// Geometry.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

/*
 *  Transformations.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace StellarLiberation.Game.Core.Utilitys
{
    /// <summary>
    /// Provides geometric calculations and operations.
    /// </summary>
    public sealed class Geometry
    {
        public static Vector2 GetPointOnCircle(Vector2 centerPosition, float circleRadius, float angleRad)
        {
            CircleF circle = new CircleF(centerPosition, circleRadius);
            return circle.BoundaryPointAt(angleRad);
        }

        public static Vector2 GetPointOnCircle(CircleF circle, float angleRad) => circle.BoundaryPointAt(angleRad);

        public static float AngleBetweenVectors(Vector2 position, Vector2 target)
        {
            Vector2 directionVector = target - position;
            float rotation = CalculateAngle(directionVector);
            if (directionVector.Y < 0) { rotation = 2f * MathF.PI - MathF.Abs(rotation); }
            return float.IsNaN(rotation) ? 0 : rotation;
        }

        public static Vector2 GetPointInDirection(Vector2 position, Vector2 direction, float length) => position + direction * length;

        public static Vector2 CalculateDirectionVector(float angleRad) => new(MathF.Cos(angleRad), MathF.Sin(angleRad));

        public static float CalculateAngle(Vector2 direction)
        {
            direction.Normalize();
            return MathF.Acos(Vector2.Dot(new Vector2(1, 0), direction));
        }

        public static Vector2 GetRelativePosition(Vector2 absolutePosition, Rectangle rectangle)
        {
            float relativeX = absolutePosition.X - rectangle.X;
            float relativeY = absolutePosition.Y - rectangle.Y;

            return new Vector2(relativeX, relativeY);
        }

        public static Vector2 GetPoitOnRectangle(RectangleF rectangle, float angle)
        {
            float halfWidth = rectangle.Width / 2;
            float halfHeight = rectangle.Height / 2;

            float x = halfWidth * (float)Math.Cos(angle);
            float y = halfHeight * (float)Math.Sin(angle);

            return Vector2.Add(new Vector2(x, y), rectangle.Center);
        }


        public static float RadToDeg(float rad) => rad * (360f / (2f * MathF.PI));

        public static float DegToRad(float deg) => deg * (2f * MathF.PI / 360f);

        public static float AngleDegDelta(float degCurrent, float degTarget) => (degTarget - degCurrent + 540f) % 360f - 180f;

        public static float AngleRadDelta(float radCurrent, float radTarget) => (RadToDeg(radTarget) - RadToDeg(radCurrent) + 540f) % 360f - 180f;
    }
}
