using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.Rendering
{
    public class FrustumCuller
    {
        public RectangleF WorldFrustum { get; private set; }
        public RectangleF ViewFrustum { get; private set; }

        public void Update(int screenWidth, int screenHeight, Matrix ViewTransformationMatrix)
        {   Matrix inverse = Matrix.Invert(ViewTransformationMatrix);
            Vector2 LetfTopEdge = Vector2.Transform(Vector2.Zero, inverse);
            Vector2 RigbtBottomEdge = Vector2.Transform(new Vector2(screenWidth, screenHeight), inverse)
                - LetfTopEdge;
            WorldFrustum = new RectangleF(LetfTopEdge.X, LetfTopEdge.Y, RigbtBottomEdge.X, RigbtBottomEdge.Y);
            ViewFrustum = new RectangleF(0, 0, screenWidth, screenHeight);
        }

        public bool VectorOnScreenView(Vector2 position) { return ViewFrustum.Contains(position); }
        public bool RectangleOnScreenView(RectangleF rectangle) { return ViewFrustum.Intersects(rectangle); }
        public bool CircleOnScreenView(CircleF circle) { return ViewFrustum.Intersects(circle); }

        public bool VectorOnWorldView(Vector2 position) { return WorldFrustum.Contains(position); }
        public bool RectangleOnWorldView(RectangleF rectangle) { return WorldFrustum.Intersects(rectangle); }
        public bool CircleOnWorldView(CircleF circle) { return WorldFrustum.Intersects(circle); }
    }
}
