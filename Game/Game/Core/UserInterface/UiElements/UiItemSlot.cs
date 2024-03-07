// UiItemSlot.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.GameObjects.Recources.Items;
using System;

namespace StellarLiberation.Game.Core.UserInterface.UiElements
{
    public class UiItemSlot : UiFrame
    {
        private readonly Action<Item> mOnPressAction;
        public Item Item { get; private set; }

        public UiItemSlot(Action<Item> onPressAction)
        {
            RelHeight = .90f;
            RelWidth = .90f;
            Anchor = Anchor.Center;
            mOnPressAction = onPressAction;
        }

        public void SetItem(Item item)
        {
            ClearChilds();
            Item = item;
            AddChild(new UiSprite(item.UiTextureId) { FillScale = FillScale.Fit, Anchor = Anchor.Center });
            if (item.IsStakable) 
                AddChild(new UiText(FontRegistries.textFont, item.Amount.ToString()) { Anchor = Anchor.SE, HSpace = 5, VSpace = 5 });
        }

        public override void Update(InputState inputState, GameTime gameTime, Rectangle root, float uiScaling)
        {
            base.Update(inputState, gameTime, root, uiScaling);
            Color = new(5, 5, 5);
            if (!AnyChild()) return;
            if (!Canvas.Contains(inputState.mMousePosition)) return;
            Color = new(30, 30, 30);
            if (!inputState.HasAction(ActionType.LeftClick)) return;
            mOnPressAction?.Invoke(Item);
        }
    }
}
