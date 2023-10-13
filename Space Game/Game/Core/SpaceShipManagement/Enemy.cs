// Enemy.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.AI;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement
{
    [Serializable]
    public abstract class Enemy : SpaceShip
    {
        [JsonIgnore] protected BehaviorBasedAI mAi;

        protected Enemy(Vector2 position, string textureId, float textureScale)
            : base(position, textureId, textureScale)
        { }

        public override void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Update(gameTime, inputState, sceneManagerLayer, scene);

            if (IsDestroyed) return;
            mAi.Update(gameTime, SensorArray, this);
        }
    }
}
