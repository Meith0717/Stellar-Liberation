// BenchmarkSetupLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
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
        private readonly UiFrame mBackgroundLayer;

        public BenchmarkLayer()
            : base(new(), 50000)
        {
            mBackgroundLayer = new() { Color = Color.Black, Anchor = Anchor.Center, FillScale = FillScale.FillIn };
            mBackgroundLayer.AddChild(new UiSprite(GameSpriteRegistries.gameBackground) { Anchor = Anchor.Center, FillScale = FillScale.FillIn });

            var planetSystem = new PlanetSystem(Vector2.Zero, 2);
            var objs = planetSystem.GameObjects.ToList();
            objs.AddRange(planetSystem.GetAstronomicalObjects());
            mGameObject2DManager = new(objs, this, SpatialHashing);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, LayerManager.PopLayer);
            base.Update(gameTime, inputState);
            Camera2DMover.UpdateCameraByMouseDrag(inputState, Camera2D);
            Camera2DMover.MoveByKeys(inputState, Camera2D);
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, .002f, 1);
            mGameObject2DManager.Update(gameTime, inputState, this);
            mBackgroundLayer.Update(inputState, gameTime, GraphicsDevice.Viewport.Bounds, ResolutionManager.UiScaling);
        }

        public override void Destroy()
        {
            
        }

        public override void DrawOnScreenView(SpriteBatch spriteBatch)
        {
            mBackgroundLayer.Draw();
        }

        public override void DrawOnWorldView(SpriteBatch spriteBatch)
        {
        }
    }
}
