
// UiSprite.cs 
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
        public float Rotation;
        public float Scale;

        public UiSprite(string SpriteId, float scale = 1)
        {
            mSpriteId = SpriteId;
            Scale = scale;
        }

        public override void Draw()
        {
            TextureManager.Instance.Draw(mSpriteId, Canvas.Position, Canvas.Bounds.Width, Canvas.Bounds.Height);
            Canvas.Draw();
        }

        public void DrawWithRotation()
        {
            TextureManager.Instance.Draw(mSpriteId, Canvas.Position, Canvas.Bounds.Width, Canvas.Bounds.Height, Canvas.Offset, Rotation);
            Canvas.Draw();
        }

        public override void Update(InputState inputState, Rectangle root, float uiScaling)
        {
            var texture = TextureManager.Instance.GetTexture(mSpriteId);
            Width = (int)(texture.Width * Scale);
            Height = (int)(texture.Height * Scale);
            Canvas.UpdateFrame(root, uiScaling);
        }
    }
}
