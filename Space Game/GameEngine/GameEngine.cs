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
        public Camera Camera { get; private set; } = new();
        public SpatialHashing<GameObject> SpatialHashing { get; private set; } = new(1000);
        public FrustumCuller FrustumCuller { get; private set; } = new();
        public DebugSystem DebugSystem { get; private set; } = new();

        public float GameTime { get; private set; } = 0;
        public Vector2 WorldMousePosition { get; private set; } = Vector2.Zero;
        public Vector2 ViewMousePosition { get; private set; } = Vector2.Zero;

        public MyUiMessageManager MessageManager { get; private set; }

        public Matrix ViewTransformationMatrix { get; private set; }
        public List<GameObject> ObjectsOnScreen { get; private set; }

        public GameEngine(MyUiMessageManager messageManager)
        {
            MessageManager = messageManager;
        }

        public void UpdateEngine(GameTime time, InputState input, GraphicsDevice graphicsDevice)
        {
            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHeight = graphicsDevice.Viewport.Height;

            DebugSystem.Update(time, input);

            GameTime += time.ElapsedGameTime.Milliseconds;
            ViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera.Position, Camera.Zoom, screenWidth, screenHeight);
            ViewMousePosition = input.mMousePosition;
            WorldMousePosition = Transformations.ScreenToWorld(ViewTransformationMatrix, ViewMousePosition);

            Camera.Update(time, input, ViewMousePosition, ViewTransformationMatrix);
            FrustumCuller.Update(screenWidth, screenHeight, ViewTransformationMatrix);
            ObjectsOnScreen = SpatialHashing.GetObjectsInSpace(FrustumCuller.WorldFrustum.ToRectangle());
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

        public List<T> GetObjectsInRadius<T>(Vector2 positionVector2, int radius)
        {
            var objectsInRadius = new List<GameObject>();
            int CellSize = SpatialHashing.mCellSize;
            var maxRadius = radius + CellSize;
            for (var i = -radius; i <= maxRadius; i += CellSize)
            {
                for (var j = -radius; j <= maxRadius; j += CellSize)
                {
                    var objectsInBucket = SpatialHashing.GetObjectsInBucket((int)(positionVector2.X + i), (int)(positionVector2.Y + j));
                    foreach (var gameObject in objectsInBucket)
                    {
                        var position = gameObject.Position;
                        var distance = Vector2.Distance(positionVector2, position);
                        if (distance <= radius)
                        {
                            objectsInRadius.Add(gameObject);
                        }
                    }
                }
            }
            Comparison<GameObject> comparison = (a, b) => Vector2.Distance(a.Position, positionVector2).CompareTo(Vector2.Distance(b.Position, positionVector2));
            objectsInRadius.Sort(comparison);
            return objectsInRadius.OfType<T>().ToList();
        }
    }
}
