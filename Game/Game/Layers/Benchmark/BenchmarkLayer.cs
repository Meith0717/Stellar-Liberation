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
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.Core.CoreProceses.Profiling;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using StellarLiberation.Game.Core.Utilitys;
using System;

namespace StellarLiberation.Game.Layers.Benchmark
{
    internal class BenchmarkLayer : GameLayer
    {
        private readonly PlanetSystem mPlanetSystem;
        private readonly GameObject2DManager mGameObject2DManager;
        private readonly FrameCounter mFrameCounter;
        private readonly UiFrame mBackgroundLayer;
        private readonly DataCollector mDataCollector = new(4, ["fps", "render time", "object count", "particle count"]);
        private float CoolDown;
        private float mRunTime = 180000;

        public BenchmarkLayer()
            : base(new(), 10000)
        {
            DebugSystem = new(true);
            mBackgroundLayer = new() { Color = Color.Black, Anchor = Anchor.Center, FillScale = FillScale.FillIn };
            mBackgroundLayer.AddChild(new UiSprite(GameSpriteRegistries.gameBackground) { Anchor = Anchor.Center, FillScale = FillScale.FillIn });

            mFrameCounter = new(100);

            mPlanetSystem = new PlanetSystem(Vector2.Zero, 42);
            mPlanetSystem.GameObjects.AddRange(mPlanetSystem.GetAstronomicalObjects());
            mGameObject2DManager = new(mPlanetSystem.GameObjects, this, SpatialHashing);
            Camera2D.Zoom = 0.002f;
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            if (mRunTime < 0) End();
            if (CoolDown < 0)
            {
                CoolDown = 100;
                SpaceShipFactory.Spawn(mPlanetSystem, ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 200000)), ShipID.Corvette, Core.GameProceses.Fractions.Enemys, out var _);
                SpaceShipFactory.Spawn(mPlanetSystem, ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 200000)), ShipID.Corvette, Core.GameProceses.Fractions.Allied, out var _);
                mDataCollector.AddData([mFrameCounter.CurrentFramesPerSecond, mFrameCounter.FrameDuration, SpatialHashing.Count, ParticleManager.GameObjects2Ds.Count]);
            }
            CoolDown -= gameTime.ElapsedGameTime.Milliseconds;
            mRunTime -= gameTime.ElapsedGameTime.Milliseconds;

            DebugSystem.Update(inputState);
            mFrameCounter.Update(gameTime);
            inputState.DoAction(ActionType.ESC, LayerManager.PopLayer);
            base.Update(gameTime, inputState);
            Camera2DMover.UpdateCameraByMouseDrag(inputState, Camera2D);
            Camera2DMover.MoveByKeys(inputState, Camera2D);
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, .002f, 1);
            mGameObject2DManager.Update(gameTime, inputState, this);
            mBackgroundLayer.Update(inputState, gameTime, GraphicsDevice.Viewport.Bounds, ResolutionManager.UiScaling);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.Black);
            mFrameCounter.UpdateFrameCouning();
            base.Draw(spriteBatch);
            spriteBatch.Begin();
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 10), $"FPS:        {mFrameCounter.CurrentFramesPerSecond} ", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 25), $"Average:    {mFrameCounter.AverageFramesPerSecond} ", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 40), $"Min FPS:    {mFrameCounter.MinFramesPerSecond} ", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 55), $"Max FPS:    {mFrameCounter.MaxFramesPerSecond} ", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 70), $"Frame:      {mFrameCounter.FrameDuration} ms", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 85), $"", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 100), $"Objects:    {SpatialHashing.Count}", .75f, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 115), $"Particles:  {ParticleManager.GameObjects2Ds.Count}", .75f, Color.White);
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(mRunTime);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 150), $"{timeSpan.Minutes} min {timeSpan.Seconds}s ESC => Quit", .75f, Color.White);
            DebugSystem.ShowInfo(new Vector2(200, 10));
            spriteBatch.End();
        }

        public override void DrawOnScreenView(SpriteBatch spriteBatch) => mBackgroundLayer.Draw();
        public override void Destroy() {; }
        public override void DrawOnWorldView(SpriteBatch spriteBatch) { DebugSystem.DrawOnScene(this); }

        private void End()
        {
            DataSaver.SaveToCsv(PersistanceManager.GetSerializer(), mDataCollector, mFrameCounter, SpatialHashing.CellSize);
            LayerManager.PopLayer();
        }
    }
}
