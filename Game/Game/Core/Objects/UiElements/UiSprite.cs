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

        public override void Initialize(Rectangle root, float UiScaling) => OnResolutionChanged(root, UiScaling);

        public override void OnResolutionChanged(Rectangle root, float UiScaling)
        {
            var texture = TextureManager.Instance.GetTexture(mSpriteId);
            Width = (int)(texture.Width * UiScaling);
            Height = (int)(texture.Height * UiScaling);
            Canvas.UpdateFrame(root, UiScaling);
        }

        public override void Update(InputState inputState, Rectangle root, float UiScaling) { System.Diagnostics.Debug.WriteLine(UiScaling); }
    }
}
