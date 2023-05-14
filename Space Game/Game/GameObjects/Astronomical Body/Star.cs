using Microsoft.Xna.Framework;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using System;
using Galaxy_Explovive.Core.GameObjects.Types;

namespace Galaxy_Explovive.Game.GameObjects.Astronomical_Body
{
    [Serializable]
    public class Star: AstronomicalBody
    {
        public Color mLightColor;
        public StarType mType;

        public Star(Vector2 position)
        {
            int randIndex = Globals.mRandom.Next(StarTypes.types.Count);
            mType = StarTypes.types[randIndex];
            // Location
            Position = position;
            Rotation = 0;

            // Rendering
            TextureId = mType.Texture;
            TextureWidth = 1024;
            TextureHeight = 1024;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureSclae = mType.Size;
            TextureDepth = 1;
            TextureColor = Color.White;
            mLightColor = mType.StarColor;

            // Add To Spatial Hashing
            Globals.mGameLayer.mSpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.UpdateInputs(inputState);
            Rotation += 0.0001f;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
        }

        public override void Draw()  
        {
            TextureManager.Instance.DrawGameObject(this, IsHover);
            TextureManager.Instance.Draw("StarLightAlpha", Position, TextureOffset, TextureSclae * 1.3f, 0, 2, mLightColor);
            Globals.mDebugSystem.DrawBoundBox(BoundedBox);
        }
    }
}
