using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Galaxy_Explovive.Core.Rendering
{
    public class FrustumCuller
    {
        private RectangleF mViewFrustum;

        public void Update()
        {
            Vector2 LetfTopEdge = Globals.mCamera2d.ViewToWorld(Vector2.Zero);
            Vector2 ScreenEdges = Globals.mCamera2d.ViewToWorld(new Vector2(Globals.mGraphicsDevice.Viewport.Width,
                Globals.mGraphicsDevice.Viewport.Height));
            Vector2 RigbtBottomEdge = ScreenEdges - LetfTopEdge;

            mViewFrustum = new RectangleF(LetfTopEdge.X, LetfTopEdge.Y, RigbtBottomEdge.X, RigbtBottomEdge.Y);
        }

        public bool IsOnScreen(GameObject.GameObject gameObject)
        {
            return mViewFrustum.Intersects(gameObject.BoundedBox);
        }

    }
}
