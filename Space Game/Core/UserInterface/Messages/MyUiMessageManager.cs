using GalaxyExplovive.Core.GameEngine.Content_Management;
using GalaxyExplovive.Core.GameEngine.InputManagement;

using System.Collections.Generic;


namespace GalaxyExplovive.Core.UserInterface.Messages
{
    public class MyUiMessageManager
    {
        const float LiveTime = 7;
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

        public void Update(InputState inputState, float currentTime)
        {
            List<MyUiMessage> remove = new();
            int i = 0;
            foreach (MyUiMessage message in mMessageList)
            {
                message.Update(inputState, mCenterX, mTopY + (i * 40));
                if (currentTime > message.CreationTime + LiveTime || message.Clicked)
                {
                    mMessageList.Remove(message);
                    break;
                }
                i += 1;
            }
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
