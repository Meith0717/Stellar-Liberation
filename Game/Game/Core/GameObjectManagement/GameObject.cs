// GameObject.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

/*
*  GameObject.cs
*
*  Copyright (c) 2023 Thierry Meiers
*  All rights reserved.
*/

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using System;

namespace StellarLiberation.Game.Core.GameObjectManagement
{
    [Serializable]
    public abstract class GameObject
    {
        [JsonProperty] public Vector2 Position;
        [JsonProperty] public float Rotation;
        [JsonProperty] public readonly string TextureId;
        [JsonProperty] public readonly float TextureScale;
        [JsonProperty] public readonly int Width;
        [JsonProperty] public readonly int Height;
        [JsonProperty] public readonly Vector2 TextureOffset;
        [JsonProperty] public readonly int TextureDepth;
        [JsonIgnore] private bool mWasRemovedFromSpatialHashing = true;
        [JsonProperty] public Color TextureColor = Color.White;
        [JsonProperty] public CircleF BoundedBox { get; private set; }
        [JsonIgnore] public bool Dispose { get; protected set; }
        [JsonIgnore] private double LiveTime32;
        [JsonIgnore] public double DisposeTime;

        internal GameObject(Vector2 position, string textureId, float textureScale, int textureDepth)
        {
            DisposeTime = double.PositiveInfinity;
            Position = position;
            TextureId = textureId;
            TextureScale = textureScale;
            TextureDepth = textureDepth;
            Width = TextureManager.Instance.GetTexture(textureId).Width;
            Height = TextureManager.Instance.GetTexture(textureId).Height;
            TextureOffset = new(Width / 2, Height / 2);
            UpdateBoundBox();
        }

        internal void UpdateBoundBox() => BoundedBox = new CircleF(Position, MathF.Max(Height, Width) / 2 * TextureScale);

        public virtual void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            LiveTime32 += gameTime.ElapsedGameTime.TotalMilliseconds;
            Dispose = (LiveTime32 > DisposeTime) || Dispose;
            if (LiveTime32 > 4294967296) LiveTime32 = 0;
            scene.GameLayer.DebugSystem.UpdateObjectCount += 1;
            UpdateBoundBox();
        }

        internal void AddToSpatialHashing(Scene scene)
        {
            if (!mWasRemovedFromSpatialHashing) return;
            scene.SpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
            mWasRemovedFromSpatialHashing = false;
        }

        internal void RemoveFromSpatialHashing(Scene scene)
        {
            mWasRemovedFromSpatialHashing = true;
            scene.SpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
        }

        public virtual void Draw(Scene scene)
        {
            scene.GameLayer.DebugSystem.DrawnObjectCount += 1;
            scene.GameLayer.DebugSystem.DrawHitbox(BoundedBox, scene);
        }
    }
}
