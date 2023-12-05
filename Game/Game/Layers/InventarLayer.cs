// InventarLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;

namespace StellarLiberation.Game.Layers
{
    internal class InventarLayer : Layer
    {
        private readonly UiFrame mFrame;

        public InventarLayer(Inventory inventory) : base(false)
        {
            mFrame = new(25) { RelHeight = .8f, RelWidth = .6f, Anchor = Anchor.Center, Color = new(33, 47, 60) };
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager ,graphicsDevice,serialize);
            mFrame.Initialize(graphicsDevice.Viewport.Bounds);
        }

        public override void Destroy()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mFrame.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mFrame.OnResolutionChanged(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.Inventar, mLayerManager.PopLayer);
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds);
        }
    }
}
