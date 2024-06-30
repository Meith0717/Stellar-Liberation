// LoadingLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;

namespace StellarLiberation.Game.Layers
{
    public class LoadingLayer : Layer
    {
        private readonly UiFrame mFrame;
        private readonly UiText mLoadingMessage;
        private readonly UiHBar mProcessBar;
        private readonly ContentLoader mContentLoader;
        private readonly UiSprite mStudioLogo;
        private float mAlpha = -.3f;

        public LoadingLayer(Game1 game1, ContentLoader contentLoader) : base(game1, false)
        {
            mContentLoader = contentLoader;
            mFrame = new() { FillScale = FillScale.Both, Alpha = 0 };
            mLoadingMessage = new("neuropolitical", "", .2f) { Anchor = Anchor.S, VSpace = 70 };
            mProcessBar = new(Color.White, "") { RelWidth = .7f, Height = 20, Anchor = Anchor.S, VSpace = 40 };
            mFrame.AddChild(mStudioLogo = new("grandDutchInteractive") { FillScale = FillScale.FillIn, Anchor = Anchor.Center, Color = Color.Transparent });
            mFrame.AddChild(mProcessBar);
            mFrame.AddChild(mLoadingMessage);
        }

        public override void Destroy()
        { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mFrame.Draw();
            spriteBatch.End();
        }

        public override void ApplyResolution()
        {
            mFrame.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mFrame.Update(inputState, gameTime);
            mAlpha += .01f;
            mStudioLogo.Color = new(mAlpha, mAlpha, mAlpha, mAlpha);
            if (mContentLoader == null) return;
            mProcessBar.Percentage = mContentLoader.Process;
            mLoadingMessage.Text = mContentLoader.ProcessMessage;
        }
    }
}
