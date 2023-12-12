// HudLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiBar;

namespace StellarLiberation.Game.Layers
{
    public class HudLayer : Layer
    {
        public bool Hide;

        private Scene mScene;
        private UiLayer mUiLayer;

        private UiHBar mShieldBar;
        private UiHBar mHullBar;
        private UiVBar mPropulsiondBar;

        private UiText mCargoHold;
        private Compass mCompass = new();

        public HudLayer(Scene scene) : base(true)
        {
            mUiLayer = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };
            mShieldBar = new(new Color(135, 206, 235), TextureRegistries.shield) { RelHeight = .025f, RelWidth = .15f, RelX = .01f, RelY = .02f };
            mHullBar = new(new Color(210, 105, 30), TextureRegistries.ship) { RelHeight = .025f, RelWidth = .15f, RelX = .01f, RelY = .05f };
            mPropulsiondBar = new(new Color(241, 196, 15), TextureRegistries.propulsion) { RelHeight = .25f, RelWidth = .02f, Anchor = Anchor.SW, HSpace = 20, VSpace = 20 };
            mCargoHold = new(FontRegistries.buttonFont, "test") { Anchor = Anchor.N, VSpace = 20 };

            mUiLayer.AddChild(new UiButton(TextureRegistries.pauseButton, "") { Anchor = Anchor.NE, HSpace = 20, VSpace = 20, OnClickAction = () => mLayerManager.AddLayer(new PauseLayer()) });
            mUiLayer.AddChild(mShieldBar);
            mUiLayer.AddChild(mHullBar);
            mUiLayer.AddChild(mPropulsiondBar);
            mUiLayer.AddChild(mCargoHold);
            mUiLayer.AddChild(mCargoHold);

            mScene = scene;
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Hide) return;
            spriteBatch.Begin();
            mUiLayer.Draw();
            mCompass.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged() { }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mUiLayer.Update(inputState, mGraphicsDevice.Viewport.Bounds, mLayerManager.ResolutionManager.UiScaling);
            mShieldBar.Percentage = mScene.GameLayer.Player.DefenseSystem.ShieldPercentage;
            mHullBar.Percentage = mScene.GameLayer.Player.DefenseSystem.HullPercentage;
            mPropulsiondBar.Percentage = (double)(mScene.GameLayer.Player.Velocity / mScene.GameLayer.Player.SublightEngine.MaxVelocity);
            mCargoHold.Text = $"{mScene.GameLayer.Inventory.Count}/{mScene.GameLayer.Inventory.Capacity}";
            mCompass.Update(mScene.GameLayer.Player.Position, mScene.ViewFrustumFilter, mScene.GameLayer.Player.SensorArray.LongRangeScan);
        }
    }
}
