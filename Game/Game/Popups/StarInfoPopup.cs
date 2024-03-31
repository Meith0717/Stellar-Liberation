// StarInfoPopup.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface.UiElements;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;

namespace StellarLiberation.Game.Popups
{
    internal class StarInfoPopup : UiFrame
    {
        public StarInfoPopup(Star star)
        {
            Width = 700;
            Height = 500;
            Anchor = Core.UserInterface.Anchor.Center;
            AddChild(new UiButton(MenueSpriteRegistries.xMark, "")
            {
                Anchor = Core.UserInterface.Anchor.NE,
                HSpace = 20,
                VSpace = 20,
                OnClickAction = () => IsDisposed = true
            });
        }
    }
}
