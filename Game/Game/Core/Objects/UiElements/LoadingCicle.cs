// LoadingCicle.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.UserInterface;

namespace StellarLiberation.Game.Core.Objects.UiElements
{
    public class LoadingCicle : UiElement
    {
        protected string mSpriteId;
        public float Rotation;

        public LoadingCicle(string SpriteId)
        {
            mSpriteId = SpriteId;
        }

        public override void Draw()
        {
            TextureManager.Instance.Draw(mSpriteId, Canvas.Position, Canvas.Bounds.Width, Canvas.Bounds.Height, Rotation);
            Canvas.Draw();
        }

        public override void Update(InputState inputState, RectangleF root, float uiScaling)
        {
            var texture = TextureManager.Instance.GetTexture(mSpriteId);
            Width = texture.Width;
            Height = texture.Height;
            Canvas.UpdateFrame(root, uiScaling);
        }
    }
}
