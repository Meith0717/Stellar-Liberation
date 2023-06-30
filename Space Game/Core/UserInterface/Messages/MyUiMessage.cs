using Galaxy_Explovive.Core.GameEngine.InputManagement;
using Galaxy_Explovive.Core.GameEngine.Rendering;
using Microsoft.Xna.Framework;

namespace Galaxy_Explovive.Core.UserInterface.Messages
{
    public class MyUiMessage
    {
        const string Font = "text";
        public float CreationTime;
        public bool Clicked = false;
        private TextureManager mTextureManager;
        private Rectangle mRect = Rectangle.Empty;
        private string mMessage;
        private float mX;
        private float mY;
        private float mWidth;
        private float mHeight;
        private Color Color;

        public MyUiMessage(TextureManager textureManager, string messgae, float creationTime, float centerX, float centerY)
        {
            mMessage = messgae;
            CreationTime = creationTime;
            mTextureManager = textureManager;
            Vector2 dim = textureManager.GetSpriteFont(Font).MeasureString(mMessage);
            mWidth = dim.X;
            mHeight = dim.Y;
            mX = centerX - mWidth / 2;
            mY = centerY - mHeight / 2;
        }

        public void Update(InputState inputState, float centerX, float centerY)
        {
            Vector2 dim = mTextureManager.GetSpriteFont(Font).MeasureString(mMessage);
            mX = centerX - dim.X / 2;
            mY = centerY - dim.Y / 2;
            mRect.X = (int)mX - 5;
            mRect.Y = (int)mY - 5;
            mRect.Width = (int)mWidth + 10;
            mRect.Height = (int)mHeight + 10;
            Color = Color.Transparent;
            if (mRect.Contains(inputState.mMousePosition))
            {
                Color = new Color(20, 20, 20, 20);
                if (inputState.mMouseActionType != MouseActionType.LeftClick) { return; }
                Clicked = true;
            }
        }

        public void Draw()
        {
            mTextureManager.SpriteBatch.Draw(mTextureManager.GetTexture("Layer"), mRect, Color);
            mTextureManager.DrawString(Font, new(mX, mY), mMessage, 1, Color.OrangeRed);
        }


    }
}
