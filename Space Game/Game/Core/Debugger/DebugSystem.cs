using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.DebugSystem;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.Debugger
{
    public class DebugSystem
    {
        // Constants
        const float UpdateTimeInSeconds = 1;

        // Some Stuff
        public float DrawnObjectCount;
        public float UpdateObjectCount;

        private bool IsDebug;
        private bool DrawBuckets;
        private bool ShowObjectsInBucket;
        private bool ShowHitBoxes;

        private float mCurrentFramesPerSecond;
        private float mFrameDuration;
        private float mUpdateCounter = 0;
        private GameTime mGameTime;

        public void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleDebugModes, () => IsDebug = !IsDebug);
            mGameTime = gameTime;
            DrawnObjectCount = 0;
            UpdateObjectCount = 0;
            if (!IsDebug) return;
            inputState.DoAction(ActionType.F1, () => DrawBuckets = !DrawBuckets);
            inputState.DoAction(ActionType.F2, () => ShowObjectsInBucket = !ShowObjectsInBucket);
            inputState.DoAction(ActionType.F3, () => ShowHitBoxes = !ShowHitBoxes);
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

        public void DrawOnScreen()
        {
            if (!IsDebug) return;
            ShowInfo(new(10, 10));
        }

        public void DrawOnScene(Scene scene)
        {
            if (!IsDebug) return;
            if (DrawBuckets) DebugSpatialHashing.Buckets(scene, scene.WorldMousePosition);
            if (ShowObjectsInBucket) DebugSpatialHashing.ObjectsInBucket(scene);
        }

        private void ShowInfo(Vector2 position)
        {
            TextureManager.Instance.DrawString("smal", position + new Vector2(0, 0), "F1 - Draw Spatial Hashing Grid", 1, DrawBuckets ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString("smal", position + new Vector2(0, 20), "F2 - Draw Objects in Bucket", 1, ShowObjectsInBucket ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString("smal", position + new Vector2(0, 40), "F3 - Show Hit Box", 1, ShowHitBoxes ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString("smal", position + new Vector2(0, 60), "F4 - none", 1, false ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString("smal", position + new Vector2(0, 80), "F5 - none", 1, false ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString("smal", position + new Vector2(0, 100), "F6 - none", 1, false ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString("smal", position + new Vector2(0, 120), "F7 - none", 1, false ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString("smal", position + new Vector2(0, 140), "F8 - none", 1, false ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString("smal", position + new Vector2(0, 160), "F9 - none", 1, false ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString("smal", position + new Vector2(0, 180), "F10 - none", 1, false ? Color.GreenYellow : Color.White);

            List<string> debug = new() {
                $"FPS - {Math.Round(mCurrentFramesPerSecond)}", $"Render Latency - {mFrameDuration} ms",
                $"Drawn Objects - {DrawnObjectCount}", $"Updated Objects - {UpdateObjectCount}",
            };

            for (int i = 0; i < debug.Count; i++)
            {
                TextureManager.Instance.DrawString("smal", position + new Vector2(210, i * 20), debug[i], 1, Color.White);
            }
        }


        public void DrawBoundBox(CircleF box, Scene scene)
        {
            if (!ShowHitBoxes) { return; }
            TextureManager.Instance.DrawAdaptiveCircle(box.Position, box.Radius, Color.Red, 2, (int)TextureManager.Instance.MaxLayerDepth,
                scene.Camera.Zoom);
        }
    }
}
