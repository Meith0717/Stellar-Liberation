// InteractiveObject.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

/*
 *  InteractiveObject.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Layers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace StellarLiberation.Game.Core.GameObjectManagement
{
    /// <summary>
    /// Abstract class representing an interactive game object derived from the GameObject class.
    /// </summary>
    [Serializable]
    public abstract class InteractiveObject : GameObject
    {
        internal InteractiveObject(Vector2 position, string textureId, float textureScale, int textureDepth)
            : base(position, textureId, textureScale, textureDepth) { }

        [JsonIgnore] public bool IsHover { get; private set; }
        [JsonIgnore] public bool RightPressed { get; private set; }
        [JsonIgnore] public bool LeftPressed { get; private set; }
        [JsonIgnore] protected Action LeftPressAction;
        [JsonIgnore] protected Action RightPressAction;

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            base.Update(gameTime, inputState, scene);

            IsHover = BoundedBox.Contains(scene.WorldMousePosition);
            LeftPressed = IsHover && inputState.HasMouseAction(MouseActionType.LeftClick);
            RightPressed = IsHover && inputState.HasMouseAction(MouseActionType.RightClick);

            if (LeftPressed && LeftPressAction is not null) LeftPressAction();
            if (RightPressed && RightPressAction is not null) LeftPressAction();
        }

    }
}
