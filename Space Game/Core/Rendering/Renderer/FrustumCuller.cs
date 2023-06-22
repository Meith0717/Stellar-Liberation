using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Runtime.InteropServices;

namespace Galaxy_Explovive.Core.Rendering
{
    internal class FrustumCuller
    {
        public RectangleF mWorldFrustum { get; private set; }
        public RectangleF mViewFrustum { get; private set; }

        public void Update(int screenWidth, int screenHeight, Matrix viewTransformation)
        {
            Vector2 LetfTopWorldEdge = Vector2.Transform(Vector2.Zero, viewTransformation);
            Vector2 RigbtBottomEdge = Vector2.Transform(new Vector2(screenWidth, screenHeight), viewTransformation)
                - LetfTopWorldEdge;

            mWorldFrustum = new RectangleF(LetfTopWorldEdge.X, LetfTopWorldEdge.Y, RigbtBottomEdge.X, RigbtBottomEdge.Y);
            mViewFrustum = new RectangleF(0, 0, screenWidth, screenHeight);
        }
    }
}
