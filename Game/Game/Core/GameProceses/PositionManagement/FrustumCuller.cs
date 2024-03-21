// FrustumCuller.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

/*
 *  FrustumCuller.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;

namespace StellarLiberation.Game.Core.GameProceses.PositionManagement
{
    public static class FrustumCuller
    {
        public static RectangleF GetFrustum(Resolution resolution, Matrix ViewTransformationMatrix)
        {
            var position = resolution.Bounds.Location;
            var screenWidth = resolution.Width;
            var screenHeight = resolution.Height;

            Matrix inverse = Matrix.Invert(ViewTransformationMatrix);
            Vector2 LetfTopEdge = Vector2.Transform(position.ToVector2(), inverse);
            Vector2 RigbtBottomEdge = Vector2.Transform(new Vector2(screenWidth, screenHeight), inverse) - LetfTopEdge;
            return new(LetfTopEdge.X, LetfTopEdge.Y, RigbtBottomEdge.X, RigbtBottomEdge.Y);
        }
    }
}
