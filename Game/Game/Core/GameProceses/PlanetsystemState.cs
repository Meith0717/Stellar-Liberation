// PlanetsystemState.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParticleSystem;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.Layers;
using System;
using System.Collections.Generic;


namespace StellarLiberation.Game.Core.GameProceses
{
    [Serializable]
    public class PlanetsystemState
    {
        private const double PlanetUpdateCoolDown = 200;
        [JsonIgnore] private double mPlanetUpdateCoolDown;

        [JsonProperty] public Vector2 MapPosition;
        [JsonProperty] public Fractions Occupier = Fractions.Neutral;
        [JsonProperty] private Star mStar;
        [JsonProperty] private List<Planet> mPlanets = new();
        [JsonProperty] private GameObjectList mGameObjects = new();

        [JsonIgnore] public readonly SpatialHashing SpatialHashing = new(5000);
        [JsonIgnore] public readonly Queue<StereoSound> StereoSounds = new();
        [JsonIgnore] public readonly Queue<ParticleEmitor> ParticleEmitors = new();

        public PlanetsystemState(Vector2 mapPosition, Star star, List<Planet> planets)
        {
            MapPosition = mapPosition;
            mStar = star;
            mPlanets = planets;
            mPlanetUpdateCoolDown = ExtendetRandom.Random.NextDouble() * PlanetUpdateCoolDown;
        }

        public void Initialize()
        {
            GameObjectManager.Initialize(SpatialHashing, ref mStar);
            GameObjectManager.Initialize(SpatialHashing, ref mPlanets);
            GameObjectManager.Initialize(SpatialHashing, ref mGameObjects);
        }

        public void Update(GameTime gameTime, GameState gameState)
        {
            mPlanetUpdateCoolDown -= gameTime.ElapsedGameTime.TotalMilliseconds;
            GameObjectManager.Update(gameTime, gameState, this, ref mStar);
            if (mPlanetUpdateCoolDown <= 0)
            {
                mPlanetUpdateCoolDown = PlanetUpdateCoolDown;
                GameObjectManager.Update(gameTime, gameState, this, ref mPlanets);
            }
            GameObjectManager.Update(gameTime, gameState, this, ref mGameObjects);
        }

        public void AddGameObject(GameObject gameObject)
        {
            SpatialHashing.InsertObject(gameObject, (int)gameObject.Position.X, (int)gameObject.Position.Y);
            mGameObjects.Add(gameObject);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            SpatialHashing.RemoveObject(gameObject, (int)gameObject.Position.X, (int)gameObject.Position.Y);
            mGameObjects.Remove(gameObject);
        }

        [JsonIgnore]
        public GameObjectList GameObjects => mGameObjects;

        [JsonIgnore]
        public List<Planet> Planets => mPlanets;

        [JsonIgnore]
        public Star Star => mStar;
    }
}
