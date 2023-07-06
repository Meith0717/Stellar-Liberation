/*
 *  FrustumCuller.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GalaxyExplovive.Core.GameEngine.Position_Management
{
    /// <summary>
    /// Represents a frustum culler for efficient visibility testing of objects within a view frustum.
    /// </summary>
    public class FrustumCuller
    {
        /// <summary>
        /// Represents the frustum in world coordinates.
        /// </summary>
        public RectangleF WorldFrustum { get; private set; }

        /// <summary>
        /// Represents the frustum in view coordinates.
        /// </summary>
        public RectangleF ViewFrustum { get; private set; }

        /// <summary>
        /// Updates the frustum based on the screen dimensions and the view transformation matrix.
        /// </summary>
        /// <param name="screenWidth">The width of the screen.</param>
        /// <param name="screenHeight">The height of the screen.</param>
        /// <param name="ViewTransformationMatrix">The view transformation matrix.</param>
        public void Update(int screenWidth, int screenHeight, Matrix ViewTransformationMatrix)
        {
            Matrix inverse = Matrix.Invert(ViewTransformationMatrix);
            Vector2 LetfTopEdge = Vector2.Transform(Vector2.Zero, inverse);
            Vector2 RigbtBottomEdge = Vector2.Transform(new Vector2(screenWidth, screenHeight), inverse) - LetfTopEdge;
            WorldFrustum = new RectangleF(LetfTopEdge.X, LetfTopEdge.Y, RigbtBottomEdge.X, RigbtBottomEdge.Y);
            ViewFrustum = new RectangleF(0, 0, screenWidth, screenHeight);
        }

        /// <summary>
        /// Checks if a vector position is within the view frustum.
        /// </summary>
        /// <param name="position">The vector position to check.</param>
        /// <returns>True if the position is within the view frustum, otherwise false.</returns>
        public bool VectorOnScreenView(Vector2 position)
        {
            return ViewFrustum.Contains(position);
        }

        /// <summary>
        /// Checks if a rectangle is intersecting with the view frustum.
        /// </summary>
        /// <param name="rectangle">The rectangle to check.</param>
        /// <returns>True if the rectangle intersects with the view frustum, otherwise false.</returns>
        public bool RectangleOnScreenView(RectangleF rectangle)
        {
            return ViewFrustum.Intersects(rectangle);
        }

        /// <summary>
        /// Checks if a circle is intersecting with the view frustum.
        /// </summary>
        /// <param name="circle">The circle to check.</param>
        /// <returns>True if the circle intersects with the view frustum, otherwise false.</returns>
        public bool CircleOnScreenView(CircleF circle)
        {
            return ViewFrustum.Intersects(circle);
        }

        /// <summary>
        /// Checks if a vector position is within the world frustum.
        /// </summary>
        /// <param name="position">The vector position to check.</param>
        /// <returns>True if the position is within the world frustum, otherwise false.</returns>
        public bool VectorOnWorldView(Vector2 position)
        {
            return WorldFrustum.Contains(position);
        }

        /// <summary>
        /// Checks if a rectangle is intersecting with the world frustum.
        /// </summary>
        /// <param name="rectangle">The rectangle to check.</param>
        /// <returns>True if the rectangle intersects with the world frustum, otherwise false.</returns>
        public bool RectangleOnWorldView(RectangleF rectangle)
        {
            return WorldFrustum.Intersects(rectangle);
        }

        /// <summary>
        /// Checks if a circle is intersecting with the world frustum.
        /// </summary>
        /// <param name="circle">The circle to check.</param>
        /// <returns>True if the circle intersects with the world frustum, otherwise false.</returns>
        public bool CircleOnWorldView(CircleF circle)
        {
            return WorldFrustum.Intersects(circle);
        }
    }
}
