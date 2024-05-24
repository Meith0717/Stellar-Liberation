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
        public static Matrix CreateViewTransformationMatrix(Vector2 cameraPosition, float cameraZoom, float cameraRotation,
                int screenWidth, int screenHeight)
        {
            int virtualWidth = 1920;
            int virtualHeight = 1080;

            float screenAspect = (float)screenWidth / screenHeight;
            float virtualAspect = (float)virtualWidth / virtualHeight;

            float scale;
            if (screenAspect < virtualAspect)
                scale = (float)screenWidth / virtualWidth;
            else
                scale = (float)screenHeight / virtualHeight;

            Matrix translationMatrix = Matrix.CreateTranslation(new Vector3(-cameraPosition.X, -cameraPosition.Y, 0));
            Matrix scaleMatrix = Matrix.CreateScale(cameraZoom * scale, cameraZoom * scale, 1);
            Matrix rotationMatrix = Matrix.CreateRotationZ(cameraRotation);
            Matrix screenCenterMatrix = Matrix.CreateTranslation(new Vector3(screenWidth / 2f, screenHeight / 2f, 0));

            return translationMatrix * scaleMatrix * rotationMatrix * screenCenterMatrix;
        }

        public static Vector2 ScreenToWorld(Matrix ViewTransformationMatrix, Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(ViewTransformationMatrix));
        }

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

        public static Vector2 Rotation(Vector2 rotationCenter, Vector2 rotatetVector, float angleRad) => RotateVector(rotatetVector, angleRad) + rotationCenter;
    }
}
