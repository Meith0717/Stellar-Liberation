// PlanetSystemState.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
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
        [JsonProperty] public Vector2 MapPosition;
        [JsonProperty] public Fractions Occupier = Fractions.Neutral;
        [JsonProperty] private Star mStar;
        [JsonProperty] private List<Planet> mPlanets = new();
        [JsonProperty] private GameObject2DList mGameObjects = new();

        [JsonIgnore] public readonly SpatialHashing SpatialHashing = new(5000);
        [JsonIgnore] public readonly Queue<StereoSound> StereoSounds = new();
        [JsonIgnore] public readonly Queue<ParticleEmitor> ParticleEmitors = new();

        public PlanetsystemState(Vector2 mapPosition, Star star, List<Planet> planets)
        {
            MapPosition = mapPosition;
            mStar = star;
            mPlanets = planets;
        }

        public void Initialize()
        {
            GameObject2DManager.Initialize(SpatialHashing, ref mStar);
            GameObject2DManager.Initialize(SpatialHashing, ref mPlanets);
            GameObject2DManager.Initialize(SpatialHashing, ref mGameObjects);
        }

        public void Update(GameTime gameTime, InputState inputState, GameState gameState)
        {
            GameObject2DManager.Update(gameTime, inputState, gameState, this,ref mStar);
            GameObject2DManager.Update(gameTime, inputState, gameState, this, ref mPlanets);
            GameObject2DManager.Update(gameTime, inputState, gameState, this, ref mGameObjects);
        }

        public void AddGameObject(GameObject2D gameObject)
        {
            SpatialHashing.InsertObject(gameObject, (int)gameObject.Position.X, (int)gameObject.Position.Y);
            mGameObjects.Add(gameObject);
        }

        public void RemoveGameObject(GameObject2D gameObject)
        {
            SpatialHashing.RemoveObject(gameObject, (int)gameObject.Position.X, (int)gameObject.Position.Y);
            mGameObjects.Remove(gameObject);
        }

        [JsonIgnore]
        public GameObject2DList GameObjects => mGameObjects;

        [JsonIgnore]
        public List<Planet> Planets => mPlanets;

        [JsonIgnore]
        public Star Star => mStar;
    }
}
