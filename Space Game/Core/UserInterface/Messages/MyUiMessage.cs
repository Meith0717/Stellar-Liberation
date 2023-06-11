using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;

namespace Galaxy_Explovive.Core.UserInterface.Messages
{
    public class MyUiMessage
    {
        const string Font = "text";
        public float CreationTime;
        private TextureManager mTextureManager;
        private string mMessage;
        private float mX;
        private float mY;

        public MyUiMessage(TextureManager textureManager, string messgae, float creationTime, float centerX, float centerY)
        {
            mMessage = messgae;
            CreationTime = creationTime;
            mTextureManager = textureManager;
            Vector2 dim = textureManager.GetSpriteFont(Font).MeasureString(mMessage);
            mX = centerX - dim.X / 2;
            mY = centerY - dim.Y / 2;
        }

        public void Update(float centerX, float centerY)
        {
            Vector2 dim = mTextureManager.GetSpriteFont(Font).MeasureString(mMessage);
            mX = centerX - dim.X / 2;
            mY = centerY - dim.Y / 2;
        }

        public void Draw()
        {
            mTextureManager.DrawString(Font, new(mX, mY), mMessage, 1, Color.Yellow);
        }


    }
}
