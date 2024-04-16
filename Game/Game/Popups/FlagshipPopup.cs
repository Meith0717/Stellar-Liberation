// FlagshipPopup.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;
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

            AddChild(new UiText(FontRegistries.subTitleFont, "Flagship MKI") { Anchor = Anchor.NW, HSpace = 30, VSpace = 30 });

            AddChild(GetFlagshipInfoFrame(flagship));
        }

        private UiFrame GetFlagshipInfoFrame(Flagship flagship)
        {
            var infoFrame = new UiFrame() { Alpha = 0, Width = 1200, Height = 650, Anchor = Anchor.Bottom, VSpace = 30 };

            var grid = new UiGrid(new List<double>() { 0.17, 0.08, 0.17, 0.08, 0.17, 0.08, 0.17, 0.08, }, Enumerable.Repeat(1d / 21, 21).ToList())
            { FillScale = FillScale.FillIn };
            infoFrame.AddChild(grid);

            grid.Set(0, 0, new UiText(FontRegistries.subTitleFont, "Engines") { Anchor = Anchor.Left });
            grid.Set(0, 1, new UiText(FontRegistries.textFont, "Sublight Velocity: ") { Anchor = Anchor.Right });
            grid.Set(1, 1, new UiText(FontRegistries.textFont, flagship.ImpulseDrive.MaxVelocity.ToString()) { Anchor = Anchor.Left });
            grid.Set(0, 2, new UiText(FontRegistries.textFont, "Hyper Velocity: ") { Anchor = Anchor.Right });
            grid.Set(1, 2, new UiText(FontRegistries.textFont, flagship.HyperDrive.MaxVelocity.ToString()) { Anchor = Anchor.Left });

            grid.Set(0, 4, new UiText(FontRegistries.subTitleFont, "Defense") { Anchor = Anchor.Left });
            grid.Set(0, 5, new UiText(FontRegistries.textFont, "Max Hullforce: ") { Anchor = Anchor.Right });
            grid.Set(1, 5, new UiText(FontRegistries.textFont, flagship.Defense.MaxHullForce.ToString()) { Anchor = Anchor.Left });
            grid.Set(0, 6, new UiText(FontRegistries.textFont, "Hullregeneration: ") { Anchor = Anchor.Right });
            grid.Set(1, 6, new UiText(FontRegistries.textFont, flagship.Defense.HullRegenerationPerSecond.ToString()) { Anchor = Anchor.Left });
            grid.Set(0, 8, new UiText(FontRegistries.textFont, "Max Shieldforce: ") { Anchor = Anchor.Right });
            grid.Set(1, 8, new UiText(FontRegistries.textFont, flagship.Defense.MaxShieldForce.ToString()) { Anchor = Anchor.Left });
            grid.Set(0, 9, new UiText(FontRegistries.textFont, "Shieldregeneration: ") { Anchor = Anchor.Right });
            grid.Set(1, 9, new UiText(FontRegistries.textFont, flagship.Defense.ShieldRegenerationPerSecond.ToString()) { Anchor = Anchor.Left });

            grid.Set(0, 11, new UiText(FontRegistries.subTitleFont, "Hangar") { Anchor = Anchor.Left });
            grid.Set(0, 12, new UiText(FontRegistries.textFont, "Free Capacity: ") { Anchor = Anchor.Right });
            grid.Set(1, 12, new UiText(FontRegistries.textFont, $"{flagship.Hangar.Capacity-flagship.Hangar.Count}/{flagship.Hangar.Capacity}") { Anchor = Anchor.Left });
            grid.Set(0, 14, new UiText(FontRegistries.textFont, $"{BattleshipID.InterceptorMKI} MKI: ") { Anchor = Anchor.Right });
            grid.Set(1, 14, new UiText(FontRegistries.textFont, flagship.Hangar.GetAmount(BattleshipID.InterceptorMKI).ToString()) { Anchor = Anchor.Left });
            grid.Set(0, 15, new UiText(FontRegistries.textFont, $"{BattleshipID.FighterMKI} MKI: ") { Anchor = Anchor.Right });
            grid.Set(1, 15, new UiText(FontRegistries.textFont, flagship.Hangar.GetAmount(BattleshipID.FighterMKI).ToString()) { Anchor = Anchor.Left });
            grid.Set(0, 16, new UiText(FontRegistries.textFont, $"{BattleshipID.BomberMKI} MKI: ") { Anchor = Anchor.Right });
            grid.Set(1, 16, new UiText(FontRegistries.textFont, flagship.Hangar.GetAmount(BattleshipID.BomberMKI).ToString()) { Anchor = Anchor.Left });
            grid.Set(0, 18, new UiText(FontRegistries.textFont, $"{BattleshipID.InterceptorMKII} MKII: ") { Anchor = Anchor.Right });
            grid.Set(1, 18, new UiText(FontRegistries.textFont, flagship.Hangar.GetAmount(BattleshipID.InterceptorMKII).ToString()) { Anchor = Anchor.Left });
            grid.Set(0, 19, new UiText(FontRegistries.textFont, $"{BattleshipID.FighterMKII} MKII: ") { Anchor = Anchor.Right });
            grid.Set(1, 19, new UiText(FontRegistries.textFont, flagship.Hangar.GetAmount(BattleshipID.FighterMKII).ToString()) { Anchor = Anchor.Left });
            grid.Set(0, 20, new UiText(FontRegistries.textFont, $"{BattleshipID.BomberMKII} MKII: ") { Anchor = Anchor.Right });
            grid.Set(1, 20, new UiText(FontRegistries.textFont, flagship.Hangar.GetAmount(BattleshipID.BomberMKII).ToString()) { Anchor = Anchor.Left });

            grid.Set(2, 0, new UiText(FontRegistries.subTitleFont, "Weapons") { Anchor = Anchor.Left });
            for (int i = 0; i < flagship.Weapons.Weapons.Count; i++)
            {
                var y = i * 2 + 1;
                grid.Set(2, y, new UiText(FontRegistries.textFont, $"Weapon {i + 1}.") { Anchor = Anchor.Left });
                grid.Set(2, y + 1, new UiText(FontRegistries.textFont, "Hull Damage: ") { Anchor = Anchor.Right });
                grid.Set(4, y + 1, new UiText(FontRegistries.textFont, "Shield Damage: ") { Anchor = Anchor.Right });
                grid.Set(6, y + 1, new UiText(FontRegistries.textFont, "Range: ") { Anchor = Anchor.Right });

                var weapon = flagship.Weapons.Weapons[i];
                grid.Set(3, y + 1, new UiText(FontRegistries.textFont, weapon.HullDamage.ToString()) { Anchor = Anchor.Left });
                grid.Set(5, y + 1, new UiText(FontRegistries.textFont, weapon.ShieldDamage.ToString()) { Anchor = Anchor.Left });
                grid.Set(7, y + 1, new UiText(FontRegistries.textFont, weapon.Range.ToString()) { Anchor = Anchor.Left });
            }
            for (int i = flagship.Weapons.Weapons.Count; i < 10; i++)
            {
                var y = i * 2 + 1;
                grid.Set(2, y, new UiText(FontRegistries.textFont, $"Weapon {i + 1}.") { Anchor = Anchor.Left });
                grid.Set(2, y + 1, new UiText(FontRegistries.textFont, "Hull Damage: ") { Anchor = Anchor.Right });
                grid.Set(4, y + 1, new UiText(FontRegistries.textFont, "Shield Damage: ") { Anchor = Anchor.Right });
                grid.Set(6, y + 1, new UiText(FontRegistries.textFont, "Range: ") { Anchor = Anchor.Right });

                grid.Set(3, y + 1, new UiText(FontRegistries.textFont, "-") { Anchor = Anchor.Left });
                grid.Set(5, y + 1, new UiText(FontRegistries.textFont, "-") { Anchor = Anchor.Left });
                grid.Set(7, y + 1, new UiText(FontRegistries.textFont, "-") { Anchor = Anchor.Left });
            }

            return infoFrame;
        }
    }
}
