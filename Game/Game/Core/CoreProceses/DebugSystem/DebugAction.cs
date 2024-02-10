// DebugAction.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using System;

namespace StellarLiberation.Game.Core.CoreProceses.DebugSystem
{
    public class DebugAction
    {
        private readonly string mDescription;
        private readonly ActionType mActionType;
        private readonly Action mUpdateAction;

        public DebugAction(string description, ActionType actionType, Action updateAction)
        {
            mDescription = $"{actionType} => {description}";
            mActionType = actionType;
            mUpdateAction = updateAction;
        }

        public void Update(InputState inputState)  => inputState.DoAction(mActionType, mUpdateAction);

        public void DrawInfo(Vector2 position, bool isActive) => TextureManager.Instance.DrawString(FontRegistries.debugFont, position, mDescription, .75f, isActive ? Color.LightGreen : Color.White);
    }
}
