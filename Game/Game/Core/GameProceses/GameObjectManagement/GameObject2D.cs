// GameObject2D.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using System;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    [Serializable]
    public abstract class GameObject2D
    {
        // Phisics Stuff
        [JsonProperty] public CircleF BoundedBox;
        [JsonProperty] public Vector2 Position;
        [JsonProperty] public float Rotation;
        [JsonProperty] public Vector2 MovingDirection;
        [JsonProperty] public float Velocity;

        // Texture Stuff
        [JsonProperty] public Color TextureColor;
        [JsonProperty] public readonly string TextureId;
        [JsonProperty] public readonly int TextureDepth;
        [JsonProperty] public readonly Vector2 TextureOffset;
        [JsonProperty] public readonly float TextureScale;
        [JsonProperty] private readonly float mMaxTextureDimension;

        // Managing Stuff
        [JsonProperty] public double DisposeTime;
        [JsonIgnore] public bool IsDisposed;

        internal GameObject2D(Vector2 position, string textureId, float textureScale, int textureDepth)
        {
            Position = position;
            TextureId = textureId;
            TextureScale = textureScale;
            TextureDepth = textureDepth;
            TextureColor = Color.White;

            var texture = TextureManager.Instance.GetTexture(TextureId);
            mMaxTextureDimension = MathF.Max(texture.Width, texture.Height);
            TextureOffset = Vector2.Divide(new(texture.Width, texture.Height), 2);
            DisposeTime = double.PositiveInfinity;
            UpdateScale(TextureScale);
        }

        public void UpdateScale(float scale)
        {
            BoundedBox.Position = Position;
            BoundedBox.Radius = mMaxTextureDimension / 2 * scale;
        }

        public virtual void Update(GameTime gameTime, InputState inputState, GameLayer scene)
        {
            DisposeTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
            IsDisposed = double.IsNegative(DisposeTime - 1) | IsDisposed;
            scene.DebugSystem.UpdateObjectCount += 1;
        }

        public virtual void Draw(GameLayer scene)
        {
            scene.DebugSystem.DrawnObjectCount += 1;
            scene.DebugSystem.DrawHitbox(BoundedBox, scene);
        }

        public virtual void HasCollide(Vector2 position, GameLayer scene) { }
    }
}
