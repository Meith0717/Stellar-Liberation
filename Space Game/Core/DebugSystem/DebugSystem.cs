using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using System;
using System.Collections.Generic;
using Galaxy_Explovive.Core.GameObject;
using System.Linq;
using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Game.GameLogik;

namespace Galaxy_Explovive.Core.Debug
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
            if (mGameTime == null) { return; }
            float deltaTime = (float)mGameTime.ElapsedGameTime.Milliseconds / 1000;
            mFrameDuration = mGameTime.ElapsedGameTime.Milliseconds;
            mUpdateCounter += deltaTime;

            // Update every x Seconds
            if (UpdateTimeInSeconds - mUpdateCounter > 0.1) { return; }
            mUpdateCounter = 0;

            mCurrentFramesPerSecond = 1.0f / deltaTime;
        }

        public void ShowRenderInfo(TextureManager textureManager, float cameraZoom, Vector2 cameraPosition)
        {
            Vector2 position = new(10 , 50);
            if (mDebugLevel < 1) { return; }
            int i = 0;
            List<string> lst = new List<string>
            {
                $"Frames per second: {Math.Round(mCurrentFramesPerSecond)}",
                $"Frame: {mFrameDuration} ms",
                $"Debug Mode {mDebugLevel}",
                $"Camera Zoom {cameraZoom}",
                $"Position {cameraPosition}",
            };
            foreach (string s in lst)
            {
                textureManager.DrawString("text", position + new Vector2(0, i * 20), s, 1, Color.White);
                i += 1;
            }
        }

        public void TestSpatialHashing(TextureManager textureManager, SpatialHashing<GameObject.GameObject> spatial, Vector2 mouseWorldPosition)
        {
            if (mDebugLevel < 3) { return; }
            var radius = Globals.MouseSpatialHashingRadius;
            List<InteractiveObject> GameObjects = ObjectLocator.GetObjectsInRadius<InteractiveObject>(spatial, mouseWorldPosition, radius);
            foreach (InteractiveObject obj in GameObjects) 
            {
                textureManager.DrawAdaptiveLine(mouseWorldPosition, obj.Position, Color.LightBlue, 2, textureManager.MaxLayerDepth);
            }
        }

        public void DrawBoundBox(TextureManager textureManager, CircleF box)
        {
            if (mDebugLevel < 2) { return; }
            textureManager.DrawAdaptiveCircle(box.Position, box.Radius, Color.Red, 1, textureManager.MaxLayerDepth);
        }
    }
}
