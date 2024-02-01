// GameObject2D.cs 
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
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using System;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    [Serializable]
    public abstract class GameObject2D
    {
        // Phisics Stuff
        public CircleF BoundedBox => new(Position, MathF.Max(mTextureHeight, mTextureWidth) / 2 * TextureScale);
        [JsonProperty] public Vector2 Position { get; set; }
        [JsonProperty] public float Rotation { get; set; }
        [JsonProperty] public Vector2 MovingDirection { get; set; }
        [JsonProperty] public float Velocity { get; set; }

        // Texture Stuff
        [JsonProperty] public Color TextureColor;
        [JsonProperty] public float TextureScale;
        [JsonProperty] public readonly string TextureId;
        [JsonProperty] public readonly int TextureDepth;
        [JsonProperty] public readonly Vector2 TextureOffset;
        [JsonProperty] private readonly int mTextureWidth;
        [JsonProperty] private readonly int mTextureHeight;

        // Managing Stuff
        [JsonIgnore] public bool Dispose { get; set; }
        [JsonIgnore] public double DisposeTime;
        [JsonIgnore] private double LiveTime32;

        internal GameObject2D(Vector2 position, string textureId, float textureScale, int textureDepth)
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
        }

        public virtual void Update(GameTime gameTime, InputState inputState, GameLayer scene)
        {
            LiveTime32 += gameTime.ElapsedGameTime.TotalMilliseconds;
            Dispose = LiveTime32 > DisposeTime || Dispose;
            if (LiveTime32 > 4294967296) LiveTime32 = 0;
            scene.GameState.DebugSystem.UpdateObjectCount += 1;
        }

        internal void AddToSpatialHashing(GameLayer scene) => scene.SpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);

        internal void RemoveFromSpatialHashing(GameLayer scene) => scene.SpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);

        public virtual void Draw(GameLayer scene)
        {
            scene.GameState.DebugSystem.DrawnObjectCount += 1;
            scene.GameState.DebugSystem.DrawHitbox(BoundedBox, scene);
        }

        public virtual void HasCollide(Vector2 position, GameLayer scene) { }
    }
}
