using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using System;
using Galaxy_Explovive.Core.GameObjects.Types;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core;

namespace Galaxy_Explovive.Game.GameObjects
{
    [Serializable]
    public class Planet : AstronomicalBody
    {

        // Some Variables
        [JsonIgnore] private float mShadowRotation;
        [JsonProperty] private Vector2 mCenterPosition;
        [JsonProperty] private PlanetType mPlanetType;
        [JsonProperty] private float Angle;
        [JsonProperty] public float mRadius;

        public Planet(int orbitNr, Vector2 CenterPosition, Color StarColor, int StarSize) : base()
        {
            // Location
            Angle = MyUtility.Random.NextAngle();
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
            Position = MyUtility.GetVector2(mRadius, Angle) + mCenterPosition;
            SelectZoom = 1;
        }

        public override void UpdateLogic(GameTime gameTime, InputState inputState, GameEngine engine)
        {
            base.UpdateLogic(gameTime, inputState, engine);

            float velocity = MathF.Sqrt(1/(mRadius*10));
            float angleUpdate = Angle + (engine.GameTime/1000) * velocity;
            Position = MyUtility.GetVector2(mRadius, Angle + angleUpdate) + mCenterPosition;
            Rotation += 0.004f; 
            mShadowRotation = MyUtility.GetAngle(mCenterPosition, Position);
        }

        public override void Draw(TextureManager textureManager, GameEngine engine)
        {
            base.Draw(textureManager, engine);
            TextureColor = new Color(100, 100, 100, 100);
            textureManager.Draw("planetShadow", Position, TextureOffset, TextureScale, mShadowRotation, TextureDepth + 1, TextureColor);
            textureManager.DrawGameObject(this, IsHover);
            engine.DebugSystem.DrawBoundBox(textureManager, BoundedBox);
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

    }
}
