// InventoryLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;

namespace StellarLiberation.Game.Layers.GameLayers
{
    public class InventoryLayer : Layer
    {
        private readonly UiFrame mFrame;

        public InventoryLayer(Inventory inventory, Wallet wallet, Game1 game1) : base(game1, true)
        {
            mFrame = new() { Height = 800, Width = 550, Anchor = Anchor.Center };
            mFrame.AddChild(new UiText(FontRegistries.subTitleFont, "Inventory") { Anchor = Anchor.NW, HSpace = 50, VSpace = 50 });
            mFrame.AddChild(new UiInventory(inventory, null) { Width = 500, Height = 600, Anchor = Anchor.Center });
            var walletGrid = new UiGrid(2, 1) { Width = 200, Height = 50, Anchor = Anchor.SE };
            mFrame.AddChild(walletGrid);
            walletGrid.Set(0, 0, new UiText(FontRegistries.textFont, "Credits:") { Anchor = Anchor.Center });
            walletGrid.Set(1, 0, new UiText(FontRegistries.textFont, wallet.Balance.ToString()) { Anchor = Anchor.Center });
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mFrame.Draw();
            spriteBatch.End();
        }

        public override void ApplyResolution()
        {
            mFrame.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, LayerManager.PopLayer);
            inputState.DoAction(ActionType.Inventar, LayerManager.PopLayer);
            mFrame.Update(inputState, gameTime);
        }
    }
}
