using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.DebugSystem;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.SpaceShips.Enemy;
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

        private bool IsDebug = true;
        private bool DrawBuckets;
        private bool ShowObjectsInBucket;
        private bool ShowHitBoxes;
        private bool SpawnFighter;
        private bool SpawnCorvette;
        private bool SpawnBattleShip;

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
            if (!IsDebug) 
            {
                DrawBuckets = ShowObjectsInBucket = ShowHitBoxes = false;
                return;
            }
            inputState.DoAction(ActionType.F1, () => DrawBuckets = !DrawBuckets);
            inputState.DoAction(ActionType.F2, () => ShowObjectsInBucket = !ShowObjectsInBucket);
            inputState.DoAction(ActionType.F3, () => ShowHitBoxes = !ShowHitBoxes);
            inputState.DoAction(ActionType.F4, () => SpawnFighter = true);
            inputState.DoAction(ActionType.F5, () => SpawnCorvette = true);
            inputState.DoAction(ActionType.F6, () => SpawnBattleShip = true);
        }

        public void CheckForSpawn(PlanetSystem planetSystem)
        {
            if (SpawnBattleShip) planetSystem.SpaceShips.Add(new EnemyBattleShip(Vector2.Zero));
            if (SpawnCorvette) planetSystem.SpaceShips.Add(new EnemyCorvette(Vector2.Zero));
            if (SpawnFighter) planetSystem.SpaceShips.Add(new EnemyFighter(Vector2.Zero));
            SpawnFighter = SpawnBattleShip = SpawnCorvette = false;
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
            TextureManager.Instance.DrawString("debug", position + new Vector2(0, 0), "F1 - Draw Spatial Hashing Grid", 1, DrawBuckets ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString("debug", position + new Vector2(0, 25), "F2 - Draw Objects in Bucket", 1, ShowObjectsInBucket ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString("debug", position + new Vector2(0, 50), "F3 - Show Hit Box", 1, ShowHitBoxes ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString("debug", position + new Vector2(0, 75), "F4 - Spawn Fighter", 1, Color.White);
            TextureManager.Instance.DrawString("debug", position + new Vector2(0, 100), "F5 - Spawn Corvette", 1, Color.White);
            TextureManager.Instance.DrawString("debug", position + new Vector2(0, 125), "F6 - Spawn Battle Ship", 1, Color.White);
            TextureManager.Instance.DrawString("debug", position + new Vector2(0, 150), "F7 - none", 1, Color.White);
            TextureManager.Instance.DrawString("debug", position + new Vector2(0, 175), "F8 - none", 1, false ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString("debug", position + new Vector2(0, 200), "F9 - none", 1, false ? Color.GreenYellow : Color.White);
            TextureManager.Instance.DrawString("debug", position + new Vector2(0, 225), "F10 - none", 1, false ? Color.GreenYellow : Color.White);

            List<string> debug = new() {
                $"FPS - {Math.Round(mCurrentFramesPerSecond)}", $"Render Latency - {mFrameDuration} ms",
                $"Drawn Objects - {DrawnObjectCount}", $"Updated Objects - {UpdateObjectCount}",
            };

            for (int i = 0; i < debug.Count; i++)
            {
                TextureManager.Instance.DrawString("debug", position + new Vector2(250, i * 25), debug[i], 1, Color.White);
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
