// FlagshipPopup.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Popups
{
    internal class FlagshipPopup : UiFrame
    {
        public FlagshipPopup(Flagship flagship) 
        {
            Width = 1300;
            Height = 800;
            Anchor = Anchor.Center;

            AddChild(new UiButton(MenueSpriteRegistries.xMark, "")
            {
                Anchor = Anchor.NE,
                HSpace = 20,
                VSpace = 20,
                OnClickAction = () => IsDisposed = true
            });

            var grid = new UiGrid(new List<double>() { 0.18, 0.07, 0.18, 0.07, 0.18, 0.07, 0.18, 0.07, }, Enumerable.Repeat(1d / 15, 15).ToList()) 
            { Width = 1250, Height = 550, Anchor = Anchor.Bottom, VSpace = 20};
            AddChild(grid);

            grid.Set(0, 0, new UiText(FontRegistries.subTitleFont, "Engines") { Anchor = Anchor.Left });
            grid.Set(0, 1, new UiText(FontRegistries.textFont, "Sublight Velocity:") { Anchor = Anchor.Right });
            grid.Set(1, 1, new UiText(FontRegistries.textFont, flagship.ImpulseDrive.MaxVelocity.ToString()) { Anchor = Anchor.Center });
            grid.Set(2, 1, new UiText(FontRegistries.textFont, "Hyper Velocity:") { Anchor = Anchor.Right });
            grid.Set(3, 1, new UiText(FontRegistries.textFont, flagship.HyperDrive.MaxVelocity.ToString()) { Anchor = Anchor.Center });

            grid.Set(0, 3, new UiText(FontRegistries.subTitleFont, "Defense") { Anchor = Anchor.Left });
            grid.Set(0, 4, new UiText(FontRegistries.textFont, "Max Hullforce:") { Anchor = Anchor.Right });
            grid.Set(1, 4, new UiText(FontRegistries.textFont, flagship.Defense.MaxHullForce.ToString()) { Anchor = Anchor.Center });
            grid.Set(2, 4, new UiText(FontRegistries.textFont, "Hullregeneration:") { Anchor = Anchor.Right });
            grid.Set(3, 4, new UiText(FontRegistries.textFont, flagship.Defense.HullRegenerationPerSecond.ToString()) { Anchor = Anchor.Center });
            grid.Set(0, 5, new UiText(FontRegistries.textFont, "Max Shieldforce:") { Anchor = Anchor.Right });
            grid.Set(1, 5, new UiText(FontRegistries.textFont, flagship.Defense.MaxShieldForce.ToString()) { Anchor = Anchor.Center });
            grid.Set(2, 5, new UiText(FontRegistries.textFont, "Shieldregeneration:") { Anchor = Anchor.Right });
            grid.Set(3, 5, new UiText(FontRegistries.textFont, flagship.Defense.ShieldRegenerationPerSecond.ToString()) { Anchor = Anchor.Center });

            grid.Set(0, 7, new UiText(FontRegistries.subTitleFont, "Hangar") { Anchor = Anchor.Left });
            grid.Set(0, 8, new UiText(FontRegistries.textFont, "Capacity:") { Anchor = Anchor.Right });
            grid.Set(1, 8, new UiText(FontRegistries.textFont, flagship.Hangar.Capacity.ToString()) { Anchor = Anchor.Center });

            grid.Set(2, 7, new UiText(FontRegistries.subTitleFont, "Cargo") { Anchor = Anchor.Left });
            grid.Set(2, 8, new UiText(FontRegistries.textFont, "Capacity:") { Anchor = Anchor.Right });
            grid.Set(3, 8, new UiText(FontRegistries.textFont, "N.A.") { Anchor = Anchor.Center });

        }
    }
}
