// HudLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.Persistance;
using StellarLiberation.Game.Core.UserInterface;

namespace StellarLiberation.Game.Layers
{
    public class HudLayer : Layer
    {
        private Scene mScene;
        private UiLayer mUiLayer;

        private UiHBar mShieldBar;
        private UiHBar mHullBar;
        private UiVBar mPropulsiondBar;

        public HudLayer(Scene scene) : base(true) 
        {
            mUiLayer = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };
            mShieldBar = new(new Color(135, 206, 235), TextureRegistries.shield) { RelHeight = .025f, RelWidth = .15f, RelX = .01f, RelY = .02f };
            mHullBar = new(new Color(210, 105, 30), TextureRegistries.ship) { RelHeight = .025f, RelWidth = .15f, RelX = .01f, RelY = .05f };
            mPropulsiondBar = new(new Color(241, 196, 15), TextureRegistries.propulsion) { RelHeight = .25f, RelWidth = .02f, Anchor = Anchor.SW, HSpace = 20, VSpace = 20 };

            mUiLayer.AddChild(new UiButton(TextureRegistries.pauseButton, "") { Anchor = Anchor.NE, HSpace = 20, VSpace = 20, OnClickAction = () => mLayerManager.AddLayer(new PauseLayer()) });
            mUiLayer.AddChild(mShieldBar);
            mUiLayer.AddChild(mHullBar);
            mUiLayer.AddChild(mPropulsiondBar);

            mScene = scene;
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            mUiLayer.Initialize(graphicsDevice.Viewport.Bounds);
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mUiLayer.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mUiLayer.OnResolutionChanged(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mUiLayer.Update(inputState, mGraphicsDevice.Viewport.Bounds);
            mShieldBar.Percentage = mScene.GameLayer.Player.DefenseSystem.ShildLevel;
            mHullBar.Percentage = mScene.GameLayer.Player.DefenseSystem.HullLevel;
            mPropulsiondBar.Percentage =  (double)(mScene.GameLayer.Player.Velocity / mScene.GameLayer.Player.SublightEngine.MaxVelocity);
        }
    }
}
