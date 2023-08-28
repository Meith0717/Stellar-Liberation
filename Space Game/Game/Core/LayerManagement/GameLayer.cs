using CelestialOdyssey.Core.GameEngine.Position_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.LayerManagement
{
    [Serializable]
    public abstract class GameLayer : Layer
    {
        [JsonProperty] public HashSet<GameObject> GameObjects { get; private set; }
        
        [JsonIgnore] public Vector2 WorldMousePosition { get; private set; }
        [JsonIgnore] private Matrix ViewTransformationMatrix;
        [JsonIgnore] public readonly Camera Camera;
        [JsonIgnore] public readonly DebugSystem DebugSystem;
        [JsonIgnore] public readonly FrustumCuller FrustumCuller;
        [JsonProperty] public readonly SpatialHashing<GameObject> SpatialHashing;

        internal GameLayer() 
            : base(false)
        {
            GameObjects = new();
            SpatialHashing = new(100000);
            Camera = new();
            DebugSystem = new();
            FrustumCuller = new();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;

            DebugSystem.Update(gameTime, inputState);

            ViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera.Position, Camera.Zoom, screenWidth, screenHeight);
            WorldMousePosition = Transformations.ScreenToWorld(ViewTransformationMatrix, inputState.mMousePosition);

            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.SetGameLayer(this);
                gameObject.RemoveFromSpatialHashing();
                gameObject.Update(gameTime, inputState);
                gameObject.AddToSpatialHashing();
            }

            Camera.Update(gameTime, inputState, inputState.mMousePosition, ViewTransformationMatrix);
            FrustumCuller.Update(screenWidth, screenHeight, ViewTransformationMatrix);
        }

        public void AddObject(GameObject obj)
        {
            GameObjects.Add(obj);
        }

        public void RemoveObject(GameObject obj)
        {
            GameObjects.Remove(obj);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DebugSystem.UpdateFrameCounting();

            spriteBatch.Begin();
            DebugSystem.ShowRenderInfo(Camera.Zoom, Camera.Position);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: ViewTransformationMatrix, samplerState: SamplerState.PointClamp);
            RenderWorldObjectsOnScreen();
            DrawOnWorld();
            spriteBatch.End();
        }

        public abstract void DrawOnWorld();

        private void RenderWorldObjectsOnScreen()
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
                        if (FrustumCuller.CircleOnWorldView(obj.BoundedBox))
                        {
                            obj.Draw();
                        }
                    }
                }
            }
        }

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
    }
}
