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
        public override void Update(GameTime gameTime, InputState inputState, GameEngine gameEngine)
        {
            if (SelectZoom == 0f) throw new WarningException("No Value Given to SelectZoom");

            IsHover = BoundedBox.Contains(gameEngine.WorldMousePosition);
            IsPressed = IsHover && inputState.mMouseActionType == MouseActionType.LeftClickReleased;
            IsSelected = gameEngine.SelectObject == this;

            ManageObjectSelection(gameEngine);
            ManageObjectTracking(gameEngine);


            base.Update(gameTime, inputState, gameEngine);

            if (IsSelected) { SelectActions(inputState, gameEngine); }
        }

        private void ManageObjectSelection(GameEngine gameEngine)
        {
            if (!IsPressed) return;
            IsPressed = false;

            if (gameEngine.SelectObject == this)
            {
                gameEngine.SelectObject = null;

                IsSelected = false;
                IsTracked = false;

                return;
            }

            if (gameEngine.SelectObject != null) return;
            gameEngine.SelectObject = this;
            gameEngine.Camera.MoveToZoom(SelectZoom);
            gameEngine.Camera.MoveToTarget(Position);

            IsTracked = true;
            IsSelected = true;
        }
        private void ManageObjectTracking(GameEngine gameEngine)
        {
            if (!IsSelected) return;
            if (gameEngine.Camera.MovedByUser)
            {
                IsTracked = false;
            }
            if (IsTracked)
            {
                gameEngine.Camera.SetPosition(Position);
            }
        }

        /// <summary>
        /// Performs actions related to the selection of the interactive object.
        /// Iss caaled in Update Method of Interactive Objects.
        /// </summary>
        /// <param name="inputState">The input state of the game.</param>
        /// <param name="gameEngine">The game engine instance.</param>
        internal abstract void SelectActions(InputState inputState, GameEngine gameEngine);
    }
}
