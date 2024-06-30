// LoadingLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.UserInterface;

namespace StellarLiberation.Game.Layers
{
    public class LoadingLayer : Layer
    {
        private readonly UiCanvas mCanvas;
        private readonly UiText mLoadingMessage;
        private readonly UiHBar mProcessBar;
        private readonly ContentLoader mContentLoader;
        private readonly UiSprite mStudioLogo;
        private float mAlpha = -.3f;

        public LoadingLayer(Game1 game1, ContentLoader contentLoader) : base(game1, false)
        {
            mContentLoader = contentLoader;
            mCanvas = new() { FillScale = FillScale.Both };

            mCanvas.AddChild(mStudioLogo = new("grandDutchInteractive") { FillScale = FillScale.FillIn, Anchor = Anchor.Center, Color = Color.Transparent });
            mCanvas.AddChild(mProcessBar = new(Color.White, "") { RelWidth = .9f, Height = 35, Anchor = Anchor.S, VSpace = 40 });
            mCanvas.AddChild(mLoadingMessage = new("neuropolitical", "", .2f) { Anchor = Anchor.S, VSpace = 41, Color = new(50, 50, 50) });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mCanvas.Draw();
            spriteBatch.End();
        }

        public override void ApplyResolution()
        {
            mCanvas.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mCanvas.Update(inputState, gameTime);
            mAlpha += .01f;
            mStudioLogo.Color = new(mAlpha, mAlpha, mAlpha, mAlpha);
            if (mContentLoader == null) return;
            mProcessBar.Percentage = mContentLoader.Process;
            mLoadingMessage.Text = mContentLoader.ProcessMessage;
        }
    }
}
