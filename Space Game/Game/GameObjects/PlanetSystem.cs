﻿using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using System;
using System.Collections.Generic;
using Galaxy_Explovive.Core.RayTracing;
using Galaxy_Explovive.Core.GameObjects.Types;
using Galaxy_Explovive.Core.Utility;

namespace Galaxy_Explovive.Game.GameObjects
{
    [Serializable]
    public class PlanetSystem : GameObject
    {
        const int AlphaModifier = 15;

        [JsonProperty] public StarState mState;
        [JsonProperty] public Star mStar;
        [JsonProperty] public List<Planet> mPlanets;
        [JsonProperty] private float mRadiusLimit;

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
            mRadiusLimit = mStar.TextureWidth / 2 * mStar.TextureSclae;
            for (int i = 1; i <= MyUtility.Random.Next(2, 6); i++)
            {
                PlanetType planetType = GetPlanetType(i);
                if (i != 0) { mRadiusLimit += 300 + (1000 * planetType.Size); }
                mPlanets.Add(new Planet((int)mRadiusLimit, position, mStar.mLightColor, planetType));    
            } 
            BoundedBox = new CircleF(position, mRadiusLimit+400);
            mRayTracing = new(mStar.mType.LightColor);
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
            mStar.Draw();
            mRayTracing.Draw();
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
            if (Globals.mCamera2d.mZoom > 0.1) { return; }
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
        private PlanetType GetPlanetType(int orbit)
        {
            PlanetType planetType = null;
            switch (orbit)
            {
                case 1:
                    planetType = PlanetTypes.getOrbit1;
                    break;
                case 2:
                    planetType = PlanetTypes.getOrbit2;
                    break;
                case 3:
                    planetType = PlanetTypes.getOrbit3;
                    break;
                case 4:
                    planetType = PlanetTypes.getOrbit4;
                    break;
                case 5:
                    planetType = PlanetTypes.getOrbit5;
                    break;
                case 6:
                    planetType = PlanetTypes.getOrbit6;
                    break;
            }
            return planetType;
        }
    }
}
