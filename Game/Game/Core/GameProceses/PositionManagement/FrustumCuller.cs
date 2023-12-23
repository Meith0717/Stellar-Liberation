// FrustumCuller.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

/*
 *  FrustumCuller.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace StellarLiberation.Game.Core.GameProceses.PositionManagement
{
    public static class FrustumCuller
    {
        public static RectangleF GetFrustum(GraphicsDevice graphicsDevice, Matrix ViewTransformationMatrix)
        {
            var position = graphicsDevice.Viewport.Bounds.Location;
            var screenWidth = graphicsDevice.Viewport.Width;
            var screenHeight = graphicsDevice.Viewport.Height;

            Matrix inverse = Matrix.Invert(ViewTransformationMatrix);
            Vector2 LetfTopEdge = Vector2.Transform(position.ToVector2(), inverse);
            Vector2 RigbtBottomEdge = Vector2.Transform(new Vector2(screenWidth, screenHeight), inverse) - LetfTopEdge;
            return new(LetfTopEdge.X, LetfTopEdge.Y, RigbtBottomEdge.X, RigbtBottomEdge.Y);
        }
    }
}
