
// PlanetSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Distributions;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips.Enemys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceStations;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    public enum Danger { None, Moderate, Medium, High }

    [Serializable]
    public class PlanetSystem
    {
        [JsonProperty] private readonly Vector2 mMapPosition;
        [JsonProperty] private readonly Star mStar;
        [JsonProperty] private readonly List<Planet> mPlanets = new();
        [JsonProperty] private readonly List<Asteroid> mAsteroids = new();

        [JsonProperty] public readonly Danger Danger;
        [JsonProperty] public readonly CircleF SystemBounding;
        [JsonProperty] public readonly GameObjectManager GameObjects = new();
        [JsonIgnore] public MapPlanetSystem MapObj => new(mMapPosition, this, mStar.TextureId);

        public PlanetSystem(Vector2 mapPosition, Danger danger)
        {
            mMapPosition = mapPosition;

            var triangularDistribution = new Triangular(2, 10, 6);
            mStar = StarTypes.GenerateRandomStar(Vector2.Zero);

            // Generate Planets of Star
            var planetAmount = (int)triangularDistribution.Sample();
            var radius = mStar.BoundedBox.Radius;

            for (int i = 1; i <= planetAmount; i++)
            {
                radius += ExtendetRandom.Random.Next(40000, 60000);
                mPlanets.Add(MapFactory.GetPlanet(mStar.Position, (int)radius, i));
            }

            mAsteroids = MapFactory.GetAsteroidsRing(ExtendetRandom.Random.Next(50, 200), mStar.Position, radius * 1.3f, radius * 1.3f + ExtendetRandom.Random.Next(3000, 60000));

            Danger = danger;
            SystemBounding = new(mStar.Position, radius);

            for (int i = 0; i < 10; i++) GameObjects.AddObj(EnemyFactory.Get(EnemyId.EnemyBattleShip, ExtendetRandom.NextVectorInCircle(SystemBounding)));
            for (int i = 0; i < 10; i++) GameObjects.AddObj(EnemyFactory.Get(EnemyId.EnemyBomber, ExtendetRandom.NextVectorInCircle(SystemBounding)));
            for (int i = 0; i < 5; i++) GameObjects.AddObj(EnemyFactory.Get(EnemyId.EnemyCarrior, ExtendetRandom.NextVectorInCircle(SystemBounding)));

            GameObjects.AddObj(new ScienceStation(ExtendetRandom.NextVectorInCircle(SystemBounding)));
            GameObjects.AddObj(mStar);
            GameObjects.AddRange(mAsteroids);
            GameObjects.AddRange(mPlanets);
        }

        public void UpdateObjects(GameTime gameTime, InputState inputState, Scene scene)
        {
            GameObjects.Update(gameTime, inputState, scene);
        }
    }
}
