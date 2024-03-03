// GameLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParticleSystem;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.Layers;

namespace StellarLiberation.Game.Core.CoreProceses.LayerManagement
{
    public abstract class GameLayer : Layer
    {
        public Debugging.DebugSystem DebugSystem { get; protected set; }
        public Vector2 WorldMousePosition { get; private set; }
        public readonly SpatialHashing SpatialHashing;
        public readonly ParticleManager ParticleManager;
        public readonly Camera2D Camera2D;
        public readonly Camera2DShaker CameraShaker;
        public readonly GameLayerManager GameState;
        private Matrix mViewTransformationMatrix;
        protected Layer HUDLayer;

        public GameLayer(GameLayerManager gameState, int spatialHashingCellSize) : base(false)
        {
            DebugSystem = gameState.DebugSystem;

            SpatialHashing = new(spatialHashingCellSize);
            ParticleManager = new();
            Camera2D = new();
            CameraShaker = new();
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
            ParticleManager.Update(gameTime);
            CameraShaker.Update(Camera2D, gameTime);
            Camera2D.Update(GraphicsDevice, this);
            mViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera2D.Position, Camera2D.Zoom, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            WorldMousePosition = Transformations.ScreenToWorld(mViewTransformationMatrix, inputState.mMousePosition);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            DrawOnScreenView(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: mViewTransformationMatrix, samplerState: SamplerState.PointClamp);
            DrawOnWorldView(spriteBatch);
            Camera2D.Draw(this);
            ParticleManager.Draw(Camera2D);
            GameState.DebugSystem.DrawOnScene(this);
            spriteBatch.End();

            HUDLayer?.Draw(spriteBatch);
        }

        public abstract void DrawOnScreenView(SpriteBatch spriteBatch);
        public abstract void DrawOnWorldView(SpriteBatch spriteBatch);

        public override void OnResolutionChanged() { }

    }
}