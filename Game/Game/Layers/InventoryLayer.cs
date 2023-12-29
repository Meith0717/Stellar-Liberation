// InventoryLayer.cs
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiInventory;

namespace StellarLiberation.Game.Layers
{
    public class InventoryLayer : Layer
    {
        private readonly UiFrame mFrame;
        private readonly UiInventoryGrid mUiInventoryGrid;
        private readonly Inventory mInventory;

        public InventoryLayer(Inventory inventory) : base(true)
        {
            mFrame = new() { Height = 600, Width = 750, Anchor = Anchor.Center, Color = new(17, 17, 17) };
            mFrame.AddChild(mUiInventoryGrid = new UiInventoryGrid(5, 5) { Anchor = Anchor.Center, Width = 500, Height = 500 });
            mInventory = inventory;
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
            mUiInventoryGrid.UpdateItems(mInventory.Items);
            inputState.DoAction(ActionType.Inventar, LayerManager.PopLayer);
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, LayerManager.ResolutionManager.UiScaling);
        }
    }
}
