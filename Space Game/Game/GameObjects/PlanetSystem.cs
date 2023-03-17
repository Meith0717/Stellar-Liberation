using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Space_Game.Core;
using Space_Game.Core.InputManagement;
using Space_Game.Game.GameObjects.Astronomical_Body;
using System;

namespace Space_Game.Game.GameObjects
{
    [Serializable]
    public class PlanetSystem
    {
        const int RadiusLimit = 2500;
        const int AlphaModifier = 25;

        [JsonProperty] public Vector2 mPosition;
        [JsonProperty] public StarState mState;
        [JsonProperty] public Star mStar;
        [JsonProperty] public Planet mPlanet;

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
            mPosition = position;
            mStar = new Star(position);
            mPlanet = new Planet(2000, position);
        }
        public void Update(GameTime gameTime, InputState inputState)
        {
            mStar.Update(gameTime, inputState);
            Hide(); Show();
            if (!mUpdatePlanets) { return; }
            mPlanet.Update(gameTime, inputState);
        }
        public void Draw()
        {
            mStar.Draw();
            mPlanet.Draw(mPlanetAlpha);
        }
        private void Show()
        {
            if (Vector2.Distance(Globals.mCamera2d.mPosition, mPosition) > RadiusLimit) { return; }
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
            if (Vector2.Distance(Globals.mCamera2d.mPosition, mPosition) < RadiusLimit) { return; }
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
