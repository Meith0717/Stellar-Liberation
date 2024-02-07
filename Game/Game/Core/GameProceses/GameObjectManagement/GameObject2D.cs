// GameObject2D.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using System;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    [Serializable]
    public abstract class GameObject2D
    {
        [JsonIgnore] protected GameLayer GameLayer { get; private set; }

        // Phisics Stuff
        public CircleF BoundedBox => new(Position, MathF.Max(mTextureHeight, mTextureWidth) / 2 * TextureScale);
        [JsonProperty] public Vector2 Position;
        [JsonProperty] public float Rotation;
        [JsonProperty] public Vector2 MovingDirection;
        [JsonProperty] public float Velocity;

        // Texture Stuff
        [JsonProperty] public Color TextureColor;
        [JsonProperty] public float TextureScale;
        [JsonProperty] public readonly string TextureId;
        [JsonProperty] public readonly int TextureDepth;
        [JsonProperty] public readonly Vector2 TextureOffset;
        [JsonProperty] private readonly int mTextureWidth;
        [JsonProperty] private readonly int mTextureHeight;

        // Managing Stuff
        [JsonIgnore] public bool Dispose;
        [JsonIgnore] public double DisposeTime;
        [JsonIgnore] public readonly bool UpdatePosition;

        internal GameObject2D(Vector2 position, string textureId, float textureScale, int textureDepth, bool updatePosition = true)
        {
            Position = position;
            TextureId = textureId;
            TextureScale = textureScale;
            TextureDepth = textureDepth;
            TextureColor = Color.White;

            mTextureWidth = TextureManager.Instance.GetTexture(textureId).Width;
            mTextureHeight = TextureManager.Instance.GetTexture(textureId).Height;
            TextureOffset = Vector2.Divide(new(mTextureWidth, mTextureHeight), 2);
            DisposeTime = double.PositiveInfinity;
            UpdatePosition = updatePosition;
        }

        public virtual void Initialize(GameLayer gameLayer, bool addToSpatialHash = true)
        {
            GameLayer = gameLayer;
            if (!addToSpatialHash) return;
            GameLayer.SpatialHashing?.InsertObject(this, (int)Position.X, (int)Position.Y);
        }

        public virtual void Update(GameTime gameTime, InputState inputState, GameLayer scene)
        {
            DisposeTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
            Dispose = double.IsNegative(DisposeTime - 1) | Dispose;
            scene.GameState.DebugSystem.UpdateObjectCount += 1;
        }

        public virtual void Draw(GameLayer scene)
        {
            scene.GameState.DebugSystem.DrawnObjectCount += 1;
            scene.GameState.DebugSystem.DrawHitbox(BoundedBox, scene);
        }

        public virtual void HasCollide(Vector2 position, GameLayer scene) { }
    }
}
