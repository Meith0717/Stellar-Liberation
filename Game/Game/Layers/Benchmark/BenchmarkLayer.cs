// BenchmarkLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.Debugging;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Profiling;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System;

namespace StellarLiberation.Game.Layers.Benchmark
{
    internal class BenchmarkLayer : GameLayer
    {
        private readonly PlanetSystem mPlanetSystem;
        private readonly FrameCounter mFrameCounter;
        private readonly UiFrame mBackgroundLayer;
        private readonly DataCollector mDataCollector = new(4, ["fps", "render time", "object count", "particle count"]);
        private float CoolDown;
        private float mRunTime = 180000;
        private bool mIsPaused;

        public BenchmarkLayer(Game1 game1)
            : base(new(game1), new(10000), game1)
        {
            mBackgroundLayer = new() { Color = Color.Black, Anchor = Anchor.Center, FillScale = FillScale.FillIn };
            mBackgroundLayer.AddChild(new UiSprite(GameSpriteRegistries.gameBackground) { Anchor = Anchor.Center, FillScale = FillScale.FillIn });

            mFrameCounter = new(100);

            //mPlanetSystem = new PlanetSystem(Vector2.Zero, 42);
            Camera2D.Zoom = 0.002f;
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            if (mRunTime < 0) End();
            if (CoolDown < 0 && !mIsPaused)
            {
                CoolDown = 100;
                var enemy = SpaceshipFactory.Get(ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 200000)), ShipID.Corvette, Core.GameProceses.Fractions.Enemys);
                var allied = SpaceshipFactory.Get(ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 200000)), ShipID.Corvette, Core.GameProceses.Fractions.Allied);
                //mPlanetSystem.GameObjects.Add(allied);
                //mPlanetSystem.GameObjects.Add(enemy);
                mDataCollector.AddData([mFrameCounter.CurrentFramesPerSecond, mFrameCounter.FrameDuration, SpatialHashing.Count, 0]);
            }
            CoolDown -= gameTime.ElapsedGameTime.Milliseconds;
            mRunTime -= gameTime.ElapsedGameTime.Milliseconds;

            DebugSystem.Update(inputState);
            mFrameCounter.Update(gameTime);
            inputState.DoAction(ActionType.ESC, LayerManager.PopLayer);
            inputState.DoAction(ActionType.BenchmarkPause, () => mIsPaused = !mIsPaused);
            base.Update(gameTime, inputState);
            Camera2DMover.UpdateCameraByMouseDrag(inputState, Camera2D);
            Camera2DMover.MoveByKeys(gameTime, inputState, Camera2D);
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, .002f, 1);
            mBackgroundLayer.Update(inputState, gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mFrameCounter.UpdateFrameCouning();

            spriteBatch.Begin();
            mBackgroundLayer.Draw();
            spriteBatch.End();

            base.Draw(spriteBatch);

            spriteBatch.Begin();
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 10), $"FPS:        {mFrameCounter.CurrentFramesPerSecond} ", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 25), $"Average:    {mFrameCounter.AverageFramesPerSecond} ", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 40), $"Min FPS:    {mFrameCounter.MinFramesPerSecond} ", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 55), $"Max FPS:    {mFrameCounter.MaxFramesPerSecond} ", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 70), $"Frame:      {mFrameCounter.FrameDuration} ms", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 85), $"", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 100), $"Objects:    {SpatialHashing.Count}", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 115), $"Particles:  N.A", .75f, Color.White);
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(mRunTime);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 150), $"{timeSpan.Minutes} min {timeSpan.Seconds}s ESC => Quit", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 165), $"Left control => Pause Spawning", .75f, mIsPaused ? Color.LightGreen : Color.White);
            DebugSystem.ShowInfo(new Vector2(200, 10));
            DebugSystem.DrawOnScene(this);
            spriteBatch.End();
        }

        private void End()
        {
            DataSaver.SaveToCsv(PersistanceManager.GetSerializer(), mDataCollector, mFrameCounter, SpatialHashing.CellSize);
            LayerManager.PopLayer();
        }
    }
}
