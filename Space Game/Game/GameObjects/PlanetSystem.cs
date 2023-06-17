using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using System;
using System.Collections.Generic;
using Galaxy_Explovive.Core.RayTracing;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Core.TextureManagement;

namespace Galaxy_Explovive.Game.GameObjects
{
    [Serializable]
    public class PlanetSystem : GameObject
    {
        const int AlphaModifier = 15;

        [JsonProperty] public StarState mState;
        [JsonProperty] public Star mStar;
        [JsonProperty] public List<Planet> mPlanets;
        [JsonProperty] private readonly float mRadiusLimit;

        [JsonProperty] private bool mIsSystemShown;
        [JsonProperty] private int mPlanetAlpha = 255;
        [JsonIgnore] private RayTracer mRayTracing;

        public enum StarState
        {
            Uncovered,
            Discovered,
            Explored
        }

        public PlanetSystem(Vector2 position) : base()
        {
            Position = position;
            mStar = new Star(position);
            mPlanets = new List<Planet>();
            mRadiusLimit = mStar.TextureWidth / 2 * mStar.TextureScale;
            int orbitNr;
            for (orbitNr = 1; orbitNr <= MyUtility.Random.Next(2, 6); orbitNr++)
            {
                mPlanets.Add(new Planet(orbitNr, position, mStar.mLightColor, (int)mRadiusLimit));    
            } 
            BoundedBox = new CircleF(position, mRadiusLimit + (orbitNr * 400));
            mRayTracing = new(mStar.mType.LightColor, GameGlobals.SpatialHashing);
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState)
        {
            // Checkst if System is on Screen
            if (!GameGlobals.FrustumCuller.IsGameObjectOnWorldView(this)) { return; }

            // Get Rays
            mRayTracing.GetRays(this, GameGlobals.SpatialHashing, GameGlobals.Camera.Zoom);

            // Show or hide Systems
            ShowSystem(); HideSystem();
            mStar.UpdateLogik(gameTime, inputState);

            // Update based on Cam. Positions 
            if (!mIsSystemShown) { return; }
            foreach (Planet planet in mPlanets)
            {
                planet.UpdateLogik(gameTime, inputState);
            }
        }

        public override void Draw(TextureManager textureManager)
        {
            // Checkst if System is on Screen
            if (!GameGlobals.FrustumCuller.IsGameObjectOnWorldView(this)) { return; }
            
            // Draw Stuff
            mStar.Draw(textureManager);
            mRayTracing.Draw(textureManager);
            GameGlobals.DebugSystem.DrawBoundBox(textureManager, BoundedBox);

            // Draw based on Cam. Positions 
            if (!mIsSystemShown) { return; }
            foreach (Planet planet in mPlanets)
            {
                planet.Draw(mPlanetAlpha, textureManager);
            }
        }

        private void ShowSystem()
        {
            if (GameGlobals.Camera.Zoom < 0.1) { return; }
            mIsSystemShown = true;
            if (mPlanetAlpha <= 255 - AlphaModifier)
            {
                mPlanetAlpha += AlphaModifier;
                return;
            }
            mPlanetAlpha = 255;
        }
        private void HideSystem()
        {
            if (GameGlobals.Camera.Zoom > 0.1) { return; }
            if (!mIsSystemShown) { return; }
            if (mPlanetAlpha >= AlphaModifier)
            {
                mPlanetAlpha -= AlphaModifier;
                return;
            }
            mPlanetAlpha = 0;
            foreach (Planet p in mPlanets) 
            { 
                p.RemoveFromSpatialHashing();
                if (GameGlobals.SelectObject != p) continue;
                GameGlobals.SelectObject = null;
            }
            mIsSystemShown = false;
        }
    }
}
