using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Space_Game.Core.InputManagement;
using Space_Game.Core.TextureManagement;
using System;
using System.Collections.Generic;

namespace Space_Game.Core.Debug
{
    public class DebugSystem
    {
        // Constants
        const float UpdateTimeInSeconds = 1;
        
        // Some Stuff
        private int mDebugLevel = 0;
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
                string.Format("Frames per second: {0}", Math.Round(mCurrentFramesPerSecond)),
                string.Format("Frame: {0} ms", mFrameDuration),
                string.Format("Debug Mode {0}", mDebugLevel),
                string.Format("Camera Zoom {0}", Globals.mCamera2d.mZoom),
                string.Format("Position {0}", Globals.mCamera2d.mPosition.ToString()),
            };
            foreach (string s in lst)
            {
                TextureManager.GetInstance().DrawString("text", position + new Vector2(0, i * 20), s, Color.White);
                i += 1;
            }
        }

        public void DrawBoundBox(RectangleF box)
        {
            if (mDebugLevel < 2) { return; }
            TextureManager.GetInstance().GetSpriteBatch().DrawRectangle(box, Color.Red, 2 / Globals.mCamera2d.mZoom, 1);
        }
        public void DrawBoundBox(CircleF box)
        {
            if (mDebugLevel < 2) { return; }
            TextureManager.GetInstance().GetSpriteBatch().DrawCircle(box, 75, Color.Red, 2 / Globals.mCamera2d.mZoom, 1);
        }
    }
}
