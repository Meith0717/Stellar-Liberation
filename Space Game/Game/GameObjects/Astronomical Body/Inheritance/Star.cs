using Microsoft.Xna.Framework;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using System;

namespace Galaxy_Explovive.Game.GameObjects.Astronomical_Body
{
    [Serializable]
    public class Star: AstronomicalBody
    {
        private CrossHair mCrosshair;
        public enum StarType
        {
            B,
            F,
            G,
            K,
            M
        }
        public Star(Vector2 position)
        {
            // Location
            Position = position;
            Rotation = 0;

            // Rendering
            NormalTextureId = "IS SET IN GetSystemTypeAndTexture";
            TextureWidth = 1024;
            TextureHeight = 1024;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureSclae = 1f;
            TextureDepth = 0;
            TextureColor = Color.White;

            // Selection Stuff
            HoverTextureId = "IS SET IN GetSystemTypeAndTexture";
            HoverRadius = 270;
            IsHover = false;
            IsSelect = false;

            // Other Stuff
            GetSystemTypeAndTexture();
            mCrosshair = new CrossHair(0.8f, 0.9f, position);
            // Add To Spatial Hashing
            Globals.mGameLayer.mSpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
        }
        private void GetSystemTypeAndTexture()
        {
            Array starTypes = Enum.GetValues(typeof(StarType));
            StarType starType = (StarType)starTypes.GetValue(Globals.mRandom.Next(starTypes.Length));
            switch (starType)
            {
                case StarType.B:
                    {
                        NormalTextureId = HoverTextureId = "sunTypeB";
                        break;
                    }
                case StarType.F:
                    {
                        NormalTextureId = HoverTextureId = "sunTypeF";
                        break;
                    }
                case StarType.G:
                    {
                        NormalTextureId = HoverTextureId = "sunTypeG";
                        break;
                    }
                case StarType.K:
                    {
                        NormalTextureId = HoverTextureId = "sunTypeK";
                        break;
                    }
                case StarType.M:
                    {
                        NormalTextureId = HoverTextureId = "sunTypeM";
                        break;
                    }
            }
        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
            mCrosshair.Update(Position, IsHover);
            UpdateInputs(inputState);
        }
        public override void UpdateInputs(InputState inputState)
        {
             base.UpdateInputs(inputState);
        }
        public override void Draw()
        {
            TextureManager.GetInstance().Draw(IsHover ? HoverTextureId : NormalTextureId, Position, TextureOffset,
                TextureWidth, TextureHeight, TextureSclae, Rotation, TextureDepth);
            mCrosshair.Draw(Color.White);
        }
    }
}
