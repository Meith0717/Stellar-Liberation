using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using System;
using Galaxy_Explovive.Core.GameObjects.Types;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Game.Layers;

namespace Galaxy_Explovive.Game.GameObjects
{
    [Serializable]
    public class Planet : AstronomicalBody
    {

        // Some Variables
#pragma warning disable IDE0044 // Modifizierer "readonly" hinzufügen
        [JsonProperty] private double mAddAllois;
        [JsonProperty] private double mAddEnergy;
        [JsonProperty] private double mAddCrystals;
#pragma warning restore IDE0044 // Modifizierer "readonly" hinzufügen
        [JsonProperty] private Vector2 mCenterPosition;
        [JsonProperty] public float Angle { get; private set; }

        public float mRadius;
        private float mShadowRotation;

        public Planet(GameLayer gameLayer, int radius, Vector2 CenterPosition, Color lightColor, PlanetType planetType) : base(gameLayer)
        {
            // Location
            Angle = MyUtility.Random.NextAngle();
                                                                                             
            // Rendering
            TextureId = planetType.Texture;
            TextureWidth = TextureHeight = 1024;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureScale = planetType.Size;
            TextureDepth = 1;
            TextureColor = lightColor;

            // Class Stuff
            mRadius = radius;
            mCenterPosition = CenterPosition;

        }

        public override void SelectActions(InputState inputState)
        {
            base.SelectActions(inputState);
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState)
        {
            // Other Stuff
            base.UpdateLogik(gameTime, inputState);

            // Remove From Spatial Hashing
            RemoveFromSpatialHashing();

            float velocity = MathF.Sqrt(1/(mRadius*10));   
            float angleUpdate = Angle + mGameLayer.GameTime * velocity;
            Position = MyUtility.GetVector2(mRadius, Angle + angleUpdate) + mCenterPosition;
            Rotation += 0.004f; 
            mShadowRotation = MyUtility.GetAngle(mCenterPosition, Position);

            // Add To Spatial Hashing
            AddToSpatialHashing();
        }

        public void RemoveFromSpatialHashing()
        {
            mSpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
        }

        private void AddToSpatialHashing()
        {
            mSpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
        }

        public void Draw(int alpha)
        {
            base.Draw();
            TextureColor = new Color(alpha, alpha, alpha, alpha);
            mTextureManager.Draw("planetShadow", Position, TextureOffset, TextureScale, mShadowRotation, TextureDepth + 1, TextureColor);
            mTextureManager.DrawGameObject(this, IsHover);
            mGameLayer.mDebugSystem.DrawBoundBox(mTextureManager, BoundedBox);
        }

        public override void Draw()
        {
            throw new Exception("Use The Other Draw Method please :)");
        }
    }
}
