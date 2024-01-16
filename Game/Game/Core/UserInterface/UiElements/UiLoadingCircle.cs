// UiLoadingCircle.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarLiberation.Game.Core.UserInterface.UiElements
{
    public class UiLoadingCircle : UiElement
    {

        private readonly UiSprite mCircle;

        public UiLoadingCircle()
        {
            mCircle = new(MenueSpriteRegistries.loading) { FillScale = FillScale.FillIn, Anchor = Anchor.Center };
        }
        public override void Update(InputState inputState, Rectangle root, float uiScaling)
        {
            Canvas.UpdateFrame(root, uiScaling);
            mCircle.Rotation += .1f;
            mCircle.Update(inputState, Canvas.Bounds, uiScaling);
        }

        public override void Draw()
        {
            mCircle.DrawWithRotation();
            Canvas.Draw();
        }

    }
}
