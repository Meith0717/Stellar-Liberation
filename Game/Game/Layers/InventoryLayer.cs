// InventoryLayer.cs
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using System;

namespace StellarLiberation.Game.Layers
{
    public class InventoryLayer : Layer
    {
        private readonly UiFrame mFrame;

        private class StackLine : UiLayer
        {
            public StackLine(InventoryItem inventoryItem) : base()
            {
                Height = 80;
                Width = 80;
                Color = new(12, 12, 12);
                AddChild(new UiSprite(inventoryItem.Texture) { FillScale = FillScale.FillIn });
                AddChild(new UiText(FontRegistries.textFont, inventoryItem.Count.ToString()) { Anchor = Anchor.SE });
            }

            public StackLine() : base()
            {
                Height = 80;
                Width = 80;
                Color = new(12, 12, 12);
            }
        }

        public InventoryLayer(Inventory inventory) : base(true)
        {
            mFrame = new(50) { Height = 500, Width = 500, Anchor = Anchor.Center, Color = new(17, 17, 17) };
            var itemLayer = new UiLayer() { Height = 440, Width = 440, Anchor = Anchor.Center, Alpha = 0};
            mFrame.AddChild(itemLayer);

            var i = 0;
            for (int y = 0;  y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    var relX = .205f * x;
                    var relY = .205f * y;

                    try { itemLayer.AddChild(new StackLine(inventory.Items[i]) { RelX = relX, RelY = relY }); } 
                    catch(ArgumentOutOfRangeException) { itemLayer.AddChild(new StackLine() { RelX = relX, RelY = relY }); }
                    i++;
                }
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
            inputState.DoAction(ActionType.Inventar, LayerManager.PopLayer);
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, LayerManager.ResolutionManager.UiScaling);
        }
    }
}
