using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.Rendering.Renderer
{
    public class Renderer
    {
        private readonly FrustumCuller mFrustumCuller = new();
        public RectangleF ViewScreen { get; private set; }
        public RectangleF WorldScreen { get; private set; }

        public void Update(int screenHight, int screenWidth, Matrix viewTransformation)
        {
            mFrustumCuller.Update(screenWidth, screenHight, viewTransformation);
            ViewScreen = mFrustumCuller.mViewFrustum;
            WorldScreen = mFrustumCuller.mWorldFrustum;
        }

        public void RenderGameObjectList(TextureManager textureManager, List<GameObject.GameObject> objects)
        {
            foreach (GameObject.GameObject obj in objects)
            {
                if (WorldScreen.Intersects(obj.BoundedBox))
                {
                    obj.Draw(textureManager);
                }
            }
        }
    }
}
