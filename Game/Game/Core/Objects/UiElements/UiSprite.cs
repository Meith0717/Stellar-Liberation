﻿// UiSprite.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;

namespace StellarLiberation.Game.Core.UserInterface
{
    internal class UiSprite : UiElement
    {
        protected string mSpriteId;

        public UiSprite(string SpriteId)
        {
            mSpriteId = SpriteId;
            var texture = TextureManager.Instance.GetTexture(mSpriteId);
            mCanvas.Width = texture.Width;
            mCanvas.Height = texture.Height;
        }

        public override void Draw()
        {
            TextureManager.Instance.Draw(mSpriteId, mCanvas.Position, mCanvas.Bounds.Width, mCanvas.Bounds.Height);
            mCanvas.Draw();
        }

        public override void Initialize(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
        }

        public override void OnResolutionChanged(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
        }

        public override void Update(InputState inputState, Rectangle root) { }
    }
}