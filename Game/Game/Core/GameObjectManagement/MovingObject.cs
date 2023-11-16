// MovingObject.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using System;

namespace StellarLiberation.Game.Core.GameObjectManagement
{
    [Serializable]
    public abstract class MovingObject : GameObject2D
    {
        [JsonProperty] public float Velocity;
        [JsonProperty] public Vector2 Direction;
        [JsonProperty] public Vector2 FuturePosition { get; private set; }

        public MovingObject(Vector2 position, string textureId, float textureScale, int textureDepth)
            : base(position, textureId, textureScale, textureDepth) { }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            RemoveFromSpatialHashing(scene);
            Position += Direction * (float)(Velocity * gameTime.ElapsedGameTime.TotalMilliseconds);
            FuturePosition = Position + Direction * (float)(Velocity * gameTime.ElapsedGameTime.TotalMilliseconds);
            AddToSpatialHashing(scene);

            base.Update(gameTime, inputState, scene);
        }

        public abstract void HasCollide();
    }
}
