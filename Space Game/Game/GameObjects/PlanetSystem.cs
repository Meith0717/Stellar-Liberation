using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using System;
using System.Collections.Generic;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core;
using MonoGame.Extended;

namespace Galaxy_Explovive.Game.GameObjects
{
    [Serializable]
    public class PlanetSystem
    {
        [JsonProperty] readonly private List<Planet> mPlanets;
        [JsonProperty] readonly private Star mStar;
        [JsonProperty] private CircleF mBoundedBox;

        public PlanetSystem(Vector2 position)
        {
            mStar = new(position);
            mPlanets = new();
            int radiusLimit = (int)(mStar.Width / 2 * mStar.TextureScale);
            int orbitNr;
            for (orbitNr = 1; orbitNr <= MyUtility.Random.Next(2, 6); orbitNr++)
            {
                mPlanets.Add(new Planet(orbitNr, position, mStar.mLightColor, radiusLimit));    
            }
            mBoundedBox = new CircleF(position, 2500);
        }

        public void Update(GameTime time, InputState inputState, GameEngine engine)
        {
            if (engine.FrustumCuller.CircleOnWorldView(mBoundedBox))
            {                
                engine.UpdateGameObject(time, inputState, mStar);
                engine.UpdateGameObjects(time, inputState, mPlanets);
            }
        }

        public void Draw(TextureManager textureManager, GameEngine engine)
        {
            engine.DrawGameObject(textureManager, mStar);
            engine.DrawGameObjects(textureManager, mPlanets);
            engine.DebugSystem.DrawBoundBox(textureManager, mBoundedBox);
        }

        // Planets need to be hide
    }
}
