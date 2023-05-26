﻿using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using System;
using System.Collections.Generic;
using Galaxy_Explovive.Core.GameObject;
using System.Linq;
using System.ComponentModel;
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
        private Vector2 mMousePosition;

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
            mMousePosition = Globals.Camera2d.ViewToWorld(inputState.mMousePosition.ToVector2());
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

        public void ShowRenderInfo(TextureManager textureManager, Vector2 position)
        {
            if (mDebugLevel < 1) { return; }
            int i = 0;
            List<string> lst = new List<string>
            {
                $"Frames per second: {Math.Round(mCurrentFramesPerSecond)}",
                $"Frame: {mFrameDuration} ms",
                $"Debug Mode {mDebugLevel}",
                $"Camera Zoom {Globals.Camera2d.mZoom}",
                $"Position {Globals.Camera2d.Position}",
            };
            foreach (string s in lst)
            {
                textureManager.DrawString("text", position + new Vector2(0, i * 20), s, Color.White);
                i += 1;
            }
        }

        public void DrawNearMousObjects(TextureManager textureManager, SpatialHashing<GameObject.GameObject> spatial)
        {
            if (mDebugLevel < 3) { return; }
            var radius = Globals.MouseSpatialHashingRadius;
            List<InteractiveObject> GameObjects = ObjectLocator.GetObjectsInRadius(spatial, mMousePosition, radius).OfType<InteractiveObject>().ToList(); ;
            System.Diagnostics.Debug.WriteLine(GameObjects.Count);
            if (GameObjects.Count() == 0) { return; }
            textureManager.DrawAdaptiveLine(mMousePosition, GameObjects[0].Position, Color.LightBlue, 2, 0);
        }

        public void DrawBoundBox(TextureManager textureManager, RectangleF box)
        {
            if (mDebugLevel < 2) { return; }
            textureManager.SpriteBatch.DrawRectangle(box, Color.Red, 2 / Globals.Camera2d.mZoom, 1);
        }
        public void DrawBoundBox(TextureManager textureManager, CircleF box)
        {
            if (mDebugLevel < 2) { return; }
            textureManager.SpriteBatch.DrawCircle(box, 75, Color.Red, 2 / Globals.Camera2d.mZoom, 1);
        }
    }
}
