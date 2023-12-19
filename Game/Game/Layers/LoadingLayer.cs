// LoadingLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.UserInterface;
using System.Collections.Generic;

namespace StellarLiberation.Game.Layers
{
    public class LoadingLayer: Layer
    {
        private readonly UiLayer mFrame;
        private readonly UiText mText;
        private readonly List<string> mFrames;
        private int mCounter;
        private int mIndex;

        public LoadingLayer(string message, bool hasBackground = true) : base(false)
        {
            mFrame = new() { FillScale = FillScale.Both, Alpha = 0 };
            mFrames = new() { $"{message} .  ", $"{message} .. ", $"{message} ..." };
            if (hasBackground) mFrame.AddChild(new UiSprite(MenueSpriteRegistries.menueBackground) { FillScale = FillScale.Both });
            mText = new(FontRegistries.subTitleFont, message) { Anchor = Anchor.CenterV, RelY = .9f };
            mFrame.AddChild(mText);
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
            mText.Text = mFrames[mIndex];
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, mLayerManager.ResolutionManager.UiScaling);

            if (mCounter < 500) return;
            mCounter = 0;
            mIndex = (mIndex + 1) % mFrames.Count;
        }
    }
}
