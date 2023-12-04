﻿
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

        public PlanetSystem(Star star, List<Planet> planets, Danger danger, float radius) 
        {
            Danger = danger;
            SystemBounding = new(star.Position, radius);
            MapObj = new(Vector2.Zero, this, star.TextureId);

            GameObjects.AddObj(star);
            GameObjects.AddRange(planets);
            for (int i = 0; i < 1; i++) GameObjects.AddObj(EnemyFactory.Get(EnemyId.EnemyBattleShip, ExtendetRandom.NextVectorInCircle(SystemBounding)));
            for (int i = 0; i < 1; i++) GameObjects.AddObj(EnemyFactory.Get(EnemyId.EnemyBomber, ExtendetRandom.NextVectorInCircle(SystemBounding)));
            for (int i = 0; i < 1; i++) GameObjects.AddObj(EnemyFactory.Get(EnemyId.EnemyCarrior, ExtendetRandom.NextVectorInCircle(SystemBounding)));
            for (int i = 0; i < 50; i++) GameObjects.AddObj(new Asteroid(ExtendetRandom.NextVectorInCircle(SystemBounding)));
        }

        public void UpdateObjects(GameTime gameTime, InputState inputState, Scene scene) => GameObjects.Update(gameTime, inputState, scene);
    }
}
