// GameObject.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

/*
*  GameObject.cs
*
*  Copyright (c) 2023 Thierry Meiers
*  All rights reserved.
*/

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Layers;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.Core.GameObjectManagement
{
    /// <summary>
    /// Abstract class representing a game object in the game engine.
    /// </summary>
    [Serializable]
    public abstract class GameObject
    {
        /// <summary>
        /// Gets or sets the position of the game object in 2D space.
        /// </summary>
        [JsonProperty]
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the rotation angle of the game object.
        /// </summary>
        [JsonProperty]
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the texture associated with the game object.
        /// </summary>
        [JsonProperty]
        public string TextureId { get; private set; }

        /// <summary>
        /// Gets or sets the scaling factor applied to the texture.
        /// </summary>
        [JsonProperty]
        public float TextureScale { get; private set; }

        /// <summary>
        /// Gets or sets the width of the game object.
        /// </summary>
        [JsonProperty]
        public int Width { get; private set; }

        /// <summary>
        /// Gets or sets the height of the game object.
        /// </summary>
        [JsonProperty]
        public int Height { get; private set; }

        /// <summary>
        /// Gets or sets the offset of the texture applied to the game object.
        /// </summary>
        [JsonProperty]
        public Vector2 TextureOffset { get; private set; }

        /// <summary>
        /// Gets or sets the depth of the texture.
        /// </summary>
        [JsonProperty]
        public int TextureDepth { get; set; }

        /// <summary>
        /// Gets or sets the color applied to the texture.
        /// </summary>
        [JsonProperty]
        public Color TextureColor { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the bounding box around the game object.
        /// </summary>
        [JsonProperty]
        public CircleF BoundedBox { get; set; }

        [JsonIgnore]
        private double LiveTime32 { get; set; }

        [JsonIgnore]
        public double DisposeTime { get; set; }

        [JsonIgnore]
        public bool Dispose { get; protected set; }


        [JsonIgnore]
        private bool mWasRemovedFromSpatialHashing = true;

        internal GameObject(Vector2 position, string textureId, float textureScale, int textureDepth, double disposeTime = double.PositiveInfinity)
        {
            DisposeTime = disposeTime;
            Position = position;
            TextureId = textureId;
            TextureScale = textureScale;
            TextureDepth = textureDepth;
            Width = TextureManager.Instance.GetTexture(textureId).Width;
            Height = TextureManager.Instance.GetTexture(textureId).Height;
            TextureOffset = new(Width / 2, Height / 2);
            UpdateBoundBox();
        }

        internal void UpdateBoundBox()
        {
            BoundedBox = new CircleF(Position, MathF.Max(Height, Width) / 2 * TextureScale);
        }

        /// <summary>
        /// Updates the game object's logic.
        /// UpdateObjectCount is incrememted and the BoundBox is updatet.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        /// <param name="inputState">The input state of the game.</param>
        /// <param name="sceneLayer">The game engine instance.</param>
        public virtual void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, Scene scene)
        {
            LiveTime32 += gameTime.ElapsedGameTime.TotalMilliseconds;
            Dispose = (LiveTime32 > DisposeTime) || Dispose;
            if (LiveTime32 > (4294967296)) LiveTime32 = 0;
            gameLayer.DebugSystem.UpdateObjectCount += 1;
            UpdateBoundBox();
        }

        /// <summary>
        /// Adds the game object to the spatial hashing system of the game engine if UpdatePosition is set true.
        /// </summary>
        /// <param name="sceneLayer">The game engine instance.</param>
        internal void AddToSpatialHashing(Scene scene)
        {
            if (!mWasRemovedFromSpatialHashing) return;
            scene.SpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
            mWasRemovedFromSpatialHashing = false;
        }

        /// <summary>
        /// Removes the game object from the spatial hashing system of the game engine if UpdatePosition is set true.
        /// </summary>
        /// <param name="scene">The game engine instance.</param>
        internal void RemoveFromSpatialHashing(Scene scene)
        {
            mWasRemovedFromSpatialHashing = true;
            scene.SpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
        }

        /// <summary>
        /// DrawnObjectCount is incrememted and the BoundBox is drawed
        /// </summary>
        /// <param name="sceneLayer">The game engine instance.</param>
        public virtual void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            sceneManagerLayer.DebugSystem.DrawnObjectCount += 1;
            sceneManagerLayer.DebugSystem.DrawHitbox(BoundedBox, scene);
        }
    }
}
