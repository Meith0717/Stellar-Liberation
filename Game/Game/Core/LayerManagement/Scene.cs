// Scene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using StellarLiberation.Core.GameEngine.Position_Management;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.Rendering;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Layers;

namespace StellarLiberation.Game.Core.LayerManagement
{
    public abstract class Scene
    {
        public RenderTarget2D RenderTarget { get; private set; }
        public RectangleF RenderRectangle { get; private set; }
        private GraphicsDevice mGraphicsDevice;
        private float mRelHeight;
        private float mRelWidth;

        public Vector2 WorldMousePosition { get; private set; }
        public readonly SpatialHashing<GameObject2D> SpatialHashing;
        public readonly RenderPipeline<GameObject2D> RenderPipeline;
        public readonly ViewFrustumFilter ViewFrustumFilter;
        public readonly Camera2D Camera2D;
        public readonly GameLayer GameLayer;
        private Matrix mViewTransformationMatrix;

        public Scene(GameLayer gameLayer, int spatialHashingCellSize, float minCamZoom, float maxCamZoom, bool moveCamByMouse, float RelWidth = 1, float RelHeight = 1)
        {
            SpatialHashing = new(spatialHashingCellSize);
            ViewFrustumFilter = new();
            RenderPipeline = new(ViewFrustumFilter, SpatialHashing);
            Camera2D = new(minCamZoom, maxCamZoom, moveCamByMouse);
            GameLayer = gameLayer;
            mRelHeight = RelHeight;
            mRelWidth = RelWidth;
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            mGraphicsDevice = graphicsDevice;
            var dim = new Vector2(mGraphicsDevice.Viewport.Width * mRelWidth, mGraphicsDevice.Viewport.Height * mRelHeight);
            RenderRectangle = new(mGraphicsDevice.Viewport.Bounds.Center.ToVector2() - dim / 2, dim);
            RenderTarget = new(mGraphicsDevice, (int)RenderRectangle.Width, (int)RenderRectangle.Height, true, SurfaceFormat.Color, DepthFormat.Depth24);
        }

        public void Update(GameTime gameTime, InputState inputState)
        {
            var screenWidth = (int)RenderRectangle.Width;
            var screenHeight = (int)RenderRectangle.Height;

            UpdateObj(gameTime, inputState);
            Camera2D.Update(gameTime, inputState, inputState.mMousePosition, mViewTransformationMatrix);
            mViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera2D.Position, Camera2D.Zoom, 0, screenWidth, screenHeight);
            WorldMousePosition = Transformations.ScreenToWorld(mViewTransformationMatrix, Geometry.GetRelativePosition(inputState.mMousePosition, RenderRectangle.ToRectangle()));
            ViewFrustumFilter.Update(screenWidth, screenHeight, mViewTransformationMatrix);
            RenderPipeline.Update();
        }

        public abstract void UpdateObj(GameTime gameTime, InputState inputState);

        public void UpdateRenderTarget2D(SceneManagerLayer sceneManagerLayer, SpriteBatch spriteBatch)
        {
            // Set the render target
            mGraphicsDevice.SetRenderTarget(RenderTarget);
            mGraphicsDevice.Clear(Color.Black);

            // Drawing to the RenderTarget
            spriteBatch.Begin();
            DrawOnScreenView(sceneManagerLayer, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: mViewTransformationMatrix, samplerState: SamplerState.PointClamp);
            RenderPipeline.Render(this);
            sceneManagerLayer.DebugSystem.DrawOnScene(this);
            spriteBatch.End();
        }

        public virtual void DrawOnScreenView(SceneManagerLayer sceneManagerLayer, SpriteBatch spriteBatch) { }

        public virtual void OnResolutionChanged() 
        {
            var dim = new Vector2(mGraphicsDevice.Viewport.Width * mRelWidth, mGraphicsDevice.Viewport.Height * mRelHeight);
            RenderRectangle = new(mGraphicsDevice.Viewport.Bounds.Center.ToVector2() - dim / 2, dim);
            RenderTarget.Dispose();
            RenderTarget = new(mGraphicsDevice, (int)RenderRectangle.Width, (int)RenderRectangle.Height, true, SurfaceFormat.Color, DepthFormat.Depth24);
        }

    }
}