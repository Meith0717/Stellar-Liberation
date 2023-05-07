using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using System;
using System.Collections.Generic;
using Galaxy_Explovive.Core.RayTracing;

namespace Galaxy_Explovive.Game.GameObjects
{
    [Serializable]
    public class PlanetSystem : GameObject
    {
        const int AlphaModifier = 15;

        [JsonProperty] public StarState mState;
        [JsonProperty] public Star mStar;
        [JsonProperty] public List<Planet> mPlanets;
        [JsonProperty] private int mRadiusLimit;

        private bool mIsSystemShown;
        private int mPlanetAlpha = 255;
        private RayTracer mRayTracing;


        public enum StarState
        {
            Uncovered,
            Discovered,
            Explored
        }

        public PlanetSystem(Vector2 position)
        {
            Position = position;
            mStar = new Star(position);
            mPlanets = new List<Planet>();
            for (int i = 0; i < Globals.mRandom.Next(2, 5); i++)
            {
                mRadiusLimit = 600 + 300 * i;
                mPlanets.Add(new Planet(mRadiusLimit, position, mStar.mLightColor));
            } 
            BoundedBox = new CircleF(position, mRadiusLimit+400);
            mRayTracing = new(mStar.mLightColor);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            // Checkst if System is on Screen
            if (!Globals.mFrustumCuller.IsGameObjectOnWorldView(this)) { return; }

            // Get Rays
            mRayTracing.GetRays(this);

            // Show or hide Systems
            ShowSystem(); HideSystem();
            mStar.Update(gameTime, inputState);

            // Update based on Cam. Positions 
            if (!mIsSystemShown) { return; }
            foreach (Planet planet in mPlanets)
            {
                planet.Update(gameTime, inputState);
            }
        }

        public override void Draw()
        {
            // Checkst if System is on Screen
            if (!Globals.mFrustumCuller.IsGameObjectOnWorldView(this)) { return; }
            
            // Draw Stuff
            mRayTracing.Draw();
            mStar.Draw();
            Globals.mDebugSystem.DrawBoundBox(BoundedBox);

            // Draw based on Cam. Positions 
            if (!mIsSystemShown) { return; }
            foreach (Planet planet in mPlanets)
            {
                planet.Draw(mPlanetAlpha);
            }
        }

        private void ShowSystem()
        {
            if (Globals.mCamera2d.mZoom < 0.1) { return; }
            ShowPlanets();
        }
        private void HideSystem()
        {
            if (Globals.mCamera2d.mZoom > 0.1) { return; }
            HidePlanets();
        }
        private void ShowPlanets()
        {
            mIsSystemShown = true;
            if (mPlanetAlpha <= 255 - AlphaModifier)
            {
                mPlanetAlpha += AlphaModifier;
                return;
            }
            mPlanetAlpha = 255;
        }
        private void HidePlanets()
        {
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
            }
            mIsSystemShown = false;
        }
    }
}
