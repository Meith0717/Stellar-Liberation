// UISprite.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.UserInterface
{
    internal class UiSprite : UiElement
    {
        protected string mSpriteId;

        public UiSprite(string SpriteId) 
        {
            mSpriteId = SpriteId;
            var texture = TextureManager.Instance.GetTexture(mSpriteId);
            Width = texture.Width;
            Height = texture.Height;
        }

        public override void Draw() => TextureManager.Instance.Draw(mSpriteId, Frame.Location.ToVector2(), Frame.Width, Frame.Height);

        public override void Update(InputState inputState, Rectangle root) { }
    }
}
