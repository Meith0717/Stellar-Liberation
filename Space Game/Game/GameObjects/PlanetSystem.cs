using GalaxyExplovive.Core.GameEngine;
using GalaxyExplovive.Core.GameEngine.InputManagement;
using GalaxyExplovive.Core.GameEngine.Rendering;
using GalaxyExplovive.Core.GameEngine.Utility;
using GalaxyExplovive.Game.GameObjects.Astronomical_Body;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GalaxyExplovive.Game.GameObjects
{
    [Serializable]
    public class PlanetSystem
    {
        [JsonProperty] readonly private List<Planet> mPlanets;
        [JsonProperty] readonly private Star mStar;
        [JsonProperty] public CircleF BoundedBox;

        public PlanetSystem(Vector2 position)
        {
            mStar = new(position);
            mPlanets = new();
            int radiusLimit = (int)(mStar.Width / 2 * mStar.TextureScale);
            int orbitNr;
            for (orbitNr = 1; orbitNr <= MyUtility.Random.Next(1, 1); orbitNr++)
            {
                mPlanets.Add(new Planet(orbitNr, position, mStar.mLightColor, radiusLimit));
            }
            BoundedBox = new CircleF(position, 2500);
        }

        public void Update(GameTime time, InputState inputState, GameEngine engine)
        {
            foreach (Planet planet in mPlanets)
            {
                switch (BoundedBox.Contains(engine.WorldMousePosition))
                {
                    case true:
                        planet.IncreaseVisibility();
                        break;
                    case false:
                        planet.DecreaseVisibility();
                        break;
                }
            }
            engine.UpdateGameObject(time, inputState, mStar);
            engine.UpdateGameObjects(time, inputState, mPlanets);
        }

        public void Draw(TextureManager textureManager, GameEngine engine)
        {
            engine.DrawGameObject(textureManager, mStar);
            engine.DrawGameObjects(textureManager, mPlanets);
            engine.DebugSystem.DrawBoundBox(textureManager, BoundedBox, engine);
        }

    }
}
