// GameLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.Rendering;

namespace StellarLiberation.Game.Core.CoreProceses.LayerManagement
{
    public abstract class GameLayer : Layer
    {
        public Vector2 WorldMousePosition { get; private set; }
        public readonly SpatialHashing<GameObject2D> SpatialHashing;
        public readonly GameObjectManager ParticleManager;
        public readonly Camera2D Camera2D;
        public readonly GameState GameState;
        private Matrix mViewTransformationMatrix;
        protected Layer HUDLayer;

        public GameLayer(GameState gameState, int spatialHashingCellSize) : base(false)
        {
            SpatialHashing = new(spatialHashingCellSize);
            ParticleManager = new();
            Camera2D = new();
            GameState = gameState;
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, PersistanceManager persistanceManager, GameSettings gameSettings, ResolutionManager resolutionManager)
        {
            base.Initialize(game1, layerManager, graphicsDevice, persistanceManager, gameSettings, resolutionManager);
            HUDLayer?.Initialize(game1, layerManager, graphicsDevice, persistanceManager, gameSettings, resolutionManager);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            HUDLayer?.Update(gameTime, inputState);
            ParticleManager.Update(gameTime, inputState, this);
            Camera2D.Update(GraphicsDevice, this);
            mViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera2D.Position, Camera2D.Zoom, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            WorldMousePosition = Transformations.ScreenToWorld(mViewTransformationMatrix, inputState.mMousePosition);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            DrawOnScreenView(GameState, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: mViewTransformationMatrix, samplerState: SamplerState.PointClamp);
            DrawOnWorldView(GameState, spriteBatch);
            Camera2D.Draw(this);
            ParticleManager.Draw(this);
            GameState.DebugSystem.DrawOnScene(this);
            spriteBatch.End();

            HUDLayer?.Draw(spriteBatch);
        }

        public abstract void DrawOnScreenView(GameState gameState, SpriteBatch spriteBatch);
        public abstract void DrawOnWorldView(GameState gameState, SpriteBatch spriteBatch);

        public override void OnResolutionChanged() { }

    }
}