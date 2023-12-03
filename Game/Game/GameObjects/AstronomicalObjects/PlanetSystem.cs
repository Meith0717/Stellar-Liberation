// PlanetSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceShipManagement;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    public enum Danger { None, Moderate, Medium, High }

    [Serializable]
    public class PlanetSystem
    {
        [JsonProperty] public readonly Danger Danger;
        [JsonProperty] public readonly CircleF SystemBounding;
        [JsonIgnore] private MapPlanetSystem mMapObj;

        [JsonProperty] public readonly Star Star;
        [JsonProperty] public readonly List<Planet> Planets;
        [JsonProperty] public readonly SpaceShipManager SpaceShipManager = new();
        [JsonProperty] public readonly GameObjectManager AsteroidManager = new();

        public PlanetSystem(Star star, List<Planet> planets, Danger danger, float radius) 
        {
            Planets = planets;
            Star = star;
            Danger = danger;
            SystemBounding = new(Star.Position, radius);
            for (int i = 0; i < 10; i++) SpaceShipManager.Spawn(ExtendetRandom.NextVectorInCircle(SystemBounding), EnemyId.EnemyBattleShip);
            for (int i = 0; i < 10; i++) SpaceShipManager.Spawn(ExtendetRandom.NextVectorInCircle(SystemBounding), EnemyId.EnemyBomber);
            for (int i = 0; i < 3; i++) SpaceShipManager.Spawn(ExtendetRandom.NextVectorInCircle(SystemBounding), EnemyId.EnemyCarrior);
            for (int i = 0; i < 50; i++) AsteroidManager.AddObj(new Asteroid(ExtendetRandom.NextVectorInCircle(SystemBounding)));
        }

        public void UpdateObjects(GameTime gameTime, InputState inputState, Scene scene)
        {
            foreach (var item in Planets) item.Update(gameTime, inputState, scene);
            Star.Update(gameTime, inputState, scene);
            SpaceShipManager.Update(gameTime, inputState, scene);
            AsteroidManager.Update(gameTime, inputState, scene);
        }

        public MapPlanetSystem MapObj { get { return mMapObj ??= new(Vector2.Zero, this, Star.TextureId); } }
    }
}
