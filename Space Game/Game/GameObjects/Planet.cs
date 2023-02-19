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
        // Constants
        const int textureHeight = 512;
        const int textureWidth = 512;

        // Some Variables
        [JsonProperty] private int mRadius;
        [JsonProperty] private double mAddAllois;
        [JsonProperty] private double mAddEnergy;
        [JsonProperty] private double mAddCrystals;
        [JsonProperty] private PlanetType mPlanetType;

        // Type Enumerartion
        public enum PlanetType
        {
            H,
            J,
            M,
            Y
        }

        public Planet(int radius) 
        {
            // Set rabdom Type 
            Array starTypes = Enum.GetValues(typeof(PlanetType));
            mPlanetType = (PlanetType)starTypes.GetValue(Globals.mRandom.Next(starTypes.Length));

            // Inizialize some Stuff
            mRadius = radius; TextureHeight = textureHeight; TextureWidth = textureWidth;
            Offset = new Vector2(textureWidth, textureHeight) / 2;

            // Inizialize Type Stuff
            switch (mPlanetType)
            {
                case PlanetType.H:
                    {
                        TextureId = "planetTypeH";
                        mAddAllois = 0.1;
                        mAddEnergy = 0.1;
                        mAddCrystals = 0.1;
                        break;
                    }
                case PlanetType.J:
                    {
                        TextureId = "planetTypeJ";
                        mAddAllois = 0.1;
                        mAddEnergy = 10;
                        mAddCrystals = 2;
                        break;
                    }
                case PlanetType.M:
                    {
                        TextureId = "planetTypeM";
                        mAddAllois = 5;
                        mAddEnergy = 10;
                        mAddCrystals = 5;
                        break;
                    }
                case PlanetType.Y:
                    {
                        TextureId = "planetTypeY";
                        mAddAllois = 15;
                        mAddEnergy = 20;
                        mAddCrystals = 15;
                        break;
                    }
            }
        }

        public override void Draw()
        {
            // Draw Planet
            var planet = TextureManager.GetInstance().GetTexture(TextureId);
            TextureManager.GetInstance().GetSpriteBatch().Draw(planet, Position, null, Color.White,0, Offset, 0.1f, SpriteEffects.None, 0.0f);

            // Show Recources
            string[] array = new string[] {$"Alloys: +{mAddAllois}", $"Energy: +{mAddEnergy}", $"Crystals: +{mAddCrystals}"};
            for ( int i = 0; i < array.Length; i++)
            {
                TextureManager.GetInstance().DrawString("text", Position + new Vector2(-70, 50 + 20 * i), array[i], Color.White);
            }
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            // Update Position after Window size has changed
            Position = new Vector2(mRadius, Globals.mGraphicsDevice.Viewport.Height / 2);
        }
    }
}
