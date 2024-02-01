// HudLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;
using StellarLiberation.Game.Layers.MenueLayers;
using System.Collections.Generic;

namespace StellarLiberation.Game.Layers.GameLayers
{
    public class HudLayer : Layer
    {
        public bool Hide;

        private readonly UiFrame mUiLayer;
        private readonly GameLayer mScene;

        private readonly UiHBar mShieldBar;
        private readonly UiHBar mHullBar;
        private readonly UiVBar mPropulsiondBar;

        private readonly Compass mCompass = new();
        private readonly List<UiElement> mPopups = new();

        public HudLayer(GameLayer gameLayer) : base(true)
        {
            mUiLayer = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };
            mShieldBar = new(new Color(135, 206, 235), MenueSpriteRegistries.shield) { RelHeight = .025f, RelWidth = .15f, RelX = .01f, RelY = .02f };
            mHullBar = new(new Color(210, 105, 30), MenueSpriteRegistries.ship) { RelHeight = .025f, RelWidth = .15f, RelX = .01f, RelY = .05f };
            mPropulsiondBar = new(new Color(241, 196, 15), MenueSpriteRegistries.propulsion) { RelHeight = .25f, RelWidth = .02f, Anchor = Anchor.SW, HSpace = 20, VSpace = 20 };

            mUiLayer.AddChild(new UiButton(MenueSpriteRegistries.pause, "") { Anchor = Anchor.NE, HSpace = 20, VSpace = 20, OnClickAction = () => LayerManager.AddLayer(new PauseLayer(gameLayer.GameState)) });
            mUiLayer.AddChild(mShieldBar);
            mUiLayer.AddChild(mHullBar);
            mUiLayer.AddChild(mPropulsiondBar);

            mScene = gameLayer;
        }

        public void AddPopup(UiElement uiElement) => mPopups.Add(uiElement);

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Hide) return;
            spriteBatch.Begin();
            mUiLayer.Draw();
            foreach (var uiElement in mPopups)
                uiElement.Draw();
            // mCompass.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged() { }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mUiLayer.Update(inputState, GraphicsDevice.Viewport.Bounds, ResolutionManager.UiScaling);
            mShieldBar.Percentage = mScene.GameState.Player.DefenseSystem.ShieldPercentage;
            mHullBar.Percentage = mScene.GameState.Player.DefenseSystem.HullPercentage;
            mPropulsiondBar.Percentage = (double)(mScene.GameState.Player.Velocity / mScene.GameState.Player.SublightDrive.MaxVelocity);
            mCompass.Update(mScene.GameState.Player.Position, GraphicsDevice, mScene.GameState.Player.SensorSystem.LongRangeScan);
            foreach (var uiElement in mPopups)
                uiElement.Update(inputState, GraphicsDevice.Viewport.Bounds, ResolutionManager.UiScaling);
            mPopups.Clear();
        }
    }
}
