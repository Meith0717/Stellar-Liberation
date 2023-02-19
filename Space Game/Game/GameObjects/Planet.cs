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
        [JsonProperty] private double mAddAllois;
        [JsonProperty] private double mAddEnergy;
        [JsonProperty] private double mAddCrystals;
        [JsonProperty] private PlanetType mPlanetType;
        
        // Hover
        private CrossHair mCrossHair;


        // Type Enumerartion
        public enum PlanetType
        {
            H,
            J,
            M,
            Y
        }

        public Planet(int radius, Vector2 CenterPosition) 
        {
            float newX = radius * MathF.Cos(0);
            float newY = radius * MathF.Sin(0);

            // Set rabdom Type 
            Array starTypes = Enum.GetValues(typeof(PlanetType));
            mPlanetType = (PlanetType)starTypes.GetValue(Globals.mRandom.Next(starTypes.Length));

            // Inizialize some Stuff
            TextureHeight = textureHeight; TextureWidth = textureWidth;
            Offset = new Vector2(textureWidth, textureHeight) / 2;
            Position = new Vector2(newX, newY) + CenterPosition;
            HoverBox = new CircleF(Position, MathF.Max(textureWidth / 10f, textureHeight / 10f));
            mCrossHair = new CrossHair(0.07f, 0.095f, Position);

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
            mCrossHair.Draw();

            // Draw Planet
            var planet = TextureManager.GetInstance().GetTexture(TextureId);
            TextureManager.GetInstance().GetSpriteBatch().Draw(planet, Position, null, Color.White,0,
                Offset, 0.1f, SpriteEffects.None, 0.0f);

            if (!Hover) { return; }
            // Show Recources
            string[] array = new string[] {$"Alloys: +{mAddAllois}", $"Energy: +{mAddEnergy}", $"Crystals: +{mAddCrystals}"};
            for ( int i = 0; i < array.Length; i++)
            {
                TextureManager.GetInstance().DrawString("text", Position + new Vector2(-70, 50 + 20 * i), array[i], Color.White);
            }
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            this.ManageHover(inputState, null);
            mCrossHair.Update(gameTime, inputState);
            mCrossHair.Hover = Hover;
        }
    }
}
