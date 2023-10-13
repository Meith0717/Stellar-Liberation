// MyUiMessageManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using System.Collections.Generic;


namespace CelestialOdyssey.Game.Core.UserInterface.Messages
{
    public class MyUiMessageManager
    {
        const float LiveTime = 7;
        private TextureManager mTextureManager;
        private List<MyUiMessage> mMessageList = new();
        private float mCenterX;
        private float mTopY;

        public MyUiMessageManager(float centerX, float topY)
        {
            mCenterX = centerX;
            mTopY = topY;
        }

        public void OnResolutionChanged(float centerX, float topY)
        {
            mCenterX = centerX;
            mTopY = topY;
        }

        public void Update(InputState inputState, float currentTime)
        {
            List<MyUiMessage> remove = new();
            int i = 0;
            foreach (MyUiMessage message in mMessageList)
            {
                message.Update(inputState, mCenterX, mTopY + i * 40);
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
            mMessageList.Add(new(message, creationTime, 0, 0));
        }
    }
}
