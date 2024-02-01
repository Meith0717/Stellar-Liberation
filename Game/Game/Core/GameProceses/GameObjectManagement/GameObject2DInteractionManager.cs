﻿// GameObject2DInteractionManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using System;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    public static class GameObject2DInteractionManager
    {
        public static void Manage(InputState inputState, GameObject2D gameObject2D, GameLayer scene, Action LeftPressAction, Action RightPressAction, Action hoverAction)
        {
            var isHover = gameObject2D.BoundedBox.Contains(scene.WorldMousePosition);

            if (!isHover) return;

            hoverAction?.Invoke();
            if (inputState.HasAction(ActionType.LeftClick))
                LeftPressAction?.Invoke();
            if (inputState.HasAction(ActionType.RightClick))
                RightPressAction?.Invoke();
        }
    }
}
