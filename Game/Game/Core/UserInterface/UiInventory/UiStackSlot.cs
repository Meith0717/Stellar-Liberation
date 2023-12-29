// UiStackSlot.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;

namespace StellarLiberation.Game.Core.UserInterface.UiInventory
{
    public class UiStackSlot : UiElement
    {

        public UiStackSlot()
        {
            Canvas.Width = 100;
            Canvas.Height = 100;
        }

        public override void Draw()
        {
            TextureManager.Instance.SpriteBatch.FillRectangle(Canvas.Bounds, Color.DarkGoldenrod, 1);
        }

        public override void Update(InputState inputState, Rectangle root, float uiScaling)
        {
            Canvas.UpdateFrame(root, uiScaling);
        }
    }
}
