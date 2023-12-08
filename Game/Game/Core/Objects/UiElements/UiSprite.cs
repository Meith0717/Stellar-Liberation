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

        public UiSprite(string SpriteId)
        {
            mSpriteId = SpriteId;
        }

        public override void Draw()
        {
            TextureManager.Instance.Draw(mSpriteId, Canvas.Position, Canvas.Bounds.Width, Canvas.Bounds.Height);
            Canvas.Draw();
        }

        public override void Initialize(Rectangle root, float uiScaling) => OnResolutionChanged(root, uiScaling);

        public override void OnResolutionChanged(Rectangle root, float uiScaling)
        {
            var texture = TextureManager.Instance.GetTexture(mSpriteId);
            Width = (int)(texture.Width * uiScaling);
            Height = (int)(texture.Height * uiScaling);
            Canvas.UpdateFrame(root, uiScaling);
        }

        public override void Update(InputState inputState, Rectangle root, float uiScaling) { System.Diagnostics.Debug.WriteLine(uiScaling); }
    }
}
