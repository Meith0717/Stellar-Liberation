﻿// Transformations.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

/*
 *  Transformations.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using Microsoft.Xna.Framework;
using System;

namespace StellarLiberation.Game.Core.Utilitys
{
    /// <summary>
    /// Provides methods for transforming positions between screen space and world space using a view transformation matrix.
    /// </summary>
    public sealed class Transformations
    {
        /// <summary>
        /// Creates a view transformation matrix based on the camera position, camera zoom level, screen width, and screen height.
        /// </summary>
        /// <param name="cameraPosition">The position of the camera in world space.</param>
        /// <param name="cameraZoom">The zoom level of the camera.</param>
        /// <param name="screenWidth">The width of the screen.</param>
        /// <param name="screenHeight">The height of the screen.</param>
        /// <returns>The view transformation matrix.</returns>
        public static Matrix CreateViewTransformationMatrix(Vector2 cameraPosition, float cameraZoom, float cameraRotation,
                int screenWidth, int screenHeight)
        {
            Matrix translationMatrix = Matrix.CreateTranslation(new Vector3(-cameraPosition.X, -cameraPosition.Y, 0));
            Matrix scaleMatrix = Matrix.CreateScale(cameraZoom, cameraZoom, 1);
            Matrix rotationMatrix = Matrix.CreateRotationZ(cameraRotation);
            Matrix screenCenterMatrix = Matrix.CreateTranslation(new Vector3(screenWidth / 2f, screenHeight / 2f, 0));

            return translationMatrix * scaleMatrix * rotationMatrix * screenCenterMatrix;
        }

        /// <summary>
        /// Converts a position in screen space to a position in world space using the provided view transformation matrix.
        /// </summary>
        /// <param name="viewTransformationMatrix">The view transformation matrix.</param>
        /// <param name="position">The position in screen space.</param>
        /// <returns>The position in world space.</returns>
        public static Vector2 ScreenToWorld(Matrix ViewTransformationMatrix, Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(ViewTransformationMatrix));
        }

        /// <summary>
        /// Converts a position in world space to a position in screen space using the provided view transformation matrix.
        /// </summary>
        /// <param name="viewTransformationMatrix">The view transformation matrix.</param>
        /// <param name="position">The position in world space.</param>
        /// <returns>The position in screen space.</returns>
        public static Vector2 WorldToScreen(Matrix ViewTransformationMatrix, Vector2 position)
        {
            return Vector2.Transform(position, ViewTransformationMatrix);
        }

        public static Vector2 RotateVector(Vector2 vector, float angle)
        {
            float cosTheta = (float)MathF.Cos(angle);
            float sinTheta = (float)MathF.Sin(angle);
            return new Vector2(
                vector.X * cosTheta - vector.Y * sinTheta,
                vector.X * sinTheta + vector.Y * cosTheta
            );
        }

        public static Vector2 Rotation(Vector2 rotationCenter, Vector2 rotatetVector, float angleRad) =>  RotateVector(rotatetVector, angleRad) + rotationCenter;
    }
}
