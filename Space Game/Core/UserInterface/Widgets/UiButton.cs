using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    internal class UiButton : UiElement
    {
        public Action OnKlick { get; set; }
        private Texture2D mTexture;
        private bool mHover;


        public UiButton(UiLayer root, double relX, double relY, string buttonTexturte, float scale = 1,
            UiCanvas.RootFill fill = UiCanvas.RootFill.Fix) : base(root)
        {
            mTexture = TextureManager.Instance.GetTexture(buttonTexturte);
            Canvas = new(root, (float)relX, (float)relY, (int)(mTexture.Width * scale), (int)(mTexture.Height * scale))
            {
                Fill = fill
            };
        }

        public override void Draw()
        {
            SpriteBatch sb = TextureManager.Instance.GetSpriteBatch();
            sb.Draw(mTexture, Canvas.ToRectangle(), mHover ? Color.Gray : Color.White);
        }

        public override void OnResolutionChanged()
        {
            Canvas.OnResolutionChanged();
        }

        public override void Update(InputState inputState)
        {
            mHover = false;
            if (Canvas.ToRectangle().Contains(inputState.mMousePosition))
            {
                mHover = true;
                if (inputState.mMouseActionType == MouseActionType.LeftClick)
                {
                    if (OnKlick == null) { return; } 
                    OnKlick();
                }
            }
        }
    }
}
