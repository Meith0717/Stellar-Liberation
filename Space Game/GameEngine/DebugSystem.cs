using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Core.GameEngine.Position_Management;
using CelestialOdyssey.Game.Core;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.GameEngine
{
    public class DebugSystem
    {
        // Constants
        const float UpdateTimeInSeconds = 1;

        // Some Stuff
        public float DrawnObjectCount;
        public float UpdateObjectCount;
        private int mDebugLevel = 0;
        private float mCurrentFramesPerSecond;
        private float mFrameDuration;
        private float mUpdateCounter = 0;
        private GameTime mGameTime;

        private void ChangeMode()
        {
            mDebugLevel += mDebugLevel > 5 ? -6 : 1;
        }

        public void Update(GameTime gameTime, InputState inputState)
        {
            if (inputState.mActionList.Contains(ActionType.ToggleDebugModes))
            {
                ChangeMode();
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

        public void ShowRenderInfo(float cameraZoom, Vector2 cameraPosition)
        {
            if (mDebugLevel < 1) { return; }
            Vector2 position = new(10, 50);
            int i = 0;
            List<string> lst = new List<string>
            {
                $"Level {mDebugLevel}",
                $"",
                $"{Math.Round(mCurrentFramesPerSecond)} FPS, {mFrameDuration} ms",
                $"",
                $"Zoom {cameraZoom}, Pos {cameraPosition.ToPoint()}",
                $"",
                $"Drawn Objects {DrawnObjectCount}",
                $"Updated Objects {UpdateObjectCount}"
            };
            foreach (string s in lst)
            {
                TextureManager.Instance.DrawString("smal", position + new Vector2(0, i * 20), s, 1, Color.White);
                i += 1;
            }
            DrawnObjectCount = 0;
            UpdateObjectCount = 0;
        }

        public void TestSpatialHashing(GameEngine engine)
        {
            if (mDebugLevel < 3) { return; }
            var radius = Globals.MouseSpatialHashingRadius;
            var GameObjects = engine.SpatialHashing.GetObjectsInBucket((int)engine.WorldMousePosition.X, (int)engine.WorldMousePosition.Y);
            Color color = Color.Green;
            for (int i = 0; i < GameObjects.Count; i++)
            {
                var obj = GameObjects[i];
                if (i > 0) color = Color.Blue;
                TextureManager.Instance.DrawAdaptiveLine(engine.WorldMousePosition, obj.Position, color,
                    2, (int)TextureManager.Instance.MaxLayerDepth, engine.Camera.Zoom);
            }
        }

        public void DrawBoundBox(CircleF box, GameEngine engine)
        {
            if (!engine.FrustumCuller.CircleOnWorldView(box)) return;
            if (mDebugLevel < 2) { return; }
            TextureManager.Instance.DrawAdaptiveCircle(box.Position, box.Radius, Color.Red, 2, (int)TextureManager.Instance.MaxLayerDepth,
                engine.Camera.Zoom);
        }
    }
}
