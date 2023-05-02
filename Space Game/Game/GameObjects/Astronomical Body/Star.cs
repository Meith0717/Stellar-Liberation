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
            TextureId = "IS SET IN GetSystemTypeAndTexture";
            TextureWidth = 1024;
            TextureHeight = 1024;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureSclae = 1f;
            TextureDepth = 0;
            TextureColor = Color.White;

            // Selection Stuff
            TextureRadius = 270;

            // Other Stuff
            GetSystemTypeAndTexture();
            Crosshair = new CrossHair(0.8f, 0.9f, position);
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
                        TextureId = "sunTypeB";
                        break;
                    }
                case StarType.F:
                    {
                        TextureId = "sunTypeF";
                        break;
                    }
                case StarType.G:
                    {
                        TextureId  = "sunTypeG";
                        break;
                    }
                case StarType.K:
                    {
                        TextureId = "sunTypeK";
                        break;
                    }
                case StarType.M:
                    {
                        TextureId = "sunTypeM";
                        break;
                    }
            }
        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.UpdateInputs(inputState);
        }

        public override void Draw()
        {
            TextureManager.Instance.Draw(TextureId, Position, TextureOffset,
                TextureWidth, TextureHeight, TextureSclae, Rotation, TextureDepth);
            Crosshair.Draw(Color.White);
        }
    }
}
