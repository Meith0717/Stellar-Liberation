using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Core.GameEngine.Position_Management;
using CelestialOdyssey.Game.Core.UserInterface.Messages;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.GameEngine
{
    public class GameEngine
    {
        public float ActiveGameTime { get; private set; } = 0;
        public SpatialHashing<GameObject> SpatialHashing { get; private set; } = new(10000);
        public FrustumCuller FrustumCuller { get; private set; } = new();
        public Camera Camera { get; private set; } = new();
        public DebugSystem DebugSystem { get; private set; } = new();
        public Vector2 WorldMousePosition { get; private set; } = Vector2.Zero;
        public Vector2 ViewMousePosition { get; private set; } = Vector2.Zero;
        public Matrix ViewTransformationMatrix { get; private set; }
        public HashSet<GameObject> ObjectsOnScreen { get; private set; } = new();

        public void UpdateEngine(GameTime time, InputState input, GraphicsDevice graphicsDevice)
        {
            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHeight = graphicsDevice.Viewport.Height;

            DebugSystem.Update(time, input);

            ActiveGameTime += time.ElapsedGameTime.Milliseconds;
            ViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera.Position, Camera.Zoom, screenWidth, screenHeight);
            ViewMousePosition = input.mMousePosition;
            WorldMousePosition = Transformations.ScreenToWorld(ViewTransformationMatrix, ViewMousePosition);

            Camera.Update(time, input, ViewMousePosition, ViewTransformationMatrix);
            FrustumCuller.Update(screenWidth, screenHeight, ViewTransformationMatrix);
            ObjectsOnScreen = SpatialHashing.GetObjectsInSpace(FrustumCuller.WorldFrustum.ToRectangle());
            System.Diagnostics.Debug.WriteLine(SpatialHashing.ToString());
        }

        public void UpdateGameObject<T>(GameTime time, InputState input, T obj) where T : GameObject
        {
            obj.Update(time, input, this);
        }

        public void UpdateGameObjects<T>(GameTime time, InputState input, List<T> objects) where T : GameObject
        {
            foreach (T obj in objects)
            {
                obj.Update(time, input, this);
            }
        }

        public void BeginWorldDrawing(SpriteBatch spriteBatch)
        {
            DebugSystem.UpdateFrameCounting();

            spriteBatch.Begin();
            DebugSystem.ShowRenderInfo(Camera.Zoom, Camera.Position);
            spriteBatch.End();

            spriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                transformMatrix: ViewTransformationMatrix,
                samplerState: SamplerState.PointClamp
            );
        }

        public void RenderWorldObjectsOnScreen()
        {
            Rendering.DrawGameObjects(this, ObjectsOnScreen);
        }

        public void EndWorldDrawing(SpriteBatch spriteBatch)
        {
            DebugSystem.TestSpatialHashing(this);
            spriteBatch.End();
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
