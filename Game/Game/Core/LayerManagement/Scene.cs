// Scene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using StellarLiberation.Core.GameEngine.Position_Management;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Layers;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public readonly SpatialHashing<GameObject> SpatialHashing;
        public readonly ViewFrustumFilter ViewFrustumFilter;
        public readonly Camera2D Camera2D;
        public readonly GameLayer GameLayer;
        private Matrix mViewTransformationMatrix;
        private HashSet<GameObject> mVisibleObjects = new();

        public Scene(GameLayer gameLayer, int spatialHashingCellSize, float minCamZoom, float maxCamZoom, bool moveCamByMouse, float RelWidth = 1, float RelHeight = 1)
        {
            SpatialHashing = new(spatialHashingCellSize);
            ViewFrustumFilter = new();
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

            mViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera2D.Position, Camera2D.Zoom, 0, screenWidth, screenHeight);
            WorldMousePosition = Transformations.ScreenToWorld(mViewTransformationMatrix, Geometry.GetRelativePosition(inputState.mMousePosition, RenderRectangle.ToRectangle()));
            UpdateObj(gameTime, inputState);
            Camera2D.Update(gameTime, inputState, inputState.mMousePosition, mViewTransformationMatrix);
            ViewFrustumFilter.Update(screenWidth, screenHeight, mViewTransformationMatrix);
            GetObjectsOnScreen();
        }

        public abstract void UpdateObj(GameTime gameTime, InputState inputState);

        public void UpdateRenderTarget(SceneManagerLayer sceneManagerLayer, SpriteBatch spriteBatch)
        {
            // Set the render target
            mGraphicsDevice.SetRenderTarget(RenderTarget);
            mGraphicsDevice.Clear(Color.Black);

            // Drawing to the RenderTarget
            spriteBatch.Begin();
            DrawOnScreen();
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: mViewTransformationMatrix, samplerState: SamplerState.PointClamp);
            foreach (var obj in mVisibleObjects) obj.Draw(this);
            DrawOnWorld();
            sceneManagerLayer.DebugSystem.DrawOnScene(this);
            spriteBatch.End();
        }


        public abstract void DrawOnScreen();
        public abstract void DrawOnWorld();

        public virtual void OnResolutionChanged() 
        {
            var dim = new Vector2(mGraphicsDevice.Viewport.Width * mRelWidth, mGraphicsDevice.Viewport.Height * mRelHeight);
            RenderRectangle = new(mGraphicsDevice.Viewport.Bounds.Center.ToVector2() - dim / 2, dim);
            RenderTarget.Dispose();
            RenderTarget = new(mGraphicsDevice, (int)RenderRectangle.Width, (int)RenderRectangle.Height, true, SurfaceFormat.Color, DepthFormat.Depth24);
        }

        public List<T> GetObjectsInRadius<T>(Vector2 position, int radius) where T : GameObject
        {
            int CellSize = SpatialHashing.CellSize;

            // Determine the range of bucket indices that fall within the radius.
            var startX = (int)Math.Floor((position.X - radius) / CellSize);
            var endX = (int)Math.Ceiling((position.X + radius) / CellSize);
            var startY = (int)Math.Floor((position.Y - radius) / CellSize);
            var endY = (int)Math.Ceiling((position.Y + radius) / CellSize);

            List<T> objectsInRadius = new List<T>();

            foreach (var x in Enumerable.Range(startX, endX - startX + 1))
            {
                foreach (var y in Enumerable.Range(startY, endY - startY + 1))
                {
                    var objectsInBucket = SpatialHashing.GetObjectsInBucket(x * CellSize, y * CellSize);
                    foreach (var gameObject in objectsInBucket.OfType<T>())
                    {
                        var objPosition = gameObject.Position;
                        var distance = Vector2.Distance(position, objPosition);
                        if (distance <= radius) objectsInRadius.Add(gameObject);
                    }
                }
            }

            // Sort the objects by distance to the specified position
            objectsInRadius.Sort((obj1, obj2) =>
            {
                var distance1 = Vector2.DistanceSquared(position, obj1.Position);
                var distance2 = Vector2.DistanceSquared(position, obj2.Position);
                return distance1.CompareTo(distance2);
            });

            return objectsInRadius;
        }

        public void GetObjectsOnScreen()
        {
            mVisibleObjects.Clear();
            Rectangle space = ViewFrustumFilter.WorldFrustum.ToRectangle();
            int cellSize = SpatialHashing.CellSize;

            for (int x = space.X - cellSize; x <= space.Right + cellSize; x += cellSize)
            {
                for (int y = space.Y - cellSize; y <= space.Bottom + cellSize; y += cellSize)
                {
                    var objs = SpatialHashing.GetObjectsInBucket(x, y);
                    foreach (var obj in objs) mVisibleObjects.Add(obj);
                }
            }
        }
    }
}