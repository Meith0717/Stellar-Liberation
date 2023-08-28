/*
*  GameObject.cs
*
*  Copyright (c) 2023 Thierry Meiers
*  All rights reserved.
*/

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.Core.GameObjects
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
        public string TextureId { get; set; }

        /// <summary>
        /// Gets or sets the scaling factor applied to the texture.
        /// </summary>
        [JsonProperty]
        public float TextureScale { get; set; }

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

        /// <summary>
        /// Gets or sets whether the game object should only be updated when it's on-screen.
        /// </summary>
        [JsonProperty]
        public bool UpdateOnlyOnScreen { get; set; } = false;

        [JsonIgnore]
        private bool mWasRemovedFromSpatialHashing = true;

        [JsonIgnore]
        protected GameLayer GameLayer;


        internal GameObject(Vector2 position, string textureId, float textureScale, int textureDepth)
        {
            Position = position;
            TextureId = textureId;
            TextureScale = textureScale;
            TextureDepth = textureDepth;
            Width = TextureManager.Instance.GetTexture(textureId).Width;
            Height = TextureManager.Instance.GetTexture(textureId).Height;
            TextureOffset = new(Width / 2, Height / 2);
            UpdateBoundBox();
        }

        public void Initialize(GameLayer gameLayer) { GameLayer = gameLayer; }

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
        /// <param name="engine">The game engine instance.</param>
        public virtual void Update(GameTime gameTime, InputState inputState)
        {
            GameLayer.DebugSystem.UpdateObjectCount += 1;
            UpdateBoundBox();
        }

        /// <summary>
        /// Adds the game object to the spatial hashing system of the game engine if UpdatePosition is set true.
        /// </summary>
        /// <param name="engine">The game engine instance.</param>
        internal void AddToSpatialHashing()
        {
            if (!mWasRemovedFromSpatialHashing) return;
            GameLayer.SpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
            mWasRemovedFromSpatialHashing = false;
        }

        /// <summary>
        /// Removes the game object from the spatial hashing system of the game engine if UpdatePosition is set true.
        /// </summary>
        /// <param name="engine">The game engine instance.</param>
        internal void RemoveFromSpatialHashing()
        {
            mWasRemovedFromSpatialHashing = true;
            GameLayer.SpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
        }

        /// <summary>
        /// DrawnObjectCount is incrememted and the BoundBox is drawed
        /// </summary>
        /// <param name="engine">The game engine instance.</param>
        public virtual void Draw()
        {
            GameLayer.DebugSystem.DrawnObjectCount += 1;
            GameLayer.DebugSystem.DrawBoundBox(BoundedBox, GameLayer);
        }
    }
}
