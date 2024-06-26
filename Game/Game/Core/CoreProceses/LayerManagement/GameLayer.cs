// GameLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParticleSystem;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.Layers;

namespace StellarLiberation.Game.Core.CoreProceses.LayerManagement
{
    /// <summary>
    /// The GameLayer class serves as a foundational layer for managing game elements. It encapsulates functionality related to managing the game's visual representation, including camera handling, rendering, and resolution management. Additionally, it integrates with other systems such as audio and spatial hashing to provide a comprehensive game experience.
    /// </summary>
    public abstract class GameLayer : Layer
    {
        public Vector2 WorldMousePosition { get; private set; }
        public Matrix ViewTransformationMatrix { get; private set; }
        public readonly SpatialHashing SpatialHashing;
        public readonly Camera2D Camera2D;
        public readonly Camera2DShaker CameraShaker;
        public readonly GameState GameState;

        protected readonly StereoSoundSystem mStereoSoundSystem = new();
        protected readonly ParticleManager mParticleManager = new();

        public GameLayer(GameState gameState, SpatialHashing spatialHashing, Game1 game1)
            : base(game1, false)
        {
            SpatialHashing = spatialHashing;
            Camera2D = new();
            CameraShaker = new();
            GameState = gameState;
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
            CameraShaker.Update(Camera2D, gameTime);
            ViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera2D.Position, Camera2D.Zoom, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            Camera2D.ApplyResolution(ResolutionManager.Resolution, this, ViewTransformationMatrix);
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
        }

        public override void ApplyResolution()
        {
            base.ApplyResolution();
            ViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera2D.Position, Camera2D.Zoom, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            Camera2D.ApplyResolution(ResolutionManager.Resolution, this, ViewTransformationMatrix);
        }

        public override void Destroy()
        {
            base.Destroy();
            mParticleManager.Clear();
        }
    }
}