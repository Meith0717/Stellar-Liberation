﻿// MovingObject.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Layers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.Core.GameObjectManagement
{
    [Serializable]
    public abstract class MovingObject : GameObject
    {
        [JsonProperty] public float Velocity { get; set; }
        [JsonProperty] public Vector2 Direction { get; set; }
        [JsonProperty] public Vector2 FuturePosition { get; private set; }

        public MovingObject(Vector2 position, string textureId, float textureScale, int textureDepth)
            : base(position, textureId, textureScale, textureDepth) { }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, Scene scene)
        {
            RemoveFromSpatialHashing(scene);
            Position += Direction * (float)(Velocity * gameTime.ElapsedGameTime.TotalMilliseconds);
            FuturePosition = Position + Direction * (float)(Velocity * gameTime.ElapsedGameTime.TotalMilliseconds);
            AddToSpatialHashing(scene);

            base.Update(gameTime, inputState, gameLayer, scene);
        }

        public abstract void HasCollide();
    }
}
