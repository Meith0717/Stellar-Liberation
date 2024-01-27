// UiTooltip.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;

namespace StellarLiberation.Game.Core.UserInterface.UiElements
{
    public class UiTooltip : UiText
    {
        public UiTooltip(string text, Vector2 position) : base(FontRegistries.textFont, text)
        {
            Canvas.X = (int)position.X + 10;
            Canvas.Y = (int)position.Y + 10;
        }
    }
}
