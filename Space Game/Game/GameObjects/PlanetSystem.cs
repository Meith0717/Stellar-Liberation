using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
using Newtonsoft.Json;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        private bool mUpdatePlanets;
        private int mPlanetAlpha;

        public enum StarState
        {
            Uncovered,
            Discovered,
            Explored
        }
        public PlanetSystem(Vector2 position)
        {
            // Location
            Position = position;
            mStar = new Star(position);
            mPlanets = new List<Planet>();
            for (int i = 0; i < Globals.mRandom.Next(2, 5); i++)
            {
                mRadiusLimit = 1200 + 600 * i;
                mPlanets.Add(new Planet(mRadiusLimit, position));
            } 
            BoundedBox = new CircleF(position, mRadiusLimit+400);
        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            mStar.Update(gameTime, inputState);
            Hide(); Show();
            if (!mUpdatePlanets) { return; }

            foreach (Planet planet in mPlanets)
            {
                planet.Update(gameTime, inputState);
            }
        }
        public override void Draw()
        {
            mStar.Draw();
            Globals.mDebugSystem.DrawBoundBox(BoundedBox);
            if (!mUpdatePlanets) { return; }
            foreach (Planet planet in mPlanets)
            {
                planet.Draw(mPlanetAlpha);
            }
        }
        private void Show()
        {
            if (!BoundedBox.Contains(Globals.mCamera2d.mPosition)) { return; }
            mUpdatePlanets = true;
            if (mPlanetAlpha <= 255 - AlphaModifier)
            {
                mPlanetAlpha += AlphaModifier;
                return;
            }
            mPlanetAlpha = 255;
        }
        private void Hide()
        {
            if (BoundedBox.Contains(Globals.mCamera2d.mPosition)) { return; }
            if (mPlanetAlpha >= AlphaModifier)
            {
                mPlanetAlpha -= AlphaModifier;
                return;
            }
            mPlanetAlpha = 0;
            mUpdatePlanets = false;
        }
    }
}
