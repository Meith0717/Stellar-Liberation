// PlanetSystemState.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Visuals.ParticleSystem;
using StellarLiberation.Game.Layers;
using System;
using System.Collections.Generic;


namespace StellarLiberation.Game.Core.GameProceses
{
    [Serializable]
    public class PlanetsystemState
    {
        [JsonProperty] public Fractions Occupier = Fractions.Neutral;
        [JsonProperty] public Vector2 MapPosition;
        [JsonProperty] private GameObject2DList mGameObjects = new();

        [JsonIgnore] public readonly SpatialHashing SpatialHashing = new(5000);
        [JsonIgnore] public readonly Queue<StereoSound> StereoSounds = new();
        [JsonIgnore] public readonly Queue<ParticleEmitor> ParticleEmitors = new();

        public PlanetsystemState(Vector2 mapPosition, List<GameObject2D> gameObjects)
        {
            MapPosition = mapPosition;
            mGameObjects.AddRange(gameObjects);
        }

        public void Initialize() => GameObject2DManager.Initialize(SpatialHashing,ref mGameObjects);

        public void Update(GameTime gameTime, InputState inputState, GameState gameState) => GameObject2DManager.Update(gameTime, inputState, gameState, this, ref mGameObjects);

        public void AddGameObject(GameObject2D gameObject)
        {
            SpatialHashing.InsertObject(gameObject, (int)gameObject.Position.X, (int)gameObject.Position.Y);
            mGameObjects.Add(gameObject);
        }

        public GameObject2DList GameObjects => mGameObjects;
    }
}
