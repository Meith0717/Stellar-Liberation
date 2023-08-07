/*
 *  InteractiveObject.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using CelestialOdyssey.GameEngine.InputManagement;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.GameEngine.GameObjects
{
    /// <summary>
    /// Abstract class representing an interactive game object derived from the GameObject class.
    /// </summary>
    [Serializable]
    public abstract class InteractiveObject : GameObject
    {
        internal InteractiveObject(Vector2 position, string textureId, float textureScale, int textureDepth) 
            : base(position, textureId, textureScale, textureDepth) { }

        /// <summary>
        /// Gets a value indicating whether the interactive object is currently being hovered over by the mouse cursor.
        /// </summary>
        [JsonIgnore]
        public bool IsHover { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the interactive object is currently being pressed by the mouse cursor.
        /// </summary>
        [JsonIgnore]
        public bool IsPressed { get; private set; }

        [JsonIgnore]
        public Action OnPressAction { get; set; }

        /// <summary>
        /// Updates the logic of the interactive object, including hover, press and track states.
        /// Object is select when pressed. Object is deselect when pressed while select.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        /// <param name="inputState">The input state of the game.</param>
        /// <param name="gameEngine">The game engine instance.</param>
        public override void Update(GameTime gameTime, InputState inputState, GameEngine gameEngine)
        {
            base.Update(gameTime, inputState, gameEngine);

            IsHover = BoundedBox.Contains(gameEngine.WorldMousePosition);
            IsPressed = IsHover && inputState.mMouseActionType == MouseActionType.LeftClickReleased;

            if (IsPressed && OnPressAction is not null) OnPressAction();
        }
    }
}
