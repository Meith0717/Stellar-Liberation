using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core;
using Space_Game.Core.GameObject;
using System;
using static Space_Game.Game.GameObjects.PlanetSystem;

namespace Space_Game.Game.GameObjects
{
    [Serializable]
    public class Planet : GameObject
    {
        const int textureHeight = 512;
        const int textureWidth = 512;

        [JsonProperty] private int mRadius;
        [JsonProperty] private PlanetType mPlanetType;
        [JsonProperty] private string mClass;
        [JsonProperty] public double mAddAllois;
        [JsonProperty] public double mAddEnergy;
        [JsonProperty] public double mAddCrystals;

        public enum PlanetType
        {
            H,
            J,
            M,
            Y
        }


        public Planet(int radius) 
        {
            Array starTypes = Enum.GetValues(typeof(PlanetType));
            mPlanetType = (PlanetType)starTypes.GetValue(Globals.mRandom.Next(starTypes.Length));

            mRadius = radius;
            Offset = new Vector2(textureWidth, textureHeight) / 2;
            TextureHeight = textureHeight;
            TextureWidth = textureWidth;

            switch (mPlanetType)
            {
                case PlanetType.H:
                    {
                        TextureId = "planetTypeH";
                        mClass = "H";
                        mAddAllois = 0.1;
                        mAddEnergy = 0.1;
                        mAddCrystals = 0.1;
                        break;
                    }
                case PlanetType.J:
                    {
                        TextureId = "planetTypeJ";
                        mClass = "J";
                        mAddAllois = 0.1;
                        mAddEnergy = 10;
                        mAddCrystals = 2;
                        break;
                    }
                case PlanetType.M:
                    {
                        TextureId = "planetTypeM";
                        mClass = "M";
                        mAddAllois = 5;
                        mAddEnergy = 10;
                        mAddCrystals = 5;
                        break;
                    }
                case PlanetType.Y:
                    {
                        TextureId = "planetTypeY";
                        mClass = "Y";
                        mAddAllois = 15;
                        mAddEnergy = 20;
                        mAddCrystals = 15;
                        break;
                    }
            }

        }

        public void SetNewRadius(int radius)
        {
            mRadius = radius;
        }

        public override void Draw()
        {
            var planet = TextureManager.GetInstance().GetTexture(TextureId);
            TextureManager.GetInstance().GetSpriteBatch().Draw(planet, Position, null, Color.White,
                0, Offset, 0.2f, SpriteEffects.None, 0.0f);
            var textPosition = Position + new Vector2(-70, 50);
            TextureManager.GetInstance().DrawString("text", textPosition, $"Class: {mClass}", Color.White);
            TextureManager.GetInstance().DrawString("text", textPosition + new Vector2(0, 20), $"Level: 0", Color.White);
            TextureManager.GetInstance().DrawString("text", textPosition + new Vector2(0, 50), $"Alloys: {mAddAllois}", Color.White);
            TextureManager.GetInstance().DrawString("text", textPosition + new Vector2(0, 70), $"Energy: {mAddEnergy}", Color.White);
            TextureManager.GetInstance().DrawString("text", textPosition + new Vector2(0, 90), $"Crystals: {mAddEnergy}", Color.White);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            Position = new Vector2(mRadius, Globals.mGraphicsDevice.Viewport.Height / 2);
        }
    }
}
