using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObjects.Types;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Game.Layers;
using Microsoft.Xna.Framework;
using System;

namespace Galaxy_Explovive.Game.GameObjects.Astronomical_Body
{
    [Serializable]
    public class Star : AstronomicalBody
    {
        public Color mLightColor;
        public StarType mType;

        public Star(GameLayer gameLayer, Vector2 position) : base(gameLayer)
        {
            int randIndex = MyUtility.Random.Next(StarTypes.Types.Count);
            mType = StarTypes.Types[randIndex];
            // Location
            Position = position;
            Rotation = 0;

            // Rendering
            TextureId = mType.Texture;
            TextureWidth = 1024;
            TextureHeight = 1024;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureScale = mType.Size;
            TextureDepth = 1;
            TextureColor = Color.White;
            mLightColor = mType.StarColor;

            // Add To Spatial Hashing
            mSpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
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
            TextureManager.Instance.Draw("StarLightAlpha", Position, TextureOffset, TextureScale * 1.3f, 0, 2, mLightColor);
            Globals.DebugSystem.DrawBoundBox(BoundedBox);
        }
    }
}
