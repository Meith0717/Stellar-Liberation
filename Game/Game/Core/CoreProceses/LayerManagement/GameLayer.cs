// GameLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParticleSystem;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.Layers;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.CoreProceses.LayerManagement
{
    public abstract class GameLayer : Layer
    {
        public Vector2 WorldMousePosition { get; private set; }
        public Matrix ViewTransformationMatrix { get; private set; }
        public readonly Debugging.DebugSystem DebugSystem;
        public readonly SpatialHashing SpatialHashing;
        public readonly Camera2D Camera2D;
        public readonly Camera2DShaker CameraShaker;
        public readonly GameState GameState;
        protected Layer HUDLayer;

        protected readonly StereoSoundSystem mStereoSoundSystem = new();
        protected readonly ParticleManager mParticleManager = new();

        public GameLayer(GameState gameState, SpatialHashing spatialHashing, Game1 game1) 
            : base(game1, false)
        {
            DebugSystem = gameState.DebugSystem ?? new(true);

            SpatialHashing = spatialHashing;
            Camera2D = new();
            CameraShaker = new();
            GameState = gameState;
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
            HUDLayer?.Update(gameTime, inputState);
            CameraShaker.Update(Camera2D, gameTime);
            Camera2D.ApplyResolution(ResolutionManager.Resolution, this);
            ViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera2D.Position, Camera2D.Zoom, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            WorldMousePosition = Transformations.ScreenToWorld(ViewTransformationMatrix, inputState.mMousePosition);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: ViewTransformationMatrix, samplerState: SamplerState.PointClamp);
            Camera2D.Draw(GameState, this);
            mParticleManager.Draw(Camera2D);
            GameState.DebugSystem.DrawOnScene(this);
            spriteBatch.End();

            HUDLayer?.Draw(spriteBatch);
        }

        public override void ApplyResolution()
        {
            base.ApplyResolution();
            HUDLayer?.ApplyResolution();
            Camera2D.ApplyResolution(ResolutionManager.Resolution, this);
        }

        public override void Destroy()
        {
            base.Destroy();
            SpatialHashing.ClearBuckets();
            mParticleManager.Clear();
        }

    }
}