// UiItemSlot.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using System;

namespace StellarLiberation.Game.Core.UserInterface.UiElements
{
    public class UiItemSlot : UiFrame
    {
        private readonly Action<ItemStack> mOnPressAction;
        public ItemStack ItemStack { get; private set; }

        public UiItemSlot(Action<ItemStack> onPressAction)
        {
            RelHeight = .90f;
            RelWidth = .90f;
            Anchor = Anchor.Center;
            mOnPressAction = onPressAction;
        }

        public void SetItem(ItemStack itemStack)
        {
            ClearChilds();
            ItemStack = itemStack;
            AddChild(new UiSprite(itemStack.TextureID) { FillScale = FillScale.Fit, Anchor = Anchor.Center });
            AddChild(new UiText(FontRegistries.textFont, itemStack.Amount.ToString()) { Anchor = Anchor.SE, HSpace = 5, VSpace = 5 });
        }

        public override void Update(InputState inputState, Rectangle root, float uiScaling)
        {
            base.Update(inputState, root, uiScaling);
            Color = new(5, 5, 5);
            if (!AnyChild()) return;
            if (!Canvas.Contains(inputState.mMousePosition)) return;
            Color = new(30, 30, 30);
            if (!inputState.HasAction(ActionType.LeftClick)) return;
            mOnPressAction?.Invoke(ItemStack);
        }
    }
}
