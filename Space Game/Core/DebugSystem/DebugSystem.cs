using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using System;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.Debug
{
    public class DebugSystem
    {
        // Constants
        const float UpdateTimeInSeconds = 1;
        
        // Some Stuff
        private int mDebugLevel = 5;
        private float mCurrentFramesPerSecond;
        private float mFrameDuration;
        private float mUpdateCounter = 0;
        private GameTime mGameTime;

        private void ChangeMode()
        {
            mDebugLevel++;
            if (mDebugLevel > 5) { mDebugLevel = 0; }
        }

        public void Update(GameTime gameTime, InputState inputState)
        {
            if (inputState.mActionList.Contains(ActionType.Debug))
            {
                this.ChangeMode();
            }
            mGameTime = gameTime;
        }

        public void UpdateFrameCounting()
        {
            float deltaTime = (float)mGameTime.ElapsedGameTime.Milliseconds / 1000;
            mFrameDuration = mGameTime.ElapsedGameTime.Milliseconds;
            mUpdateCounter += deltaTime;

            // Update every x Seconds
            if (UpdateTimeInSeconds - mUpdateCounter > 0.1) { return; }
            mUpdateCounter = 0;

            mCurrentFramesPerSecond = 1.0f / deltaTime;
        }

        public void ShowRenderInfo(Vector2 position)
        {
            if (mDebugLevel < 1) { return; }
            int i = 0;
            List<string> lst = new List<string>
            {
                $"Frames per second: {Math.Round(mCurrentFramesPerSecond)}",
                $"Frame: {mFrameDuration} ms",
                $"Debug Mode {mDebugLevel}",
                $"Camera Zoom {Globals.mCamera2d.mZoom}",
                $"Position {Globals.mCamera2d.mPosition}",
            };
            foreach (string s in lst)
            {
                TextureManager.Instance.DrawString("text", position + new Vector2(0, i * 20), s, Color.White);
                i += 1;
            }
        }

        public void DrawBoundBox(RectangleF box)
        {
            if (mDebugLevel < 2) { return; }
            TextureManager.Instance.GetSpriteBatch().DrawRectangle(box, Color.Red, 2 / Globals.mCamera2d.mZoom, 1);
        }
        public void DrawBoundBox(CircleF box)
        {
            if (mDebugLevel < 2) { return; }
            TextureManager.Instance.GetSpriteBatch().DrawCircle(box, 75, Color.Red, 2 / Globals.mCamera2d.mZoom, 1);
        }
    }
}
