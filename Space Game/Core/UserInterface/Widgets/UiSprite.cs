using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Diagnostics;

namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    public class UiSprite : UiElement
    {
        public enum SpriteFit
        {
            Fit,  // sprite is fit into frame without distortion
            Cover,  // sprite gets distorted to the frame
            Fixed,  // sprite is original size
            Fill  // sprite is filling the frame without distortion
        }

        // Public Attributes
        public SpriteFit SpriteAnchor { get; set; } = SpriteFit.Fill;
        public double Alpha { get; set; } = 1;
        public string Image { get; set; } = "Monogame";
        
        // Private Attributes
        private Texture2D Texture { get { return TextureManager.Instance.GetTexture(Image); } }

        public UiSprite(UiLayer root, double relX, double relY) 
        {
            this.RelX = relX;
            this.RelY = relY;
            RelHeight = RelWidth = .5;
            Root = root;
            GetRectangle();
        }

        public override void Draw()
        {
            TextureManager textureManager = TextureManager.Instance;
            textureManager.GetSpriteBatch().Draw(Texture, Rectangle, Color.White);
        }

        public override void OnResolutionChanged()
        {
        }

        public override void Update(InputState inputState)
        {
            GetRectangle();
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

            float width = rootWidth; float height = rootHeight;
            switch (SpriteAnchor)
            {
                case SpriteFit.Cover:
                    width = rootWidth;
                    height = rootHeight;
                    break;

                case SpriteFit.Fixed:
                    width = Texture.Width;
                    height = Texture.Height;
                    startPos.X = (float)(rootWidth * RelX) - (width / 2) + startPos.X;
                    startPos.Y = (float)(rootHeight * RelY) - (height / 2) + startPos.Y;
                    break;

                case SpriteFit.Fit:
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
                    break;

                case SpriteFit.Fill:
                    if (rootAspectRatio < spriteAspectRatio)
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
                    break;
            }
            Rectangle = new RectangleF(new Vector2(startPos.X, startPos.Y), new Vector2(width, height)).ToRectangle();
        }
    }
}