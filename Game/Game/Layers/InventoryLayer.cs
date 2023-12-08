// InventoryLayer.cs 
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
    internal class InventoryLayer : Layer
    {
        private readonly UiFrame mFrame;

        public InventoryLayer(Inventory inventory) : base(false)
        {
            mFrame = new(50) { RelHeight = .8f, RelWidth = .6f, Anchor = Anchor.Center, Color = new(17, 17, 17) };

            var i = 0;
            foreach (var itemStack in inventory.Items.Values)
            {
                mFrame.AddChild(new UiText(FontRegistries.textFont, $"{itemStack.ItemName}: {itemStack.Amount}")
                {
                    Anchor = Anchor.CenterV,
                    RelY = .1f * i
                });
                i++;
            }
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mFrame.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged() { }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.Inventar, mLayerManager.PopLayer);
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, mLayerManager.mResolutionManager.ActualResolution.uiScaling);
        }
    }
}
