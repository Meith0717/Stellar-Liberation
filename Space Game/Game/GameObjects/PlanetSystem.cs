using GalaxyExplovive.Core.GameEngine;
using GalaxyExplovive.Core.GameEngine.Content_Management;
using GalaxyExplovive.Core.GameEngine.InputManagement;
using GalaxyExplovive.Game.GameObjects.Astronomical_Body;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;

namespace GalaxyExplovive.Game.GameObjects
{
    [Serializable]
    public class PlanetSystem
    {
        [JsonProperty] readonly private List<Planet> mPlanets;
        [JsonProperty] readonly private Star mStar;
        [JsonProperty] public CircleF BoundedBox;
        [JsonProperty] public Species Occupant;

        public PlanetSystem(Vector2 position)
        {
            mStar = new(position);
            mPlanets = new();
            int radiusLimit = (int)(mStar.Width / 2 * mStar.TextureScale);
            int orbitNr;
            var maxOrbit = new Triangular(1, 7, 5);
            for (orbitNr = 1; orbitNr <= (int)maxOrbit.Sample(); orbitNr++)
            {
                mPlanets.Add(new Planet(orbitNr, position, mStar.mLightColor, radiusLimit));
            }
            BoundedBox = new CircleF(position, 2500);
        }

        public void Update(GameTime time, InputState inputState, GameEngine engine)
        { 
            engine.UpdateGameObject(time, inputState, mStar);
            engine.UpdateGameObjects(time, inputState, mPlanets);
        }

        public void Draw(TextureManager textureManager, GameEngine engine)
        {
            Rendering.DrawGameObject(textureManager, engine, mStar);
            Rendering.DrawGameObjects(textureManager, engine, mPlanets);
            engine.DebugSystem.DrawBoundBox(textureManager, BoundedBox, engine);
        }

        public void Hide()
        {
            foreach (Planet planet in mPlanets)
            {
                planet.DecreaseVisibility();
            }
        }

        public void Show()
        {
            foreach (Planet planet in mPlanets)
            {
                planet.IncreaseVisibility();
            }
        }

    }
}
