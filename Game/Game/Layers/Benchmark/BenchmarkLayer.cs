// BenchmarkSetupLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.DebugSystem;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using System.Linq;

namespace StellarLiberation.Game.Layers.Benchmark
{
    internal class BenchmarkLayer : GameLayer
    {
        private readonly GameObject2DManager mGameObject2DManager;
        private readonly FrameCounter mFrameCounter;
        private readonly UiFrame mBackgroundLayer;

        public BenchmarkLayer()
            : base(new(), 50000)
        {
            mBackgroundLayer = new() { Color = Color.Black, Anchor = Anchor.Center, FillScale = FillScale.FillIn };
            mBackgroundLayer.AddChild(new UiSprite(GameSpriteRegistries.gameBackground) { Anchor = Anchor.Center, FillScale = FillScale.FillIn });

            mFrameCounter = new(200);

            var planetSystem = new PlanetSystem(Vector2.Zero, 2);
            var objs = planetSystem.GameObjects.ToList();
            objs.AddRange(planetSystem.GetAstronomicalObjects());
            mGameObject2DManager = new(objs, this, SpatialHashing);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
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
            mFrameCounter.UpdateFrameCouning();
            base.Draw(spriteBatch);
            spriteBatch.Begin();
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 10), $"FPS:     {mFrameCounter.CurrentFramesPerSecond} ", 1, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 40), $"Average: {mFrameCounter.AverageFramesPerSecond} ", 1, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 70), $"Min FPS: {mFrameCounter.MinFramesPerSecond} ", 1, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 100), $"Max FPS: {mFrameCounter.MaxFramesPerSecond} ", 1, Color.White);
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(10, 130), $"Frame:   {mFrameCounter.FrameDuration} ms", 1, Color.White);
            spriteBatch.End();
        }


        public override void DrawOnScreenView(SpriteBatch spriteBatch) => mBackgroundLayer.Draw();
        public override void Destroy() { ; }
        public override void DrawOnWorldView(SpriteBatch spriteBatch) { ; }
    }
}
