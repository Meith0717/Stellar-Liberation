// UiSprite.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;

namespace StellarLiberation.Game.Core.UserInterface
{
    public class UiSprite : UiElement
    {
        protected string mSpriteId;
        public float Rotation;
        public float Scale;
        public Color Color = Color.White;

        public UiSprite(string SpriteId, float scale = 1)
        {
            mSpriteId = SpriteId;
            Scale = scale;
        }

        public override void Draw()
        {
            TextureManager.Instance.Draw(mSpriteId, Canvas.Position, Canvas.Bounds.Width, Canvas.Bounds.Height, Color);
            Canvas.Draw();
        }

        public void DrawWithRotation()
        {
            TextureManager.Instance.Draw(mSpriteId, Canvas.Position, Canvas.Bounds.Width, Canvas.Bounds.Height, Canvas.Offset, Rotation);
            Canvas.Draw();
        }

        public override void ApplyResolution(Rectangle root, Resolution resolution)
        {
            var texture = TextureManager.Instance.GetTexture(mSpriteId);
            Width = (int)(texture.Width * Scale);
            Height = (int)(texture.Height * Scale);
            Canvas.UpdateFrame(root, resolution.UiScaling);
        }

        public override void Update(InputState inputState, GameTime gameTime)
        {; }
    }
}
