using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using System.Data;
using Galaxy_Explovive.Core.Rendering;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Game.Layers;

namespace Galaxy_Explovive.Core.GameObject
{
    public class CrossHair : GameObject
    {
        private bool mHover;

        public CrossHair(GameLayer gameLayer, Vector2 position, float scale) : base(gameLayer)
        {
            // Location
            Position = position;
            Rotation = 0;

            // Rendering
            TextureId = "crossHair1";
            TextureWidth = 1024;
            TextureHeight = 1024;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureScale = scale;
            TextureDepth = 0;
            TextureColor = Color.White;
        }

        public void Update(Vector2 position, float scale, Color color, bool isHover) 
        {
            Position = position;
            TextureScale = scale;
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