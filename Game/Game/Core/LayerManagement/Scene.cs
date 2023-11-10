﻿// Scene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Core.GameEngine.Position_Management;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.Utilitys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.LayerManagement
{
    public abstract class Scene
    {
        public Vector2 WorldMousePosition { get; private set; }
        public readonly SpatialHashing<GameObject> SpatialHashing;
        public readonly FrustumCuller FrustumCuller;
        public readonly Camera Camera;
        private Matrix mViewTransformationMatrix;
        private HashSet<GameObject> mVisibleObjects = new();

        public Scene(int spatialHashingCellSize, float minCamZoom, float maxCamZoom, bool moveCamByMouse)
        {
            SpatialHashing = new(spatialHashingCellSize);
            FrustumCuller = new();
            Camera = new(minCamZoom, maxCamZoom, moveCamByMouse);
        }

        public void Update(GameTime gameTime, InputState inputState, int screenWidth, int screenHeight)
        {
            mViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera.Position, Camera.Zoom, 0, screenWidth, screenHeight);
            WorldMousePosition = Transformations.ScreenToWorld(mViewTransformationMatrix, inputState.mMousePosition);
            Camera.Update(gameTime, inputState, inputState.mMousePosition, mViewTransformationMatrix);
            FrustumCuller.Update(screenWidth, screenHeight, mViewTransformationMatrix);
            UpdateObj(gameTime, inputState);
            GetObjectsOnScreen();
        }

        public abstract void UpdateObj(GameTime gameTime, InputState inputState);

        public void Draw(SceneManagerLayer sceneManagerLayer, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            DrawOnScreen();
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: mViewTransformationMatrix, samplerState: SamplerState.PointClamp);
            foreach (var obj in mVisibleObjects) obj.Draw(sceneManagerLayer, this);
            DrawOnWorld();
            sceneManagerLayer.DebugSystem.DrawOnScene(this);
            spriteBatch.End();
        }

        public abstract void DrawOnScreen();
        public abstract void DrawOnWorld();

        public abstract void OnResolutionChanged();

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
            Rectangle space = FrustumCuller.WorldFrustum.ToRectangle();
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