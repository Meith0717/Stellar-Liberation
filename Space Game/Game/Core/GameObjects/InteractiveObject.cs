/*
 *  InteractiveObject.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using CelestialOdyssey.Game.Core.InputManagement;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.Core.GameObjects
{
    /// <summary>
    /// Abstract class representing an interactive game object derived from the GameObject class.
    /// </summary>
    [Serializable]
    public abstract class InteractiveObject : GameObject
    {
        internal InteractiveObject(Vector2 position, string textureId, float textureScale, int textureDepth, Color hoverColor)
            : base(position, textureId, textureScale, textureDepth) { HoverColor = hoverColor; }

        /// <summary>
        /// Gets a value indicating whether the interactive object is currently being hovered over by the mouse cursor.
        /// </summary>
        [JsonIgnore]
        public bool IsHover { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the interactive object is currently being pressed by the Left mouse cursor.
        /// </summary>
        [JsonIgnore]
        public bool IsLeftPressed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the interactive object is currently being pressed by the Right mouse cursor.
        /// </summary>
        [JsonIgnore]
        public bool IsRightPressed { get; private set; }

        [JsonIgnore]
        public Color HoverColor { get; private set; }

        /// <summary>
        /// Updates the logic of the interactive object, including hover, press and track states.
        /// Object is select when pressed. Object is deselect when pressed while select.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        /// <param name="inputState">The input state of the game.</param>
        /// <param name="gameEngine">The game engine instance.</param>
        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
            if (GameLayer is null) return;

            IsHover = BoundedBox.Contains(GameLayer.WorldMousePosition);
            IsLeftPressed = IsHover && inputState.mMouseActionType == MouseActionType.LeftClickReleased;
            IsRightPressed = IsHover && inputState.mMouseActionType == MouseActionType.LeftClickReleased;

            if (IsLeftPressed) OnLeftPressAction();
            if (IsRightPressed) OnRightPressAction();
        }

        public abstract void OnLeftPressAction();
        public abstract void OnRightPressAction();
    }
}
