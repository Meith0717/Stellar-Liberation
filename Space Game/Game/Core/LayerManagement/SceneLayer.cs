using CelestialOdyssey.Core.GameEngine.Position_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.LayerManagement
{
    public abstract class SceneLayer : Layer
    {
        public Vector2 WorldMousePosition { get; private set; }
        
        private Matrix mViewTransformationMatrix;
        public readonly SpatialHashing<GameObject> SpatialHashing;
        public readonly FrustumCuller FrustumCuller;
        public readonly Camera Camera;
        public readonly DebugSystem DebugSystem;

        public SceneLayer(int spatialHashingCellSize, bool updateBelow, float minCamZoom, float maxCamZoom, bool moveCamByMouse) : base(updateBelow) 
        {
            SpatialHashing = new(spatialHashingCellSize);
            FrustumCuller = new();
            Camera = new(minCamZoom, maxCamZoom, moveCamByMouse);
            DebugSystem = new();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            int screenWidth = mGraphicsDevice.Viewport.Width;
            int screenHeight = mGraphicsDevice.Viewport.Height;

            DebugSystem.Update(gameTime, inputState);
            mViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera.Position, Camera.Zoom, screenWidth, screenHeight);
            WorldMousePosition = Transformations.ScreenToWorld(mViewTransformationMatrix, inputState.mMousePosition);
            Camera.Update(gameTime, inputState, inputState.mMousePosition, mViewTransformationMatrix);
            FrustumCuller.Update(screenWidth, screenHeight, mViewTransformationMatrix);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DebugSystem.UpdateFrameCounting();

            spriteBatch.Begin();
            DebugSystem.ShowRenderInfo(Camera.Zoom, Camera.Position);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: mViewTransformationMatrix, samplerState: SamplerState.PointClamp );
            DebugSystem.TestSpatialHashing(this);
            RenderWorldObjectsOnScreen();
            DrawOnScene();
            spriteBatch.End();
        }

        public abstract void DrawOnScene();

        public List<T> GetObjectsInRadius<T>(Vector2 position, int radius) where T : GameObject
        {
            var objectsInRadius = new List<GameObject>();
            int CellSize = SpatialHashing.mCellSize;

            // Determine the range of bucket indices that fall within the radius.
            var startX = (int)Math.Floor((position.X - radius) / CellSize);
            var endX = (int)Math.Ceiling((position.X + radius) / CellSize);
            var startY = (int)Math.Floor((position.Y - radius) / CellSize);
            var endY = (int)Math.Ceiling((position.Y + radius) / CellSize);

            for (var x = startX; x <= endX; x++)
            {
                for (var y = startY; y <= endY; y++)
                {
                    var objectsInBucket = SpatialHashing.GetObjectsInBucket(x * CellSize, y * CellSize);
                    foreach (var gameObject in objectsInBucket)
                    {
                        var objPosition = gameObject.Position;
                        var distance = Vector2.Distance(position, objPosition);
                        if (distance <= radius)
                        {
                            objectsInRadius.Add(gameObject);
                        }
                    }
                }
            }
            return objectsInRadius.OfType<T>().ToList();
        }

        public List<T> GetSortedObjectsInRadius<T>(Vector2 position, int radius) where T : GameObject
        {
            var objectsInRadius = GetObjectsInRadius<T>(position, radius);
            Comparison<GameObject> comparison = (a, b) => Vector2.Distance(a.Position, position).CompareTo(Vector2.Distance(b.Position, position));
            objectsInRadius.Sort(comparison);
            return objectsInRadius;
        }

        public void RenderWorldObjectsOnScreen()
        {
            Rectangle space = FrustumCuller.WorldFrustum.ToRectangle();
            int cellSize = SpatialHashing.mCellSize;
            var screeenMaxX = space.X + space.Width + cellSize;
            var screenMaxY = space.Y + space.Height + cellSize;

            for (int x = space.X - cellSize; x <= screeenMaxX; x += cellSize)
            {
                for (int y = space.Y - cellSize; y <= screenMaxY; y += cellSize)
                {
                    var objs = SpatialHashing.GetObjectsInBucket(x, y);
                    foreach (var obj in objs)
                    {
                        if (obj.BoundedBox.Radius == 0) throw new System.Exception($"BoundedBox Radius is Zero {obj}");
                        if (this.FrustumCuller.CircleOnWorldView(obj.BoundedBox))
                        {
                            obj.Draw(this);
                        }
                    }
                }
            }
        }
    }
}
