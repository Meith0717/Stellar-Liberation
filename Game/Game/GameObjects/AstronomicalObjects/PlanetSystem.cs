
// PlanetSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameObjectManagement;
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
        [JsonIgnore] public readonly MapPlanetSystem MapObj;

        [JsonProperty] public readonly GameObjectManager GameObjects = new();

        public PlanetSystem(Vector2 mapPosition, Star star, List<Planet> planets, List<Asteroid> asteroids, Danger danger, float radius)
        {
            Danger = danger;
            SystemBounding = new(star.Position, radius);
            MapObj = new(mapPosition, this, star.TextureId);

            GameObjects.AddObj(star);
            GameObjects.AddRange(planets);
            GameObjects.AddRange(asteroids);
            for (int i = 0; i < 10; i++) GameObjects.AddObj(EnemyFactory.Get(EnemyId.EnemyBattleShip, ExtendetRandom.NextVectorInCircle(SystemBounding)));
            for (int i = 0; i < 0; i++) GameObjects.AddObj(EnemyFactory.Get(EnemyId.EnemyBomber, ExtendetRandom.NextVectorInCircle(SystemBounding)));
            for (int i = 0; i < 0; i++) GameObjects.AddObj(EnemyFactory.Get(EnemyId.EnemyCarrior, ExtendetRandom.NextVectorInCircle(SystemBounding)));
        }

        public void UpdateObjects(GameTime gameTime, InputState inputState, Scene scene) => GameObjects.Update(gameTime, inputState, scene);
    }
}
