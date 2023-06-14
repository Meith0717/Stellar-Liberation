using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Runtime.InteropServices;

namespace Galaxy_Explovive.Core.Rendering
{
    public class FrustumCuller
    {
        private RectangleF mWorldFrustum;
        private RectangleF mViewFrustum;

        public void Update(GraphicsDevice graphicsDevice, Func<Vector2, Vector2> ViewToWorld)
        {
            Vector2 LetfTopEdge = ViewToWorld(Vector2.Zero);
            Vector2 ScreenEdges = ViewToWorld(new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));
            Vector2 RigbtBottomEdge = ScreenEdges - LetfTopEdge;

            mWorldFrustum = new RectangleF(LetfTopEdge.X, LetfTopEdge.Y, RigbtBottomEdge.X, RigbtBottomEdge.Y);
            mViewFrustum = new RectangleF(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
        }

        public bool IsVectorOnScreenView(Vector2 position)
        {
            return mViewFrustum.Contains(position);
        }

        public bool IsVectorOnWorldView(RectangleF rectangle)
        {
            return mWorldFrustum.Intersects(rectangle);
        }

        public bool IsGameObjectOnWorldView(GameObject.GameObject gameObject)
        {
            return mWorldFrustum.Intersects(gameObject.BoundedBox);
        }
    }
}
