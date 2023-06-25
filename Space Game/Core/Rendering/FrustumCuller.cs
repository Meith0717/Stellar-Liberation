using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Galaxy_Explovive.Core.Rendering
{
    public class FrustumCuller
    {
        private RectangleF mWorldFrustum;
        private RectangleF mViewFrustum;

        public void Update(int screenWidth, int screenHeight, Matrix ViewTransformationMatrix)
        {   Matrix inverse = Matrix.Invert(ViewTransformationMatrix);
            Vector2 LetfTopEdge = Vector2.Transform(Vector2.Zero, inverse);
            Vector2 RigbtBottomEdge = Vector2.Transform(new Vector2(screenWidth, screenHeight), inverse)
                - LetfTopEdge;
            mWorldFrustum = new RectangleF(LetfTopEdge.X, LetfTopEdge.Y, RigbtBottomEdge.X, RigbtBottomEdge.Y);
            mViewFrustum = new RectangleF(0, 0, screenWidth, screenHeight);
        }

        public bool VectorOnScreenView(Vector2 position) { return mViewFrustum.Contains(position); }
        public bool RectangleOnScreenView(RectangleF rectangle) { return mViewFrustum.Intersects(rectangle); }
        public bool CircleOnScreenView(CircleF circle) { return mViewFrustum.Intersects(circle); }

        public bool VectorOnWorldView(Vector2 position) { return mWorldFrustum.Contains(position); }
        public bool RectangleOnWorldView(RectangleF rectangle) { return mWorldFrustum.Intersects(rectangle); }
        public bool CircleOnWorldView(CircleF circle) { return mWorldFrustum.Intersects(circle); }
    }
}
