using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
 
namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    public class UiButton : UiElement
    {
        // Public Attributes
        public string Image { get; set; } = "back";
        public Action OnClickAction { get; set; } = null;
        public bool Hover { get; private set; }

        // Private Attributes
        private Texture2D Texture { get { return TextureManager.Instance.GetTexture(Image); } }

        public UiButton(UiLayer root, double relX, double relY) 
        {
            RelX = relX;
            RelY = relY;
            RelHeight = RelWidth = .5;
            Root = root;
            GetRectangle();
        }

        public override void Draw()
        {
            TextureManager textureManager = TextureManager.Instance;
            textureManager.GetSpriteBatch().Draw(Texture, Rectangle, Hover ? Color.Gray:Color.White);
        }

        public override void OnResolutionChanged()
        {
        }

        public override void Update(InputState inputState)
        {
            GetRectangle();
            Hover = false;
            if (Rectangle.Contains(inputState.mMousePosition))
            {
                Hover = true;
                if (OnClickAction == null) { return; }
                if (inputState.mMouseActionType == MouseActionType.LeftClick)
                {
                    OnClickAction();
                }
            }
        }

        private void GetRectangle()
        {
            Vector2 startPos = Vector2.Zero;
            int rootWidth = Globals.mGraphicsDevice.Viewport.Width;
            int rootHeight = Globals.mGraphicsDevice.Viewport.Height;

            if (Root != null)
            {
                startPos = new(Root.Rectangle.X, Root.Rectangle.Y);
                rootWidth = Root.Rectangle.Width;
                rootHeight = Root.Rectangle.Height;
            }

            float rootAspectRatio = (float)rootWidth / rootHeight;
            float spriteAspectRatio = (float)Texture.Width / Texture.Height;

            float width; 
            float height;
            if (rootAspectRatio > spriteAspectRatio)
            {
                height = rootHeight;
                width = height * spriteAspectRatio;
            }
            else
            {
                width = rootWidth;
                height = width / spriteAspectRatio;
            }
            startPos.X = (float)(rootWidth * RelX) - (width / 2) + startPos.X;
            startPos.Y = (float)(rootHeight * RelY) - (height / 2) + startPos.Y;

            Rectangle = new RectangleF(new Vector2(startPos.X, startPos.Y), new Vector2(width, height)).ToRectangle();
        }

    }
}
