using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.MyMath;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using System;
using Galaxy_Explovive.Core.GameObjects.Types;

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
        [JsonProperty] public float mAngle { get; private set; }

        public float mRadius;
        private float mShadowRotation;

        public Planet(int radius, Vector2 CenterPosition, Color lightColor, PlanetType planetType)
        {
            // Location
            mAngle = Globals.mRandom.NextAngle();
                                                                                             
            // Rendering
            TextureId = planetType.Texture;
            TextureWidth = TextureHeight = 1024;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureSclae = planetType.Size;
            TextureDepth = 1;
            TextureColor = lightColor;

            // Class Stuff
            mRadius = radius;
            mCenterPosition = CenterPosition;

        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            // Remove From Spatial Hashing
            RemoveFromSpatialHashing();

            // Other Stuff
            base.UpdateInputs(inputState);
            float velocity = MathF.Sqrt(1/(mRadius*10));   
            float angleUpdate = mAngle + (float)gameTime.TotalGameTime.TotalSeconds * velocity;
            Position = MyMath.Instance.GetCirclePosition(mRadius, mAngle + angleUpdate) + mCenterPosition;
            Rotation += 0.004f; 
            mShadowRotation = MyMath.Instance.GetRotation(mCenterPosition, Position);

            // Add To Spatial Hashing
            AddToSpatialHashing();
        }

        public void RemoveFromSpatialHashing()
        {
            Globals.mGameLayer.mSpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
        }

        private void AddToSpatialHashing()
        {
            Globals.mGameLayer.mSpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
        }

        public void Draw(int alpha)
        {
            DrawPlanet(alpha);
            //DrawRecources(alpha);
        }

        private void DrawPlanet(int alpha)
        {
            TextureColor = new Color(alpha, alpha, alpha, alpha);
            TextureManager.Instance.Draw("planetShadow", Position, TextureOffset, TextureSclae, mShadowRotation, TextureDepth+1, TextureColor);
            TextureManager.Instance.DrawGameObject(this, IsHover);
            //TextureManager.Instance.DrawCircle(mCenterPosition, mRadius, TextureColor, 1, 0);
            Globals.mDebugSystem.DrawBoundBox(BoundedBox);
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
