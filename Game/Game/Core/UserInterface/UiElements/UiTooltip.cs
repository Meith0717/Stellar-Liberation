// UiTooltip.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;

namespace StellarLiberation.Game.Core.UserInterface.UiElements
{
    public class UiTooltip : UiText
    {
        public UiTooltip(string text, Vector2 position) : base(FontRegistries.textFont, text)
        {
            Canvas.X = position.X + 10;
            Canvas.Y = position.Y + 10;
        }
    }
}
