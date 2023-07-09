﻿/*
 *  Transformations.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace GalaxyExplovive.Core.GameEngine.Utility
{
    /// <summary>
    /// Provides geometric calculations and operations.
    /// </summary>
    public sealed class Geometry
    {
        /// <summary>
        /// Calculates the position of a point on a circle given the center position, radius, and angle in radians.
        /// </summary>
        /// <param name="centerPosition">The center position of the circle.</param>
        /// <param name="circleRadius">The radius of the circle.</param>
        /// <param name="angleRad">The angle in radians.</param>
        /// <returns>The position of the point on the circle.</returns>
        public static Vector2 GetPointOnCircle(Vector2 centerPosition, float circleRadius, float angleRad)
        {
            CircleF circle = new CircleF(centerPosition, circleRadius);
            return circle.BoundaryPointAt(angleRad);
        }

        /// <summary>
        /// Calculates the angle in radians between two vectors.
        /// </summary>
        /// <param name="position">The starting position of the vector.</param>
        /// <param name="target">The target position of the vector.</param>
        /// <returns>The angle in radians between the two vectors.</returns>
        public static float AngleBetweenVectors(Vector2 position, Vector2 target)
        {
            Vector2 directionVector = target - position;
            float rotation = (float)MathF.Acos(Vector2.Dot(new Vector2(1, 0), directionVector) / directionVector.Length());
            if (directionVector.Y < 0) { rotation = 2 * MathF.PI - MathF.Abs(rotation); }
            return rotation;
        }

        /// <summary>
        /// Calculates the normalized direction vector based on the given angle in radians.
        /// </summary>
        /// <param name="angleRad">The angle in radians.</param>
        /// <returns>The normalized direction vector.</returns>
        public static Vector2 CalculateDirectionVector(float angleRad)
        {
            return new Vector2(MathF.Cos(angleRad), MathF.Sin(angleRad));
        }
    }
}