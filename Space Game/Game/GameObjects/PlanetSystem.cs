using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using System;
using System.Collections.Generic;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core;

namespace Galaxy_Explovive.Game.GameObjects
{
    [Serializable]
    public class PlanetSystem : GameObject
    {
        const int AlphaModifier = 15;

        [JsonProperty] public Star mStar;
        [JsonProperty] public List<Planet> mPlanets;
        [JsonProperty] private readonly float mRadiusLimit;
        [JsonProperty] private bool mIsSystemShown;
        [JsonProperty] private int mPlanetAlpha = 255;

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
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState, GameEngine engine)
        {
            // Checkst if System is on Screen
            if (!engine.FrustumCuller.CircleOnWorldView(BoundedBox)) { return; }

            // Show or hide Systems
            ShowSystem(engine); HideSystem(engine);
            mStar.UpdateLogik(gameTime, inputState, engine);

            // Update based on Cam. Positions 
            if (!mIsSystemShown) { return; }
            foreach (Planet planet in mPlanets)
            {
                planet.UpdateLogik(gameTime, inputState, engine);
            }
        }

        public override void Draw(TextureManager textureManager, GameEngine engine)
        {
            
            // Draw Stuff
            mStar.Draw(textureManager, engine);
            engine.DebugSystem.DrawBoundBox(textureManager, BoundedBox);

            // Draw based on Cam. Positions 
            if (!mIsSystemShown) { return; }
            foreach (Planet planet in mPlanets)
            {
                planet.Draw(mPlanetAlpha, textureManager, engine);
            }
        }

        private void ShowSystem(GameEngine engine)
        {
            if (engine.Camera.Zoom < 0.1) { return; }
            mIsSystemShown = true;
            if (mPlanetAlpha <= 255 - AlphaModifier)
            {
                mPlanetAlpha += AlphaModifier;
                return;
            }
            mPlanetAlpha = 255;
        }
        private void HideSystem(GameEngine engine)
        {
            if (engine.Camera.Zoom > 0.1) { return; }
            if (!mIsSystemShown) { return; }
            if (mPlanetAlpha >= AlphaModifier)
            {
                mPlanetAlpha -= AlphaModifier;
                return;
            }
            mPlanetAlpha = 0;
            foreach (Planet p in mPlanets) 
            { 
                if (engine.SelectObject != p) continue;
                engine.SelectObject = null;
            }
            mIsSystemShown = false;
        }
    }
}
