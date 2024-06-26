﻿// FlagshipPopup.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
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
        private readonly Flagship mFlagship;
        private UiText mText1;
        private UiText mText2;
        private UiText mText3;
        private UiText mText4;
        private UiText mText5;
        private UiText mText6;

        public FlagshipPopup(Flagship flagship, Action closeAction)
        {
            Width = 420;
            Height = 900;
            Anchor = Anchor.E;
            HSpace = 20;
            mFlagship = flagship;

            AddChild(new UiText("neuropolitical", "Flagship MKI", 0.2f) { Anchor = Anchor.NW, HSpace = 20, VSpace = 20 });

            AddChild(GetFlagshipInfoFrame(flagship));
        }

        private UiFrame GetFlagshipInfoFrame(Flagship flagship)
        {
            var infoFrame = new UiFrame() { Alpha = 0, Width = 380, Height = 810, Anchor = Anchor.Bottom, VSpace = 20 };

            var grid1 = new UiGrid(new List<double>() { 0.75, 0.25 }, Enumerable.Repeat(1d / 9, 9).ToList())
            { Width = 360, Height = 225, Anchor = Anchor.Top, RelY = 0f };
            infoFrame.AddChild(grid1);

            grid1.Set(0, 0, new UiText("neuropolitical", "Engines", .15f) { Anchor = Anchor.Left });
            grid1.Set(0, 1, new UiText("neuropolitical", "Sublight Velocity:", .1f) { Anchor = Anchor.Right });
            grid1.Set(1, 1, new UiText("neuropolitical", flagship.ImpulseDrive.MaxVelocity.ToString(), .1f) { Anchor = Anchor.Center });
            grid1.Set(0, 2, new UiText("neuropolitical", "Hyper Velocity:", .1f) { Anchor = Anchor.Right });
            grid1.Set(1, 2, new UiText("neuropolitical", flagship.HyperDrive.MaxVelocity.ToString(), .1f) { Anchor = Anchor.Center });

            grid1.Set(0, 4, new UiText("neuropolitical", "Defense", .15f) { Anchor = Anchor.Left });
            grid1.Set(0, 5, new UiText("neuropolitical", "Max Hullforce:", .1f) { Anchor = Anchor.Right });
            grid1.Set(1, 5, new UiText("neuropolitical", flagship.Defense.MaxHullForce.ToString(), .1f) { Anchor = Anchor.Center });
            grid1.Set(0, 6, new UiText("neuropolitical", "Hullregeneration:", .1f) { Anchor = Anchor.Right });
            grid1.Set(1, 6, new UiText("neuropolitical", flagship.Defense.HullRegenerationPerSecond.ToString(), .1f) { Anchor = Anchor.Center });
            grid1.Set(0, 7, new UiText("neuropolitical", "Max Shieldforce:", .1f) { Anchor = Anchor.Right });
            grid1.Set(1, 7, new UiText("neuropolitical", flagship.Defense.MaxShieldForce.ToString(), .1f) { Anchor = Anchor.Center });
            grid1.Set(0, 8, new UiText("neuropolitical", "Shieldregeneration:", .1f) { Anchor = Anchor.Right });
            grid1.Set(1, 8, new UiText("neuropolitical", flagship.Defense.ShieldRegenerationPerSecond.ToString(), .1f) { Anchor = Anchor.Center });

            var grid2 = new UiGrid(new List<double>() { 0.04, 0.32, 0.32, 0.32 }, Enumerable.Repeat(1d / 12, 12).ToList())
            { Width = 360, Height = 300, Anchor = Anchor.Center };
            infoFrame.AddChild(grid2);

            grid2.Set(0, 0, new UiText("neuropolitical", "Weapons", .15f) { Anchor = Anchor.Left });
            grid2.Set(0, 1, new UiText("neuropolitical", "N.", .1f) { Anchor = Anchor.Left });
            grid2.Set(1, 1, new UiText("neuropolitical", "Hulldamage", .1f) { Anchor = Anchor.Center });
            grid2.Set(2, 1, new UiText("neuropolitical", "Shielddamage", .1f) { Anchor = Anchor.Center });
            grid2.Set(3, 1, new UiText("neuropolitical", "Range", .1f) { Anchor = Anchor.Center });

            for (int i = 0; i < flagship.Weapons.Weapons.Count; i++)
            {
                var y = i + 2;
                grid2.Set(0, y, new UiText("neuropolitical", $"{i + 1}", .1f) { Anchor = Anchor.Center });
                var weapon = flagship.Weapons.Weapons[i];
                grid2.Set(1, y, new UiText("neuropolitical", weapon.HullDamage.ToString(), .1f) { Anchor = Anchor.Center });
                grid2.Set(2, y, new UiText("neuropolitical", weapon.ShieldDamage.ToString(), .1f) { Anchor = Anchor.Center });
                grid2.Set(3, y, new UiText("neuropolitical", weapon.Range.ToString(), .1f) { Anchor = Anchor.Center });
            }
            for (int i = flagship.Weapons.Weapons.Count; i < 10; i++)
            {
                var y = i + 2;
                grid2.Set(0, y, new UiText("neuropolitical", $"{i + 1}", .1f) { Anchor = Anchor.Center });            /// 
                grid2.Set(1, y, new UiText("neuropolitical", "-", .1f) { Anchor = Anchor.Center });
                grid2.Set(2, y, new UiText("neuropolitical", "-", .1f) { Anchor = Anchor.Center });
                grid2.Set(3, y, new UiText("neuropolitical", "-", .1f) { Anchor = Anchor.Center });
            }

            var grid3 = new UiGrid(new List<double>() { .4, .2, .2, .2 }, Enumerable.Repeat(1d / 9, 9).ToList())
            { Width = 360, Height = 225, Anchor = Anchor.Bottom };
            infoFrame.AddChild(grid3);

            grid3.Set(0, 0, new UiText("neuropolitical", "Hangar", .15f) { Anchor = Anchor.Left });
            grid3.Set(0, 1, new UiText("neuropolitical", "Free Capacity:", .1f) { Anchor = Anchor.Right });
            grid3.Set(1, 1, new UiText("neuropolitical", $"{mFlagship.Hangar.Capacity - mFlagship.Hangar.Count}/{mFlagship.Hangar.Capacity}", .1f) { Anchor = Anchor.Center });
            var battleships = Enum.GetValues(typeof(BattleshipID)).Cast<BattleshipID>().ToList();
            for (int i = 0; i < battleships.Count; i++)
            {
                var battleshipID = battleships[i];
                grid3.Set(0, i + 3, new UiText("neuropolitical", $"{battleshipID}:", .1f) { Anchor = Anchor.Right });
                grid3.Set(1, i + 3, new UiButton("arrowL", "") { OnClickAction = () => mFlagship.Hangar.OrderBack(battleshipID), Anchor = Anchor.Center });
                grid3.Set(3, i + 3, new UiButton("arrowR", "") { OnClickAction = () => mFlagship.Hangar.Spawn(battleshipID), Anchor = Anchor.Center });
            }
            grid3.Set(2, 3, mText1 = new UiText("neuropolitical", "", .1f) { Anchor = Anchor.Center });
            grid3.Set(2, 4, mText2 = new UiText("neuropolitical", "", .1f) { Anchor = Anchor.Center });
            grid3.Set(2, 5, mText3 = new UiText("neuropolitical", "", .1f) { Anchor = Anchor.Center });
            grid3.Set(2, 6, mText4 = new UiText("neuropolitical", "", .1f) { Anchor = Anchor.Center });
            grid3.Set(2, 7, mText5 = new UiText("neuropolitical", "", .1f) { Anchor = Anchor.Center });
            grid3.Set(2, 8, mText6 = new UiText("neuropolitical", "", .1f) { Anchor = Anchor.Center });
            return infoFrame;
        }

        public override void Update(InputState inputState, GameTime gameTime)
        {
            mText1.Text = $"{mFlagship.Hangar.GetOnBoardAmount(BattleshipID.BomberMKI)}/{mFlagship.Hangar.GetAssignedAmount(BattleshipID.BomberMKI)}";
            mText2.Text = $"{mFlagship.Hangar.GetOnBoardAmount(BattleshipID.InterceptorMKI)}/{mFlagship.Hangar.GetAssignedAmount(BattleshipID.InterceptorMKI)}";
            mText3.Text = $"{mFlagship.Hangar.GetOnBoardAmount(BattleshipID.FighterMKI)}/{mFlagship.Hangar.GetAssignedAmount(BattleshipID.FighterMKI)}";
            mText4.Text = $"{mFlagship.Hangar.GetOnBoardAmount(BattleshipID.BomberMKII)}/{mFlagship.Hangar.GetAssignedAmount(BattleshipID.BomberMKII)}";
            mText5.Text = $"{mFlagship.Hangar.GetOnBoardAmount(BattleshipID.InterceptorMKII)}/{mFlagship.Hangar.GetAssignedAmount(BattleshipID.InterceptorMKII)}";
            mText6.Text = $"{mFlagship.Hangar.GetOnBoardAmount(BattleshipID.FighterMKII)}/{mFlagship.Hangar.GetAssignedAmount(BattleshipID.FighterMKII)}";
            base.Update(inputState, gameTime);
        }
    }
}
