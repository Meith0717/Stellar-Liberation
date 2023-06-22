using Microsoft.Xna.Framework;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Game;

namespace Galaxy_Explovive.Core.GameObject
{
    public class CrossHair : GameObject
    {
        public enum CrossHairType
        {
            Target, Select
        };

        private bool mHover;
        private bool mDraw;

        public CrossHair(Vector2 position, float scale, CrossHairType type)
        {
            // Location
            Position = position;
            Rotation = 0;

            // Rendering
            TextureId = (type == CrossHairType.Target) ? "targetCrosshair" : "selectCrosshait";
            TextureWidth = 64;
            TextureHeight = 64;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureScale = scale;
            TextureDepth = null;
            TextureColor = Color.White;
        }

        public void Update(Vector2? position, float scale, Color color, bool isHover) 
        {
            if (position  == null) { mDraw = false; return; }
            mDraw = true;
            Position = (Vector2)position;
            TextureScale = scale;
            TextureColor = color;
            mHover = isHover;
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState)
        {
            throw new System.NotImplementedException();
        }
        public override void Draw(TextureManager textureManager)
        {
            if (!mDraw) { return; }
            textureManager.DrawGameObject(this, mHover);
        }
    }
}