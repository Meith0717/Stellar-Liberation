using GalaxyExplovive.Core.GameEngine;
using GalaxyExplovive.Core.GameEngine.GameObjects;
using GalaxyExplovive.Core.GameEngine.InputManagement;
using GalaxyExplovive.Core.GameEngine.Rendering;
using GalaxyExplovive.Core.GameEngine.Utility;
using GalaxyExplovive.Core.GameObjects.Types;
using GalaxyExplovive.Game.GameObjects.Astronomical_Body;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using System;

namespace GalaxyExplovive.Game.GameObjects
{
    [Serializable]
    public class Planet : InteractiveObject
    {
        [JsonIgnore] private float mAlpha { get; set; } = 0;

        // Some Variables
        [JsonIgnore] private float mShadowRotation;
        [JsonProperty] private Vector2 mCenterPosition;
        [JsonProperty] private readonly PlanetType mPlanetType;
        [JsonProperty] private readonly float mAngle;
        [JsonProperty] private readonly float mRadius;

        public Planet(int orbitNr, Vector2 CenterPosition, Color StarColor, int StarSize) : base()
        {
            // Location
            mAngle = Utility.Random.NextAngle();
            mPlanetType = GetPlanetType(orbitNr);

            // Rendering
            if (mPlanetType != null)
            {
                TextureId = mPlanetType.Texture;
                TextureScale = mPlanetType.Size;
                mRadius = StarSize + (300 * orbitNr) + (1000 * mPlanetType.Size);
            }
            Width = Height = 1024;
            TextureOffset = new Vector2(Width, Height) / 2;
            TextureDepth = 1;
            TextureColor = StarColor;

            // Class Stuff
            mCenterPosition = CenterPosition;
            Position = Geometry.GetPointOnCircle(mCenterPosition, mRadius, mAngle);
            SelectZoom = 1;
            BoundedBox = new CircleF(Position, (Math.Max(Height, Width) / 2) * TextureScale);
        }

        public override void UpdateLogic(GameTime gameTime, InputState inputState, GameEngine engine)
        {
            RemoveFromSpatialHashing(engine);
            if (mAlpha <= 0)
            {
                engine.SelectObject = (engine.SelectObject == this) ? null : engine.SelectObject;
                return;
            }

            base.UpdateLogic(gameTime, inputState, engine);

            float velocity = MathF.Sqrt(1 / (mRadius * 10));
            float angleUpdate = mAngle + (engine.GameTime / 1000) * velocity;
            Position = Geometry.GetPointOnCircle(mCenterPosition, mRadius, mAngle + angleUpdate);
            Rotation += 0.004f;
            mShadowRotation = Geometry.AngleBetweenVectors(mCenterPosition, Position);
            AddToSpatialHashing(engine);
        }

        public override void Draw(TextureManager textureManager, GameEngine engine)
        {
            if (mAlpha <= 0) return;

            base.Draw(textureManager, engine);

            TextureColor = new Color((int)mAlpha, (int)mAlpha, (int)mAlpha, (int)mAlpha);
            textureManager.Draw("planetShadow", Position, TextureOffset, TextureScale, mShadowRotation, TextureDepth + 1, Color.White);
            textureManager.DrawGameObject(this, IsHover);
            engine.DebugSystem.DrawBoundBox(textureManager, BoundedBox, engine);
        }

        private PlanetType GetPlanetType(int orbit)
        {
            PlanetType planetType = null;
            switch (orbit)
            {
                case 1:
                    planetType = PlanetTypes.GetOrbit1;
                    break;
                case 2:
                    planetType = PlanetTypes.GetOrbit2;
                    break;
                case 3:
                    planetType = PlanetTypes.GetOrbit3;
                    break;
                case 4:
                    planetType = PlanetTypes.GetOrbit4;
                    break;
                case 5:
                    planetType = PlanetTypes.GetOrbit5;
                    break;
                case 6:
                    planetType = PlanetTypes.GetOrbit6;
                    break;
            }
            return planetType;
        }

        public void IncreaseVisibility()
        {
            if (mAlpha == 255) { return; }
            mAlpha = (mAlpha >= 255) ? 255 : mAlpha + 50;
        }

        public void DecreaseVisibility()
        {
            if (mAlpha == 0) { return; }
            mAlpha = (mAlpha <= 0) ? 0 : mAlpha - 50;
        }
    }
}
