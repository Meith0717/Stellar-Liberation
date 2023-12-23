// UiTooltip.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;

namespace StellarLiberation.Game.Core.UserInterface.UiElements
{
    public class UiTooltip : UiText
    {
        public UiTooltip(string text) : base(FontRegistries.textFont, text)
        {

        }

        public override void Update(InputState inputState, RectangleF root, float uiScaling)
        {
            Canvas.X = inputState.mMousePosition.X;
            Canvas.Y = inputState.mMousePosition.Y;
            base.Update(inputState, root, uiScaling);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
