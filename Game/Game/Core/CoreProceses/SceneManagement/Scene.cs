// Scene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParticleSystem;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.Layers;

namespace StellarLiberation.Game.Core.CoreProceses.SceneManagement
{
    public abstract class Scene
    {
        private GraphicsDevice mGraphicsDevice;

        public Vector2 WorldMousePosition { get; private set; }
        public readonly SpatialHashing<GameObject2D> SpatialHashing;
        public readonly ParticleManager ParticleManager;
        public readonly NewCamera2D Camera2D;
        public readonly GameLayer GameLayer;
        private Matrix mViewTransformationMatrix;

        public Scene(GameLayer gameLayer, int spatialHashingCellSize, float minCamZoom, float maxCamZoom, bool moveCamByMouse, float RelWidth = 1, float RelHeight = 1)
        {
            SpatialHashing = new(spatialHashingCellSize);
            ParticleManager = new();
            Camera2D = new();
            GameLayer = gameLayer;
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            mGraphicsDevice = graphicsDevice;
        }

        public void Update(GameTime gameTime, InputState inputState)
        {
            UpdateObj(gameTime, inputState);
            ParticleManager.Update(gameTime, inputState, this);
            Camera2D.Update(mGraphicsDevice, this);
            mViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera2D.Position, Camera2D.Zoom, 0, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            WorldMousePosition = Transformations.ScreenToWorld(mViewTransformationMatrix, inputState.mMousePosition);
        }

        public abstract void UpdateObj(GameTime gameTime, InputState inputState);

        public void Draw(SceneManagerLayer sceneManagerLayer, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            DrawOnScreenView(sceneManagerLayer, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: mViewTransformationMatrix, samplerState: SamplerState.PointClamp);
            Camera2D.Draw(this);
            sceneManagerLayer.DebugSystem.DrawOnScene(this);
            spriteBatch.End();
        }

        public virtual void DrawOnScreenView(SceneManagerLayer sceneManagerLayer, SpriteBatch spriteBatch) { }

        public virtual void OnResolutionChanged()
        {
        }

    }
}