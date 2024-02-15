// UiLoadingCircle.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;

namespace StellarLiberation.Game.Core.UserInterface.UiElements
{
    public class UiLoadingCircle : UiElement
    {

        private readonly UiSprite mCircle;

        public UiLoadingCircle()
        {
            mCircle = new(MenueSpriteRegistries.loading) { FillScale = FillScale.FillIn, Anchor = Anchor.Center };
        }
        public override void Update(InputState inputState, GameTime gameTime, Rectangle root, float uiScaling)
        {
            Canvas.UpdateFrame(root, uiScaling);
            mCircle.Rotation += (float)(.005 * gameTime.ElapsedGameTime.TotalMilliseconds);
            mCircle.Update(inputState, gameTime, Canvas.Bounds, uiScaling);
        }

        public override void Draw()
        {
            mCircle.DrawWithRotation();
            Canvas.Draw();
        }

    }
}
