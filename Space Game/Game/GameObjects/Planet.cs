using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;
using Space_Game.Core;
using Space_Game.Core.GameObject;
using Space_Game.Core.InputManagement;
using Space_Game.Core.Maths;
using Space_Game.Core.TextureManagement;
using System;

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
        [JsonProperty] private SpaceStation mSpaceStation;

        public float mAlpha;

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
            // Set rabdom Type 
            Array starTypes = Enum.GetValues(typeof(PlanetType));
            mPlanetType = (PlanetType)starTypes.GetValue(Globals.mRandom.Next(starTypes.Length));

            mAlpha = 0; 
            mAngle = (float)Globals.mRandom.NextDouble() * (MathF.PI * 2);
            mRadius = radius; 
            mCenterPosition = CenterPosition;

            // Inizialize some Stuff
            TextureSclae = 0.15f;
            TextureWidth = textureWidth;
            TextureHeight = textureHeight;
            TextureDepth = 1;
            TextureRotation = 0;
            TextureColor = Color.White;

            TextureOffset = new Vector2(textureWidth, textureHeight) / 2;
            Position = MyMathF.GetInstance().GetCirclePosition(mRadius, mAngle, 0) + mCenterPosition;
            Globals.mGameLayer.mSpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
            mSpaceStation = new SpaceStation(Position + new Vector2(50, 0));

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
            Globals.mGameLayer.mSpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
            mAngle += (0.005f * Globals.mTimeWarp) / mRadius;
            Position = MyMathF.GetInstance().GetCirclePosition(mRadius, mAngle, 0) + mCenterPosition;
            TextureRotation = MyMathF.GetInstance().GetRotation(Position - mCenterPosition);
            Globals.mGameLayer.mSpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
            mSpaceStation.Update(Position + new Vector2(50, 0));
        }

        public override void Draw()
        {
            DrawPlanet();
            DrawRecources();
            mSpaceStation.Draw();
        }
        // Draw Stuff ___________________________________________________
        private void DrawPlanet()
        {
            var texture = TextureManager.GetInstance().GetTexture(TextureId);
            TextureManager.GetInstance().GetSpriteBatch().Draw(texture, Position, null,
                new Color(mAlpha, mAlpha, mAlpha, mAlpha), TextureRotation, TextureOffset, TextureSclae, SpriteEffects.None, TextureDepth);
        }
        private void DrawRecources()
        {
            string[] array = new string[] { $"Alloys: +{mAddAllois}", $"Energy: +{mAddEnergy}", $"Crystals: +{mAddCrystals}" };
            for (int i = 0; i < array.Length; i++)
            {
                TextureManager.GetInstance().DrawString("text", Position + new Vector2(-70, 50 + 20 * i), array[i],
                    new Color(mAlpha, mAlpha, mAlpha, mAlpha));
            }
        }
    }
}
