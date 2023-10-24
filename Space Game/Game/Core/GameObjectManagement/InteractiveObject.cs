// InteractiveObject.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

/*
 *  InteractiveObject.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Layers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.Core.GameObjectManagement
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
        /// Gets a value indicating whether the interactive object is currently being pressed by the Right mouse cursor.
        /// </summary>
        [JsonIgnore]
        public bool RightPressed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the interactive object is currently being pressed by the Left mouse cursor.
        /// </summary>
        [JsonIgnore]
        public bool LeftPressed { get; private set; }

        [JsonIgnore]
        protected Action LeftPressAction { get; set; }

        [JsonIgnore]
        protected Action RightPressAction { get; set; }

        /// <summary>
        /// Updates the logic of the interactive object, including hover, press and track states.
        /// Object is select when pressed. Object is deselect when pressed while select.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        /// <param name="inputState">The input state of the game.</param>
        /// <param name="gameLayer">The game engine instance.</param>
        public override void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, Scene scene)
        {
            base.Update(gameTime, inputState, gameLayer, scene);

            IsHover = BoundedBox.Contains(scene.WorldMousePosition);
            LeftPressed = IsHover && inputState.HasMouseAction(MouseActionType.LeftClickReleased);
            RightPressed = IsHover && inputState.HasMouseAction(MouseActionType.RightClick);

            if (LeftPressed && LeftPressAction is not null) LeftPressAction();
            if (RightPressed && RightPressAction is not null) LeftPressAction();
        }

    }
}
