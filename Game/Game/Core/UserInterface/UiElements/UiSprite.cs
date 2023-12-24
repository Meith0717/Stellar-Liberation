
// UiSprite.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using System;

namespace StellarLiberation.Game.Core.UserInterface
{
    internal class UiSprite : UiElement
    {
        protected string mSpriteId;
        public float Scale = 1;

        public UiSprite(string SpriteId)
        {
            mSpriteId = SpriteId;
        }

        public override void Draw()
        {
            TextureManager.Instance.Draw(mSpriteId, Canvas.Position, Canvas.Bounds.Width, Canvas.Bounds.Height);
            Canvas.Draw();
        }

        public void Draw(float rotation)
        {
            TextureManager.Instance.Draw(mSpriteId, Canvas.Position, Canvas.Bounds.Width, Canvas.Bounds.Height, Canvas.Offset, rotation);
            Canvas.Draw();
        }

        public override void Update(InputState inputState, RectangleF root, float uiScaling)
        {
            var texture = TextureManager.Instance.GetTexture(mSpriteId);
            Width = texture.Width * Scale;
            Height = texture.Height * Scale;
            Canvas.UpdateFrame(root, uiScaling);
        }
    }
}
