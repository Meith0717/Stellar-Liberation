// UiLoadingCircle.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;

namespace StellarLiberation.Game.Core.UserInterface.UiElements
{
    public class UiLoadingCircle : UiElement
    {

        private readonly UiSprite mCircle;

        public UiLoadingCircle()
        {
            mCircle = new("loading") { FillScale = FillScale.FillIn, Anchor = Anchor.Center };
        }
        public override void Update(InputState inputState, GameTime gameTime)
        {
            mCircle.Rotation += (float)(.005 * gameTime.ElapsedGameTime.TotalMilliseconds);
            mCircle.Update(inputState, gameTime);
        }

        public override void Draw()
        {
            mCircle.DrawWithRotation();
            Canvas.Draw();
        }

        public override void ApplyResolution(Rectangle root, Resolution resolution)
        {
            Canvas.UpdateFrame(root, resolution.UiScaling);
            mCircle.ApplyResolution(Bounds, resolution);
        }
    }
}
