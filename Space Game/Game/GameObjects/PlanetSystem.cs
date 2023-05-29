using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
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
        [JsonProperty] private readonly float mRadiusLimit;

        private bool mIsSystemShown;
        private int mPlanetAlpha = 255;
        private RayTracer mRayTracing;


        public enum StarState
        {
            Uncovered,
            Discovered,
            Explored
        }

        public PlanetSystem(GameLayer gameLayer, Vector2 position) : base(gameLayer)
        {
            Position = position;
            mStar = new Star(gameLayer, position);
            mPlanets = new List<Planet>();
            mRadiusLimit = mStar.TextureWidth / 2 * mStar.TextureScale;
            for (int i = 1; i <= MyUtility.Random.Next(2, 6); i++)
            {
                PlanetType planetType = GetPlanetType(i);
                if (i != 0) { mRadiusLimit += 300 + (1000 * planetType.Size); }
                mPlanets.Add(new Planet(gameLayer, (int)mRadiusLimit, position, mStar.mLightColor, planetType));    
            } 
            BoundedBox = new CircleF(position, mRadiusLimit+400);
            mRayTracing = new(mStar.mType.LightColor, gameLayer.mSpatialHashing);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            // Checkst if System is on Screen
            if (!mFrustumCuller.IsGameObjectOnWorldView(this)) { return; }

            // Get Rays
            mRayTracing.GetRays(this, mSpatialHashing, mGameLayer.mCamera.Zoom);

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
            if (!mFrustumCuller.IsGameObjectOnWorldView(this)) { return; }
            
            // Draw Stuff
            mStar.Draw();
            mRayTracing.Draw(mTextureManager);
            mGameLayer.mDebugSystem.DrawBoundBox(mTextureManager, BoundedBox);

            // Draw based on Cam. Positions 
            if (!mIsSystemShown) { return; }
            foreach (Planet planet in mPlanets)
            {
                planet.Draw(mPlanetAlpha);
            }
        }

        private void ShowSystem()
        {
            if (mGameLayer.mCamera.Zoom < 0.1) { return; }
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
            if (mGameLayer.mCamera.Zoom > 0.1) { return; }
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
