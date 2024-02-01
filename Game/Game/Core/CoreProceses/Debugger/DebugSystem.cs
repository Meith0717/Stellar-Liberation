// DebugSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.CoreProceses.Debugger
{
    public class DebugSystem
    {
        // Constants
        private const float UpdateTimeInSeconds = 1;

        // Some Stuff
        public float DrawnObjectCount;
        public float UpdateObjectCount;

        private bool IsDebug;
        private bool DrawBuckets;
        private bool ShowObjectsInBucket;
        private bool ShowHitBoxes;
        private bool ShowSensorRadius;
        private bool ShowPaths;
        private bool ShowAi;

        private bool SpawnFighter;
        private bool SpawnBomber;
        private bool SpawnBattleShip;
        private bool SpawnCarrior;

        private float mCurrentFramesPerSecond;
        private float mFrameDuration;
        private float mUpdateCounter = 0;
        private GameTime mGameTime;

        public void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleDebug, () => IsDebug = !IsDebug);
            mGameTime = gameTime;
            DrawnObjectCount = 0;
            UpdateObjectCount = 0;
            if (!IsDebug)
            {
                DrawBuckets = ShowObjectsInBucket = ShowHitBoxes = ShowSensorRadius = ShowPaths = false;
                return;
            }
            inputState.DoAction(ActionType.HideHud, () => DrawBuckets = !DrawBuckets);
            inputState.DoAction(ActionType.F2, () => ShowObjectsInBucket = !ShowObjectsInBucket);
            inputState.DoAction(ActionType.F3, () => ShowHitBoxes = !ShowHitBoxes);
            inputState.DoAction(ActionType.F4, () => ShowSensorRadius = !ShowSensorRadius);
            inputState.DoAction(ActionType.F5, () => ShowPaths = !ShowPaths);
            inputState.DoAction(ActionType.F6, () => SpawnFighter = true);
            inputState.DoAction(ActionType.F7, () => SpawnBomber = true);
            inputState.DoAction(ActionType.F8, () => SpawnBattleShip = true);
            inputState.DoAction(ActionType.F9, () => SpawnCarrior = true);
            inputState.DoAction(ActionType.F10, () => ShowAi = !ShowAi);
        }

        public void CheckForSpawn(PlanetSystemInstance planetSystem, GameLayer scene)
        {
            // if (SpawnBattleShip) planetSystem.GameObjects.AddObj(EnemyFactory.Get(EnemyId.EnemyBattleShip, scene.Camera2D.Position));
            // if (SpawnBomber) planetSystem.GameObjects.AddObj(EnemyFactory.Get(EnemyId.EnemyBomber, scene.Camera2D.Position));
            // if (SpawnFighter) planetSystem.GameObjects.AddObj(EnemyFactory.Get(EnemyId.EnemyFighter, scene.Camera2D.Position));
            // if (SpawnCarrior) planetSystem.GameObjects.AddObj(EnemyFactory.Get(EnemyId.EnemyCarrior, scene.Camera2D.Position));
            SpawnFighter = SpawnBattleShip = SpawnBomber = SpawnCarrior = false;
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
            ShowInfo(new(10, 120));
        }

        public void DrawOnScene(GameLayer scene)
        {
            if (!IsDebug) return;
            if (DrawBuckets) DebugSpatialHashing.Buckets(scene, scene.WorldMousePosition);
            if (ShowObjectsInBucket) DebugSpatialHashing.ObjectsInBucket(scene);
        }

        private void ShowInfo(Vector2 position)
        {
            TextureManager.Instance.DrawString(FontRegistries.debugFont, position + new Vector2(0, 0), "F1 - Draw Spatial Hashing Grid", 1, DrawBuckets ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, position + new Vector2(0, 25), "F2 - Draw Objects in Bucket", 1, ShowObjectsInBucket ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, position + new Vector2(0, 50), "F3 - Show Hit Box", 1, ShowHitBoxes ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, position + new Vector2(0, 75), "F4 - Show Sensor Radius", 1, ShowSensorRadius ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, position + new Vector2(0, 100), "F5 - Show Paths", 1, ShowPaths ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, position + new Vector2(0, 125), "F6 - Spawn Fighter", 1, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, position + new Vector2(0, 150), "F7 - Spawn Bomber", 1, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, position + new Vector2(0, 175), "F8 - Spawn Battle Ship", 1, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, position + new Vector2(0, 200), "F9 - Spawn Carrior", 1, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, position + new Vector2(0, 225), "F10 - Shwo Ai", 1, ShowAi ? Color.GreenYellow : Color.White);

            List<string> debug = new() {
                $"FPS - {Math.Round(mCurrentFramesPerSecond)}", $"Render Latency - {mFrameDuration} ms",
                $"Drawn Objects - {DrawnObjectCount}", $"Updated Objects - {UpdateObjectCount}",
            };

            for (int i = 0; i < debug.Count; i++)
            {
                TextureManager.Instance.DrawString(FontRegistries.debugFont, position + new Vector2(300, i * 25), debug[i], 1, Color.White);
            }
        }

        public void DrawHitbox(CircleF box, GameLayer scene)
        {
            if (!ShowHitBoxes) { return; }
            TextureManager.Instance.DrawAdaptiveCircle(box.Position, box.Radius, Color.Red, 2, (int)TextureManager.MaxLayerDepth,
                scene.Camera2D.Zoom);
        }

        public void DrawMovingDir(Vector2? target, SpaceShip spaceShip, GameLayer scene)
        {
            if (target is null | !ShowPaths) return;
            TextureManager.Instance.DrawAdaptiveLine(spaceShip.Position, Geometry.GetPointInDirection(spaceShip.Position, spaceShip.MovingDirection, spaceShip.Velocity * 100), Color.LightBlue, 2, spaceShip.TextureDepth - 1, scene.Camera2D.Zoom);
        }

        public void DrawSensorRadius(Vector2 center, float radius, GameLayer scene)
        {
            if (!ShowSensorRadius) { return; }
            CircleF box = new(center, radius);
            TextureManager.Instance.DrawAdaptiveCircle(box.Position, box.Radius, Color.LightBlue, 2, (int)TextureManager.MaxLayerDepth,
                scene.Camera2D.Zoom);
        }

        public void DrawAiDebug(CircleF circle, string message, float camZoom)
        {
            if (!ShowAi) return;
            TextureManager.Instance.DrawString(FontRegistries.subTitleFont, circle.ToRectangleF().TopRight, message, 0.2f / camZoom, Color.White);
        }
    }
}
