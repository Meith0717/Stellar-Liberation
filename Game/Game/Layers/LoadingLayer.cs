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

namespace StellarLiberation.Game.Layers
{
    public class LoadingLayer : Layer
    {
        private readonly UiFrame mFrame;
        private readonly UiLoadingCircle mLoadingCircle;

        public LoadingLayer(bool hasBackground = true) : base(false)
        {
            mFrame = new() { FillScale = FillScale.Both, Alpha = 0 };
            mLoadingCircle = new() { Width = 100, Height = 100, Anchor = Anchor.SE, HSpace = 10, VSpace = 10 };
            if (hasBackground) mFrame.AddChild(new UiText(FontRegistries.titleFont, "Stellar\nLieberation") { Anchor = Anchor.Center });
            mFrame.AddChild(mLoadingCircle);
        }

        public override void Destroy()
        { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mFrame.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged() { }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, LayerManager.ResolutionManager.UiScaling);
        }
    }
}
