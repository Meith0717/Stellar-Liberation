using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using System.Data;

namespace Galaxy_Explovive.Core.GameObject
{
    public class CrossHair : GameObject
    {
        private bool mHover;

        public CrossHair(Vector2 position, float scale)
        {
            // Location
            Position = position;
            Rotation = 0;

            // Rendering
            TextureId = "crossHair1";
            TextureWidth = 1024;
            TextureHeight = 1024;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureSclae = scale;
            TextureDepth = 0;
            TextureColor = Color.White;
        }

        public void Update(Vector2 position, float scale, Color color, bool isHover) 
        {
            Position = position;
            TextureSclae = scale;
            TextureColor = color;
            mHover = isHover;
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            throw new System.NotImplementedException();
        }
        public override void Draw()
        {
            TextureManager.Instance.DrawGameObject(this, mHover);
        }
    }
}