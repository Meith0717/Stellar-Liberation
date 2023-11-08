// PauseLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.Persistance;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StellarLiberation.Game.Layers
{
    public class PauseLayer : Layer
    {
        private UiLayer mBackgroundLayer;
        public PauseLayer()
            : base(false) 
        {
            mBackgroundLayer = new() {RelHeight = 1, RelWidth = 1, Color = Color.Black, Alpha = .8f};
            var buttonlayer = new UiLayer() { RelHeight = .4f, RelWidth = .3f, Color = Color.Transparent, Anchor = Anchor.Center };
            mBackgroundLayer.AddChild(buttonlayer);

            buttonlayer.AddChild(new UiButton(ContentRegistry.buttonContinue) { FillScale = FillScale.X, Anchor = Anchor.N, OnClickAction = () => mLayerManager.PopLayer() });
            buttonlayer.AddChild(new UiButton(ContentRegistry.buttonSave) { FillScale = FillScale.X, Anchor = Anchor.Center, IsDisabled = true });
            buttonlayer.AddChild(new UiButton(ContentRegistry.buttonExitgame) { FillScale = FillScale.X, Anchor = Anchor.S, OnClickAction = () => { mLayerManager.PopLayer(); mLayerManager.PopLayer(); } });
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            mBackgroundLayer.Initialize(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Destroy()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mBackgroundLayer.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mBackgroundLayer.OnResolutionChanged(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, () => mLayerManager.PopLayer());
            mBackgroundLayer.Update(inputState, mGraphicsDevice.Viewport.Bounds);
        }
        
        private void Exit()
        {
            mLayerManager.Exit();
        }
    }
}
