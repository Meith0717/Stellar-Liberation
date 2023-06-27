using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using System;
using Newtonsoft.Json;

namespace Galaxy_Explovive.Core.GameObject
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
        /// Gets or sets the offset of the texture applied to the game object.
        /// </summary>
        [JsonProperty]
        public Vector2 TextureOffset { get; set; }

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
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the game object.
        /// </summary>
        [JsonProperty]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the depth of the texture.
        /// </summary>
        [JsonProperty]
        public int TextureDepth { get; set; }

        /// <summary>
        /// Gets or sets the color applied to the texture.
        /// </summary>
        [JsonProperty]
        public Color TextureColor { get; set; }

        /// <summary>
        /// Gets or sets the bounding box around the game object.
        /// </summary>
        [JsonProperty]
        public CircleF BoundedBox { get; set; }

        /// <summary>
        /// Represents a flag indicating whether the position of the game object should be updated.
        /// </summary>
        [JsonProperty]
        protected bool UpdatePosition { get; set; } = true;

        /// <summary>
        /// Gets or sets a flag indicating whether the position of the game object is initialized.
        /// </summary>
        [JsonProperty]
        protected bool InitializePosition { get; set; } = true;
        [JsonProperty]
        private bool mPositionWasInitialize = false;

        /// <summary>
        /// Saves the position of the game object on first call.
        /// Updates the game object's logic.
        /// UpdateObjectCount is incrememted and the BoundBox is updatet.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        /// <param name="inputState">The input state of the game.</param>
        /// <param name="engine">The game engine instance.</param>
        public virtual void UpdateLogic(GameTime gameTime, InputState inputState, GameEngine engine)
        {
            if (InitializePosition && !mPositionWasInitialize)
            {
                engine.SpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
                mPositionWasInitialize = true;
            }
            engine.DebugSystem.UpdateObjectCount += 1;
            BoundedBox = new CircleF(Position, (Math.Max(Height, Width) / 2) * TextureScale);
        }

        /// <summary>
        /// Adds the game object to the spatial hashing system of the game engine if UpdatePosition is set true.
        /// </summary>
        /// <param name="engine">The game engine instance.</param>
        public void AddToSpatialHashing(GameEngine engine)
        {
            if (!UpdatePosition) return;
            engine.SpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
        }

        /// <summary>
        /// Removes the game object from the spatial hashing system of the game engine if UpdatePosition is set true.
        /// </summary>
        /// <param name="engine">The game engine instance.</param>
        public void RemoveFromSpatialHashing(GameEngine engine)
        {
            if (!UpdatePosition) return;
            engine.SpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
        }

        /// <summary>
        /// DrawnObjectCount is incrememted and the BoundBox is drawed
        /// </summary>
        /// <param name="textureManager">The texture manager for managing textures.</param>
        /// <param name="engine">The game engine instance.</param>
        public virtual void Draw(TextureManager textureManager, GameEngine engine)
        {
            engine.DebugSystem.DrawnObjectCount += 1;
            engine.DebugSystem.DrawBoundBox(textureManager, BoundedBox);
        }
    }
}
