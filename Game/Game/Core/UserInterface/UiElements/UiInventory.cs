// UiInventory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.GameObjects.Recources.Items;
using System;

namespace StellarLiberation.Game.Core.UserInterface.UiElements
{
    internal class UiInventory : UiElement
    {
        private readonly UiGrid mGrid;
        private readonly Inventory mInventory;
        private readonly Action<Item> mAction;

        private readonly int I;
        private readonly int J;

        public UiInventory(Inventory inventory, Action<Item> action, int i = 5, int j = 6)
        {
            mGrid = new(i, j) { FillScale = FillScale.Both, Anchor = Anchor.Center };
            mInventory = inventory;
            mAction = action;
            I = i;
            J = j;
        }

        public override void Draw()
        {
            Canvas.Draw();
            mGrid.Draw();
        }

        public override void ApplyResolution()
        {
            throw new NotImplementedException();
        }

        public override void Update(InputState inputState, GameTime gameTime, Rectangle root, float uiScaling)
        {
            var i = 0;
            for (int y = 0; y < J; y++)
            {
                for (int x = 0; x < I; x++)
                {
                    var slot = new UiItemSlot(mAction);
                    mGrid.Set(x, y, slot);
                    if (i >= mInventory.ItemStacks.Count) continue;
                    var item = mInventory.ItemStacks[i];
                    slot.SetItem(item);
                    i++;
                }
            }

            Canvas.UpdateFrame(root, uiScaling);
            mGrid.Update(inputState, gameTime, Bounds, uiScaling);
        }
    }
}
