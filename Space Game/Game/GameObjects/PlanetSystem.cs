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
        const int AlphaModifier = 25;

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
                mRadiusLimit = 1200 + 600 * i;
                mPlanets.Add(new Planet(mRadiusLimit, position, mStar.mLightColor));
            } 
            BoundedBox = new CircleF(position, mRadiusLimit+400);
            mRayTracing = new(mStar.mLightColor);
        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            ShowSystem(); HideSystem();
            mStar.Update(gameTime, inputState);
            if (!mIsSystemShown) { return; }
            mRayTracing.GetRays(this);
            foreach (Planet planet in mPlanets)
            {
                planet.Update(gameTime, inputState);
            }
        }
        public override void Draw()
        {
            mStar.Draw();
            Globals.mDebugSystem.DrawBoundBox(BoundedBox);
            if (!mIsSystemShown) { return; }
            mRayTracing.Draw();
            foreach (Planet planet in mPlanets)
            {
                planet.Draw(mPlanetAlpha);
            }
        }

        private void ShowSystem()
        {
            mIsSystemShown = true;
            if (!BoundedBox.Contains(Globals.mCamera2d.Position)) { return; }
            ShowPlanets();
            //DecreaseStarSize();
        }
        private void HideSystem()
        {
            if (BoundedBox.Contains(Globals.mCamera2d.Position)) { return; }
            HidePlanets();
            //IncreaseStarSize();
            mIsSystemShown = false;
        }
        private void ShowPlanets()
        {
            if (mPlanetAlpha <= 255 - AlphaModifier)
            {
                mPlanetAlpha += AlphaModifier;
                return;
            }
            mPlanetAlpha = 255;
        }
        private void HidePlanets()
        {
            if (mPlanetAlpha >= AlphaModifier)
            {
                mPlanetAlpha -= AlphaModifier;
                return;
            }
            mPlanetAlpha = 0;
        }
        private void DecreaseStarSize()
        {
            if (mStar.TextureSclae >  1)
            {
                mStar.TextureSclae -= 0.5f;
                return;
            }
            mStar.TextureSclae = 1;
        }
        private void IncreaseStarSize()
        {
            if (mStar.TextureSclae < 5)
            {
                mStar.TextureSclae += 0.5f;
                return;
            }
            mStar.TextureSclae = 5;
        }
    }
}
