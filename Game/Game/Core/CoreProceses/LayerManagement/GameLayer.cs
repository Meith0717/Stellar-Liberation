// GameLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Distributions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
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
        public Debugging.DebugSystem DebugSystem { get; protected set; }
        public Vector2 WorldMousePosition { get; private set; }
        public PenumbraComponent Penumbra { get; private set; }
        private GameTime GameTime;
        public GameObject2DTypeList GameObjects;
        public readonly SpatialHashing SpatialHashing;
        public readonly ParticleManager ParticleManager;
        public readonly Camera2D Camera2D;
        public readonly Camera2DShaker CameraShaker;
        public readonly GameLayerManager GameState;
        private Matrix mViewTransformationMatrix;
        protected Layer HUDLayer;

        public GameLayer(GameLayerManager gameState, int spatialHashingCellSize, Game1 game1) : base(game1, false)
        {
            DebugSystem = gameState.DebugSystem;

            SpatialHashing = new(spatialHashingCellSize);
            GameObjects = new();
            ParticleManager = new();
            Camera2D = new();
            CameraShaker = new();
            GameState = gameState;
            Penumbra = new(Game1);
            GameObject2DManager.Initialize(SpatialHashing, ref GameObjects, this);
            Penumbra.Debug = false;
            Penumbra.AmbientColor = Color.Transparent;
            Penumbra.Initialize();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            GameTime = gameTime;
            HUDLayer?.Update(gameTime, inputState);
            ParticleManager.Update(gameTime);
            CameraShaker.Update(Camera2D, gameTime);
            Camera2D.ApplyResolution(ResolutionManager.Resolution, this);
            mViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera2D.Position, Camera2D.Zoom, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            WorldMousePosition = Transformations.ScreenToWorld(mViewTransformationMatrix, inputState.mMousePosition);
            GameObject2DManager.Update(gameTime, inputState, this, ref GameObjects);
            Penumbra.Transform = mViewTransformationMatrix;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Penumbra.BeginDraw();
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(blendState: BlendState.Additive);
            DrawOnScreenView(spriteBatch);
            Penumbra.Draw(GameTime);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: mViewTransformationMatrix, samplerState: SamplerState.PointClamp);
            Camera2D.Draw(this);
            DrawOnWorldView(spriteBatch);
            ParticleManager.Draw(Camera2D);
            GameState.DebugSystem.DrawOnScene(this);
            spriteBatch.End();

            HUDLayer?.Draw(spriteBatch);
        }

        public abstract void DrawOnScreenView(SpriteBatch spriteBatch);
        public abstract void DrawOnWorldView(SpriteBatch spriteBatch);

        public override void ApplyResolution() 
        {
            HUDLayer.ApplyResolution();
            Camera2D.ApplyResolution(ResolutionManager.Resolution, this);
        }

    }
}