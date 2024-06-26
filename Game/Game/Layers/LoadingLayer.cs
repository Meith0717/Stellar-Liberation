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
        private readonly UiLoadingCircle mLoadingCircle;
        private readonly UiText mLoadingMessage;
        public readonly ContentLoader mContentLoader;

        public LoadingLayer(Game1 game1, ContentLoader contentLoader, bool hasBackground = true) : base(game1, false)
        {
            mContentLoader = contentLoader;
            mFrame = new() { FillScale = FillScale.Both, Alpha = 0 };
            mLoadingCircle = new() { Width = 100, Height = 100, Anchor = Anchor.SE, HSpace = 10, VSpace = 10 };
            mLoadingMessage = new("neuropolitical", "", .2f) { Anchor = Anchor.S, VSpace = 40 };
            if (hasBackground)
                mFrame.AddChild(new UiText("brolink", "Stellar\nLieberation", 1) { Anchor = Anchor.Center, Color = Color.White });
            mFrame.AddChild(mLoadingCircle);
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
            if (mContentLoader == null) return;
            mLoadingMessage.Text = mContentLoader.ProcessMessage;
        }
    }
}
