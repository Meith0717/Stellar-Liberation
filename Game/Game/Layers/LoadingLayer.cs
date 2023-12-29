// LoadingLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;
using System.Collections.Generic;

namespace StellarLiberation.Game.Layers
{
    public class LoadingLayer: Layer
    {
        private readonly UiFrame mFrame;
        private readonly UiText mText;
        private readonly UiLoadingCircle mLoadingCircle;
        private readonly List<string> mFrames;
        private int mCounter;
        private int mIndex;

        public LoadingLayer(bool hasBackground = true) : base(false)
        {
            mFrame = new() { FillScale = FillScale.Both, Alpha = 0 };
            mLoadingCircle = new() { Width = 100, Height = 100, Anchor = Anchor.SE, HSpace = 10, VSpace = 10 };
            if (hasBackground) mFrame.AddChild(new UiSprite(MenueSpriteRegistries.menueBackground) { FillScale = FillScale.FillIn, Anchor = Anchor.Center });
            mFrame.AddChild(mLoadingCircle);
        }

        public override void Destroy()
        {}

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mFrame.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged() { }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mCounter += gameTime.ElapsedGameTime.Milliseconds;
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, LayerManager.ResolutionManager.UiScaling);
        }
    }
}
