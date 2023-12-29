// UiInventorySlot.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.UserInterface.UiInventory
{
    public class UiInventoryGrid : UiElement
    {

        private readonly List<UiLayer> mGridElements = new();

        public UiInventoryGrid(int i, int j) 
        {
            for (int y = 0; y < i; y++)
            {
                for (int x = 0; x < j; x++)
                {
                    var relX = 1f/i * x;
                    var relY = 1f/j* y;
                    var width = 1f/i;
                    var height = 1f/j;

                    var grid = new UiLayer { Color = Color.Transparent, RelX = relX, RelY = relY, RelWidth = width, RelHeight = height };
                    mGridElements.Add(grid);

                }
            }
        }

        public void UpdateItems(List<ItemStack> itemStacks)
        {
            var i = 0;
            foreach (var grid in mGridElements)
            {
                var slot = new UiLayer { Color = new(5, 5, 5), RelHeight = .90f, RelWidth = .90f, Anchor = Anchor.Center };
                grid.ClearChilds();
                grid.AddChild(slot);

                if (i < itemStacks.Count) 
                { 
                    var itemStack = itemStacks[i];
                    slot.AddChild(new UiSprite(itemStack.Texture) { Anchor = Anchor.Center, FillScale = FillScale.Fit });
                    slot.AddChild(new UiText(FontRegistries.textFont, itemStack.Count.ToString()) { Anchor = Anchor.SE });
                }
                i++;
            }
        }

        public override void Draw()
        {
            foreach (var slot in mGridElements) slot.Draw();
            Canvas.Draw();
        }

        public override void Update(InputState inputState, Rectangle root, float uiScaling)
        {
            Canvas.UpdateFrame(root, uiScaling);
            foreach (var slot in mGridElements) slot.Update(inputState, Canvas.Bounds, uiScaling);
         }
    }
}
