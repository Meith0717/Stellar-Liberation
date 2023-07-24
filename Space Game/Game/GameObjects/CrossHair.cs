
using CelestialOdyssey.Core.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Core.GameObject
{
    public class CrossHair : GameEngine.GameObjects.GameObject
    {
        public enum CrossHairType
        {
            Target, Select
        };

        private bool mHover;
        private bool mDraw;

        public CrossHair(CrossHairType type)
        {
            // Location
            Rotation = 0;

            // Rendering
            TextureId = (type == CrossHairType.Target) ? "targetCrosshair" : "selectCrosshait";
            Width = 64;
            Height = 64;
            TextureOffset = new Vector2(Width, Height) / 2;
            TextureDepth = 1000;
            TextureColor = Color.White;
        }

        public void Update(Vector2? position, float scale, Color color, bool isHover)
        {
            if (position == null) { mDraw = false; return; }
            mDraw = true;
            Position = (Vector2)position;
            TextureScale = scale;
            TextureColor = color;
            mHover = isHover;
        }

        public override void Draw(TextureManager textureManager, GameEngine.GameEngine engine)
        {
            if (!mDraw) { return; }
            textureManager.DrawGameObject(this, mHover);
        }
    }
}