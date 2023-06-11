using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Galaxy_Explovive.Core.UserInterface.UiCanvas;

namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    public class UiText : UiElement
    {
        public Color FontColor { get; set; } = Color.Black;
        public string Text { get; set; } = "UiText";
        public float RelativX = 0.5f;
        public float RelativY = 0.5f;
        public RootSide Side = RootSide.None;
        public int MarginX = 0;
        public int MarginY = 0;

        private string mFont = "text";

        public UiText(UiFrame root, TextureManager textureManager, GraphicsDevice graphicsDevice) 
            : base(root, textureManager, graphicsDevice)
        {
            mCanvas = new(root);
        }

        public override void Draw()
        {
            Vector2 position = new(mCanvas.ToRectangle().X, mCanvas.ToRectangle().Y);
            mTextureManager.DrawString(mFont, position, Text, 1, FontColor);
        }

        public override void OnResolutionChanged()
        {
        }

        public override void Update(InputState inputState)
        {
            Vector2 stringDimensions = mTextureManager.GetSpriteFont(mFont).MeasureString(Text);
            mCanvas.Width = stringDimensions.X;
            mCanvas.Height = stringDimensions.Y;
            mCanvas.RelativeX = RelativX;
            mCanvas.RelativeY = RelativY;
            mCanvas.Side = Side;
            mCanvas.MarginX = MarginX;
            mCanvas.MarginY = MarginY;
            mCanvas.OnResolutionChanged(mGraphicsDevice);
        }
    }
}
