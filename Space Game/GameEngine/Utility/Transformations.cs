/*
 *  Transformations.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using Microsoft.Xna.Framework;

namespace CelestialOdyssey.GameEngine.Utility
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
        public static Matrix CreateViewTransformationMatrix(Vector2 cameraPosition, float cameraZoom,
            int screenWidth, int screenHeight)
        {
            return Matrix.CreateTranslation(new Vector3(-cameraPosition.X, -cameraPosition.Y, 0))
                * Matrix.CreateScale(cameraZoom, cameraZoom, 1)
                * Matrix.CreateTranslation(new Vector3(screenWidth / 2f, screenHeight / 2f, 0));
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
    }
}
