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

namespace CelestialOdyssey.Game.Core.Utilitys
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
            if (directionVector.Y < 0) { rotation = 2 * MathF.PI - MathF.Abs(rotation); }
            return rotation == float.NaN ? 0 : rotation;
        }

        public static Vector2 GetPointInDirection(Vector2 position, Vector2 direction, float length) => position + direction * length;

        public static Vector2 CalculateDirectionVector(float angleRad) => new(MathF.Cos(angleRad), MathF.Sin(angleRad));

        public static float CalculateAngle(Vector2 direction)
        {
            direction.Normalize();
            return (float)MathF.Acos(Vector2.Dot(new Vector2(1, 0), direction));
        }

        public static float RadToDeg(float rad) => rad * (360 / (2 * MathF.PI));

        public static float DegToRad(float deg) => deg * (2 * MathF.PI / 360);

        public static float AngleDegDelta(float degCurrent, float degTarget) => (degTarget - degCurrent + 540) % 360 - 180;

        public static float AngleRadDelta(float radCurrent, float radTarget) => (RadToDeg(radTarget) - RadToDeg(radCurrent) + 540) % 360 - 180;
    }
}
