// GameLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParticleSystem;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.Layers;

namespace StellarLiberation.Game.Core.CoreProceses.LayerManagement
{
    public abstract class GameLayer : Layer
    {
        public Vector2 WorldMousePosition { get; private set; }
        public Matrix ViewTransformationMatrix { get; private set; }   
        public GameObject2DManager GameObjectsManager;
        public readonly Debugging.DebugSystem DebugSystem;
        public readonly SpatialHashing SpatialHashing;
        public readonly ParticleManager ParticleManager;
        public readonly Camera2D Camera2D;
        public readonly Camera2DShaker CameraShaker;
        public readonly GameLayerManager GameState;
        protected Layer HUDLayer;

        public GameLayer(GameLayerManager gameState, int spatialHashingCellSize, Game1 game1) : base(game1, false)
        {
            DebugSystem = gameState.DebugSystem?? new(true);

            SpatialHashing = new(spatialHashingCellSize);
            GameObjectsManager = new();
            ParticleManager = new();
            Camera2D = new();
            CameraShaker = new();
            GameState = gameState;
            GameObjectsManager.Initialize(SpatialHashing);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            HUDLayer?.Update(gameTime, inputState);
            ParticleManager.Update(gameTime);
            CameraShaker.Update(Camera2D, gameTime);
            Camera2D.ApplyResolution(ResolutionManager.Resolution, this);
            ViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera2D.Position, Camera2D.Zoom, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            WorldMousePosition = Transformations.ScreenToWorld(ViewTransformationMatrix, inputState.mMousePosition);
            GameObjectsManager.Update(gameTime, inputState, this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: ViewTransformationMatrix, samplerState: SamplerState.PointClamp);
            Camera2D.Draw(this);
            ParticleManager.Draw(Camera2D);
            GameState.DebugSystem.DrawOnScene(this);
            spriteBatch.End();

            HUDLayer?.Draw(spriteBatch);
        }

        public override void ApplyResolution() 
        {
            HUDLayer?.ApplyResolution();
            Camera2D.ApplyResolution(ResolutionManager.Resolution, this);
        }

        public override void Destroy() 
        { 
            SpatialHashing.ClearBuckets();
            ParticleManager.Clear();
        }

    }
}