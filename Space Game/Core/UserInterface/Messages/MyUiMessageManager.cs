using Galaxy_Explovive.Core.TextureManagement;
using System.Collections.Generic;


namespace Galaxy_Explovive.Core.UserInterface.Messages
{
    public class MyUiMessageManager
    {
        const float LiveTime = 10;
        private TextureManager mTextureManager;
        private List<MyUiMessage> mMessageList = new();
        private float mCenterX;
        private float mTopY;

        public MyUiMessageManager(TextureManager textureManager, float centerX, float topY)
        {
            this.mTextureManager = textureManager;
            this.mCenterX = centerX;
            this.mTopY = topY;
        }

        public void OnResolutionChanged(float centerX, float topY)
        {
            this.mCenterX = centerX;
            this.mTopY = topY;
        }

        public void Update(float currentTime)
        {
            List<MyUiMessage> remove = new();
            int i = 0;
            foreach (MyUiMessage message in mMessageList)
            {
                message.Update(mCenterX, mTopY + (i * 20));
                if (currentTime <= message.CreationTime + LiveTime) { i += 1; continue; }
                remove.Add(message);
            }
            foreach (MyUiMessage message in remove) { mMessageList.Remove(message); }
        }

        public void Draw()
        {
            foreach (MyUiMessage message in mMessageList)
            {
                message.Draw();
            }
        }

        public void AddMessage(string message, float creationTime)
        {
            mMessageList.Add(new(mTextureManager, message, creationTime, 0, 0));
        }
    }
}
