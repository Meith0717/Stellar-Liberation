// UiStackSlot.cs 
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
    public class UiStackSlot : UiFrame
    {
        private readonly Action<ItemStack> mOnPressAction;
        private readonly ItemStack mItemStack;

        public UiStackSlot(Action<ItemStack> onPressAction, ItemStack itemStack)
        {
            RelHeight = .90f;
            RelWidth = .90f;
            Anchor = Anchor.Center;
            mOnPressAction = onPressAction;
            mItemStack = itemStack;
            AddChild(new UiSprite(itemStack.Texture) { FillScale = FillScale.Fit, Anchor = Anchor.Center });
            AddChild(new UiText(FontRegistries.textFont, itemStack.Count.ToString()) { Anchor = Anchor.SE });
        }

        public UiStackSlot()
        {
            RelHeight = .90f;
            RelWidth = .90f;
            Anchor = Anchor.Center;
        }

        public override void Update(InputState inputState, Rectangle root, float uiScaling)
        {
            base.Update(inputState, root, uiScaling);
            Color = new(5, 5, 5);
            if (!AnyChild()) return;
            if (!Canvas.Contains(inputState.mMousePosition)) return;
            Color = new(15, 15, 15);
            if (!inputState.HasAction(ActionType.LeftClick)) return;
            mOnPressAction?.Invoke(mItemStack);
        }
    }
}
