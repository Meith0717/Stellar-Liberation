// UiSprite.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;

namespace StellarLiberation.Game.Core.UserInterface
{
    public class UiSprite(string SpriteId, float scale = 1) : UiElement
    {
        protected string mSpriteId = SpriteId;
        public float Rotation = 0;
        public float Scale = scale;
        public Color Color = Color.White;

        public override void Draw()
        {
            TextureManager.Instance.Draw(mSpriteId, Position, Bounds.Width, Bounds.Height, Color);
            DrawCanvas();
        }

        public void DrawWithRotation()
        {
            TextureManager.Instance.Draw(mSpriteId, Position, Bounds.Width, Bounds.Height, Offset, Rotation);
            DrawCanvas();
        }

        public override void ApplyResolution(Rectangle root, Resolution resolution)
        {
            var texture = TextureManager.Instance.GetTexture(mSpriteId);
            Width = (int)(texture.Width * Scale);
            Height = (int)(texture.Height * Scale);
            base.ApplyResolution(root, resolution);
        }
    }
}
