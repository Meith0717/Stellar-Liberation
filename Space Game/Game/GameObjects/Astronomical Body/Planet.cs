using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.MyMath;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using System;

namespace Galaxy_Explovive.Game.GameObjects
{
    [Serializable]
    public class Planet : AstronomicalBody
    {

        // Some Variables
        [JsonProperty] private double mAddAllois;
        [JsonProperty] private double mAddEnergy;
        [JsonProperty] private double mAddCrystals;
        [JsonProperty] private PlanetType mPlanetType;
        [JsonProperty] private Vector2 mCenterPosition;
        [JsonProperty] private float mAngle;

        private float mRadius;

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
            // Location
            mAngle = Globals.mRandom.NextAngle();
                                                                                             
            // Rendering
            TextureId = "IS SET IN GetSystemTypeAndTexture";
            TextureWidth = TextureHeight = 1024;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureSclae = 0.3f;
            TextureDepth = 0;
            TextureColor = Color.White;

            // Selection Stuff
            TextureRadius = 270;

            // Class Stuff
            mRadius = radius;
            mCenterPosition = CenterPosition;

            // Other Stuff
            GetPlanetTypeAndTexture();
            Crosshair = new CrossHair(0.3f, 0.35f, Position);

        }
        private void GetPlanetTypeAndTexture()
        {
            // Set rabdom Type 
            Array starTypes = Enum.GetValues(typeof(PlanetType));
            mPlanetType = (PlanetType)starTypes.GetValue(Globals.mRandom.Next(starTypes.Length));

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
            // Remove From Spatial Hashing
            Globals.mGameLayer.mSpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);

            // Other Stuff
            base.UpdateInputs(inputState);
            float velocity = MathF.Sqrt(1/(mRadius*100));   
            float angleUpdate = mAngle + (float)gameTime.TotalGameTime.TotalSeconds * velocity;
            Position = MyMath.Instance.GetCirclePosition(mRadius, mAngle + angleUpdate, 0) + mCenterPosition;
            Rotation = MyMath.Instance.GetRotation(mCenterPosition, Position);

            // Add To Spatial Hashing
            Globals.mGameLayer.mSpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
        }

        public void Draw(int alpha)
        {
            DrawPlanet(alpha);
            DrawRecources(alpha);
            Crosshair.Draw(new Color(alpha, alpha, alpha, alpha));
        }
        private void DrawPlanet(int alpha)
        {
            TextureManager.Instance.Draw(TextureId, Position, TextureOffset,
                TextureWidth, TextureHeight, TextureSclae, Rotation, 1, new Color(alpha, alpha, alpha, alpha));

            TextureManager.Instance.DrawCircle(mCenterPosition, mRadius, new Color(alpha, alpha, alpha, alpha), 1, 0);
        }
        private void DrawRecources(int alpha)
        {
            string[] array = new string[] { $"Alloys: +{mAddAllois}", $"Energy: +{mAddEnergy}", $"Crystals: +{mAddCrystals}" };
            for (int i = 0; i < array.Length; i++)
            {
                TextureManager.Instance.DrawString("text", Position + new Vector2(0, 200 + 20 * i), array[i],
                    new Color(alpha, alpha, alpha, alpha));
            }
        }
        public override void Draw()
        {
            throw new Exception("Use The Other Draw Method please :)");
        }
    }
}
