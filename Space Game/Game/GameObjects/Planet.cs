using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using MonoGame.Extended;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core;
using Space_Game.Core.GameObject;
using System;
using System.Runtime.InteropServices;
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
        [JsonProperty] private float mAngle;
        [JsonProperty] private float mRadius;
        [JsonProperty] private Vector2 mCenterPosition;

        public int mAlpha;

        // Type Enumerartion
        public enum PlanetType
        {
            H,
            J,
            M,
            Y
        }

        public Planet(int radius,  Vector2 CenterPosition) 
        {

            // Set rabdom Type 
            Array starTypes = Enum.GetValues(typeof(PlanetType));
            mPlanetType = (PlanetType)starTypes.GetValue(Globals.mRandom.Next(starTypes.Length));
            mAlpha = 0; mAngle = (float)Globals.mRandom.NextDouble() * (MathF.PI * 2);
            mRadius = radius; mCenterPosition = CenterPosition;

            float newX = radius * MathF.Cos(mAngle);
            float newY = radius * MathF.Sin(mAngle);

            // Inizialize some Stuff
            TextureHeight = textureHeight; TextureWidth = textureWidth;
            Offset = new Vector2(textureWidth, textureHeight) / 2;
            Position = new Vector2(newX, newY) + CenterPosition;
            HoverBox = new CircleF(Position, MathF.Max(textureWidth / 10f, textureHeight / 10f));

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

        public override void Update(GameTime gameTime, InputState inputState)
        {
        }

        public override void Draw()
        {
            // Draw Planet
            var planet = TextureManager.GetInstance().GetTexture(TextureId);
            TextureManager.GetInstance().GetSpriteBatch().Draw(planet, Position, null, 
                new Color(mAlpha, mAlpha, mAlpha, mAlpha), 0, Offset, 0.1f, SpriteEffects.None, 1);

            // Draw Orbit
            TextureManager.GetInstance().GetSpriteBatch().DrawCircle(mCenterPosition, mRadius, 50,
                new Color(mAlpha, mAlpha, mAlpha, mAlpha), 2f / Globals.mCamera2d.mZoom, 0);

            // Show Recources
            string[] array = new string[] {$"Alloys: +{mAddAllois}", $"Energy: +{mAddEnergy}", $"Crystals: +{mAddCrystals}"};
            for ( int i = 0; i < array.Length; i++)
            {
                TextureManager.GetInstance().DrawString("text", Position + new Vector2(-70, 50 + 20 * i), array[i],
                    new Color(mAlpha, mAlpha, mAlpha, mAlpha));
            }
        }
    }
}
