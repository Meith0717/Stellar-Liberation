using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Runtime.InteropServices;

namespace Galaxy_Explovive.Core.Rendering
{
    public class FrustumCuller
    {
        private RectangleF mWorldFrustum;
        private RectangleF mViewFrustum;

        public void Update()
        {
            Vector2 LetfTopEdge = Globals.Camera2d.ViewToWorld(Vector2.Zero);
            Vector2 ScreenEdges = Globals.Camera2d.ViewToWorld(new Vector2(Globals.GraphicsDevice.Viewport.Width,
                Globals.GraphicsDevice.Viewport.Height));
            Vector2 RigbtBottomEdge = ScreenEdges - LetfTopEdge;

            mWorldFrustum = new RectangleF(LetfTopEdge.X, LetfTopEdge.Y, RigbtBottomEdge.X, RigbtBottomEdge.Y);
            mViewFrustum = new RectangleF(0, 0, Globals.GraphicsDevice.Viewport.Width, Globals.GraphicsDevice.Viewport.Height);
        }

        public bool IsVectorOnScreenView(Vector2 position)
        {
            return mViewFrustum.Contains(position);
        }

        public bool IsGameObjectOnWorldView(GameObject.GameObject gameObject)
        {
            return mWorldFrustum.Intersects(gameObject.BoundedBox);
        }

        public void test()
        {

        }
    }
}
