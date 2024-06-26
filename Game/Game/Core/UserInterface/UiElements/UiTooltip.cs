// UiTooltip.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;

namespace StellarLiberation.Game.Core.UserInterface.UiElements
{
    public class UiTooltip : UiText
    {
        public UiTooltip(string text, Vector2 position) : base("neuropolitical", text, .1f)
        {
            Canvas.X = (int)position.X + 10;
            Canvas.Y = (int)position.Y + 10;
        }
    }
}
