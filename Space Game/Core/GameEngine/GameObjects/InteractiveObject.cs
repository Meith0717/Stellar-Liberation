/*
 *  InteractiveObject.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using GalaxyExplovive.Core.GameEngine.InputManagement;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace GalaxyExplovive.Core.GameEngine.GameObjects
{
    /// <summary>
    /// Abstract class representing an interactive game object derived from the GameObject class.
    /// </summary>
    [Serializable]
    public abstract class InteractiveObject : GameObject
    {
        /// <summary>
        /// Gets a value indicating whether the object is currently selected.
        /// </summary>
        public bool IsSelected { get; private set; }

        /// <summary>
        /// Gets or sets the zoom level applied when the interactive object is selected.
        /// </summary>
        [JsonIgnore]
        public float SelectZoom { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interactive object is tracked.
        /// </summary>
        [JsonIgnore]
        public bool IsTracked { get; set; }

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

        /// <summary>
        /// Updates the logic of the interactive object, including hover, press and track states.
        /// Object is select when pressed. Object is deselect when pressed while select.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        /// <param name="inputState">The input state of the game.</param>
        /// <param name="gameEngine">The game engine instance.</param>
        public override void UpdateLogic(GameTime gameTime, InputState inputState, GameEngine gameEngine)
        {
            if (SelectZoom == 0f) throw new WarningException("No Value Given to SelectZoom");

            base.UpdateLogic(gameTime, inputState, gameEngine);

            IsHover = BoundedBox.Contains(gameEngine.WorldMousePosition);
            IsPressed = IsHover && inputState.mMouseActionType == MouseActionType.LeftClickReleased;

            if (gameEngine.SelectObject == this && IsPressed)
            {
                IsSelected = IsPressed = IsTracked = false;
                gameEngine.SelectObject = null;
                return;
            }

            if (gameEngine.SelectObject == null && IsPressed)
            {
                IsPressed = false;
                IsTracked = IsSelected = true;
                gameEngine.SelectObject = this;
                gameEngine.Camera.MoveToZoom(SelectZoom);
                gameEngine.Camera.MoveToTarget(Position);
            }
        }

        /// <summary>
        /// Performs actions related to the selection of the interactive object.
        /// Performs the tracking of the object.
        /// </summary>
        /// <param name="inputState">The input state of the game.</param>
        /// <param name="gameEngine">The game engine instance.</param>
        public virtual void SelectActions(InputState inputState, GameEngine gameEngine)
        {
            if (gameEngine.Camera.MovedByUser)
            {
                IsTracked = false;
            }
            if (IsTracked)
            {
                gameEngine.Camera.SetPosition(Position);
            }
        }
    }
}
