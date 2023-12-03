// GameObject2DInteractionManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using System;

namespace StellarLiberation.Game.Core.GameObjectManagement
{
    public static class GameObject2DInteractionManager
    {
        public static void Manage(InputState inputState, GameObject2D gameObject2D, Scene scene, Action LeftPressAction, Action RightPressAction)
        {
            var isHover = gameObject2D.BoundedBox.Contains(scene.WorldMousePosition);

            if (!isHover) return;

            var leftPressed = inputState.HasAction(ActionType.LeftClick);
            var rightPressed = inputState.HasAction(ActionType.RightClick);

            if (leftPressed && LeftPressAction is not null) LeftPressAction();
            if (rightPressed && RightPressAction is not null) LeftPressAction();
        }
    }
}
