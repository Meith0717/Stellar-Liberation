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

            AddChild(new UiText(FontRegistries.text, "Flagship MKI", 0.1f) { Anchor = Anchor.NW, HSpace = 30, VSpace = 30 });

            AddChild(GetFlagshipInfoFrame(flagship));
        }

        private UiFrame GetFlagshipInfoFrame(Flagship flagship)
        {
            var infoFrame = new UiFrame() { Alpha = 0, Width = 1200, Height = 650, Anchor = Anchor.Bottom, VSpace = 30 };

            var grid = new UiGrid(new List<double>() { 0.17, 0.08, 0.17, 0.08, 0.17, 0.08, 0.17, 0.08, }, Enumerable.Repeat(1d / 21, 21).ToList())
            { FillScale = FillScale.FillIn };
            infoFrame.AddChild(grid);

            grid.Set(0, 0, new UiText(FontRegistries.text, "Engines", .1f) { Anchor = Anchor.Left });
            grid.Set(0, 1, new UiText(FontRegistries.text, "Sublight Velocity: ", .1f) { Anchor = Anchor.Right });
            grid.Set(1, 1, new UiText(FontRegistries.text, flagship.ImpulseDrive.MaxVelocity.ToString(), .1f) { Anchor = Anchor.Left });
            grid.Set(0, 2, new UiText(FontRegistries.text, "Hyper Velocity: ", .1f) { Anchor = Anchor.Right });
            grid.Set(1, 2, new UiText(FontRegistries.text, flagship.HyperDrive.MaxVelocity.ToString(), .1f) { Anchor = Anchor.Left });

            grid.Set(0, 4, new UiText(FontRegistries.text, "Defense", .1f) { Anchor = Anchor.Left });
            grid.Set(0, 5, new UiText(FontRegistries.text, "Max Hullforce: ", .1f) { Anchor = Anchor.Right });
            grid.Set(1, 5, new UiText(FontRegistries.text, flagship.Defense.MaxHullForce.ToString(), .1f) { Anchor = Anchor.Left });
            grid.Set(0, 6, new UiText(FontRegistries.text, "Hullregeneration: ", .1f) { Anchor = Anchor.Right });
            grid.Set(1, 6, new UiText(FontRegistries.text, flagship.Defense.HullRegenerationPerSecond.ToString(), .1f) { Anchor = Anchor.Left });
            grid.Set(0, 8, new UiText(FontRegistries.text, "Max Shieldforce: ", .1f) { Anchor = Anchor.Right });
            grid.Set(1, 8, new UiText(FontRegistries.text, flagship.Defense.MaxShieldForce.ToString(), .1f) { Anchor = Anchor.Left });
            grid.Set(0, 9, new UiText(FontRegistries.text, "Shieldregeneration: ", .1f) { Anchor = Anchor.Right });
            grid.Set(1, 9, new UiText(FontRegistries.text, flagship.Defense.ShieldRegenerationPerSecond.ToString(), .1f) { Anchor = Anchor.Left });

            grid.Set(0, 11, new UiText(FontRegistries.text, "Hangar", .1f) { Anchor = Anchor.Left });
            grid.Set(0, 12, new UiText(FontRegistries.text, "Free Capacity: ", .1f) { Anchor = Anchor.Right });
            grid.Set(1, 12, new UiText(FontRegistries.text, $"{flagship.Hangar.Capacity-flagship.Hangar.Count}/{flagship.Hangar.Capacity}", .1f) { Anchor = Anchor.Left });
            grid.Set(0, 14, new UiText(FontRegistries.text, $"{BattleshipID.InterceptorMKI} MKI: ", .1f) { Anchor = Anchor.Right });
            grid.Set(1, 14, new UiText(FontRegistries.text, flagship.Hangar.GetAmount(BattleshipID.InterceptorMKI).ToString(), .1f) { Anchor = Anchor.Left });
            grid.Set(0, 15, new UiText(FontRegistries.text, $"{BattleshipID.FighterMKI} MKI: ", .1f) { Anchor = Anchor.Right });
            grid.Set(1, 15, new UiText(FontRegistries.text, flagship.Hangar.GetAmount(BattleshipID.FighterMKI).ToString(), .1f) { Anchor = Anchor.Left });
            grid.Set(0, 16, new UiText(FontRegistries.text, $"{BattleshipID.BomberMKI} MKI: ", .1f) { Anchor = Anchor.Right });
            grid.Set(1, 16, new UiText(FontRegistries.text, flagship.Hangar.GetAmount(BattleshipID.BomberMKI).ToString(), .1f) { Anchor = Anchor.Left });
            grid.Set(0, 18, new UiText(FontRegistries.text, $"{BattleshipID.InterceptorMKII} MKII: ", .1f) { Anchor = Anchor.Right });
            grid.Set(1, 18, new UiText(FontRegistries.text, flagship.Hangar.GetAmount(BattleshipID.InterceptorMKII).ToString(), .1f) { Anchor = Anchor.Left });
            grid.Set(0, 19, new UiText(FontRegistries.text, $"{BattleshipID.FighterMKII} MKII: ", .1f) { Anchor = Anchor.Right });
            grid.Set(1, 19, new UiText(FontRegistries.text, flagship.Hangar.GetAmount(BattleshipID.FighterMKII).ToString(), .1f) { Anchor = Anchor.Left });
            grid.Set(0, 20, new UiText(FontRegistries.text, $"{BattleshipID.BomberMKII} MKII: ", .1f) { Anchor = Anchor.Right });
            grid.Set(1, 20, new UiText(FontRegistries.text, flagship.Hangar.GetAmount(BattleshipID.BomberMKII).ToString(), .1f) { Anchor = Anchor.Left });

            grid.Set(2, 0, new UiText(FontRegistries.text, "Weapons", .1f) { Anchor = Anchor.Left });
            for (int i = 0; i < flagship.Weapons.Weapons.Count; i++)
            {
                var y = i * 2 + 1;
                grid.Set(2, y, new UiText(FontRegistries.text, $"Weapon {i + 1}.", .1f) { Anchor = Anchor.Left });
                grid.Set(2, y + 1, new UiText(FontRegistries.text, "Hull Damage: ", .1f) { Anchor = Anchor.Right });
                grid.Set(4, y + 1, new UiText(FontRegistries.text, "Shield Damage: ", .1f) { Anchor = Anchor.Right });
                grid.Set(6, y + 1, new UiText(FontRegistries.text, "Range: ", .1f) { Anchor = Anchor.Right });

                var weapon = flagship.Weapons.Weapons[i];
                grid.Set(3, y + 1, new UiText(FontRegistries.text, weapon.HullDamage.ToString(), .1f) { Anchor = Anchor.Left });
                grid.Set(5, y + 1, new UiText(FontRegistries.text, weapon.ShieldDamage.ToString(), .1f) { Anchor = Anchor.Left });
                grid.Set(7, y + 1, new UiText(FontRegistries.text, weapon.Range.ToString(), .1f) { Anchor = Anchor.Left });
            }
            for (int i = flagship.Weapons.Weapons.Count; i < 10; i++)
            {
                var y = i * 2 + 1;
                grid.Set(2, y, new UiText(FontRegistries.text, $"Weapon {i + 1}.", .1f) { Anchor = Anchor.Left });
                grid.Set(2, y + 1, new UiText(FontRegistries.text, "Hull Damage: ", .1f) { Anchor = Anchor.Right });
                grid.Set(4, y + 1, new UiText(FontRegistries.text, "Shield Damage: ", .1f) { Anchor = Anchor.Right });
                grid.Set(6, y + 1, new UiText(FontRegistries.text, "Range: ", .1f) { Anchor = Anchor.Right });

                grid.Set(3, y + 1, new UiText(FontRegistries.text, "-", .1f) { Anchor = Anchor.Left });
                grid.Set(5, y + 1, new UiText(FontRegistries.text, "-", .1f) { Anchor = Anchor.Left });
                grid.Set(7, y + 1, new UiText(FontRegistries.text, "-", .1f) { Anchor = Anchor.Left });
            }

            return infoFrame;
        }
    }
}
