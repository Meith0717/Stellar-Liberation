using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Core.GameEngine.GameObjects;
using CelestialOdyssey.Core.GameEngine.InputManagement;
using CelestialOdyssey.Core.GameEngine.Position_Management;
using CelestialOdyssey.Core.GameEngine.Utility;
using CelestialOdyssey.Core.GameObject;
using CelestialOdyssey.Core.UserInterface.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;

namespace CelestialOdyssey.Core.GameEngine
{
    public class GameEngine
    {
        public Camera Camera { get; private set; } = new();
        public SpatialHashing<GameObjects.GameObject> SpatialHashing { get; private set; } = new(1000);
        public FrustumCuller FrustumCuller { get; private set; } = new();
        public DebugSystem DebugSystem { get; private set; } = new();

        public float GameTime { get; private set; } = 0;
        public Vector2 WorldMousePosition { get; private set; } = Vector2.Zero;
        public Vector2 ViewMousePosition { get; private set; } = Vector2.Zero;
        public InteractiveObject SelectObject { get; set; } = null;

        public SoundManager SoundManager { get; private set; }
        public MyUiMessageManager MessageManager { get; private set; }

        public Matrix ViewTransformationMatrix { get; private set; }
        public List<GameObjects.GameObject> ObjectsOnScreen { get; private set; }

        public GameEngine(SoundManager soundManager, MyUiMessageManager messageManager)
        {
            SoundManager = soundManager;
            MessageManager = messageManager;
        }

        public void UpdateEngine(GameTime time, InputState input, GraphicsDevice graphicsDevice)
        {
            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHeight = graphicsDevice.Viewport.Height;

            DebugSystem.Update(time, input);

            GameTime += time.ElapsedGameTime.Milliseconds;
            ViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera.Position, Camera.Zoom, screenWidth, screenHeight);
            ViewMousePosition = input.mMousePosition.ToVector2();
            WorldMousePosition = Transformations.ScreenToWorld(ViewTransformationMatrix, ViewMousePosition);

            Camera.Update(time, input, ViewMousePosition, ViewTransformationMatrix);
            FrustumCuller.Update(screenWidth, screenHeight, ViewTransformationMatrix);
            ObjectsOnScreen = SpatialHashing.GetObjectsInSpace(FrustumCuller.WorldFrustum.ToRectangle());
        }

        public void UpdateGameObject<T>(GameTime time, InputState input, T obj) where T : GameObjects.GameObject
        {
            obj.Update(time, input, this);
        }

        public void UpdateGameObjects<T>(GameTime time, InputState input, List<T> objects) where T : GameObjects.GameObject
        {
            foreach (T obj in objects)
            {
                obj.Update(time, input, this);
            }
        }

        public void BeginWorldDrawing(SpriteBatch spriteBatch, TextureManager textureManager)
        {
            DebugSystem.UpdateFrameCounting();

            spriteBatch.Begin();
            DebugSystem.ShowRenderInfo(textureManager, Camera.Zoom, Camera.Position);
            spriteBatch.End();

            spriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                transformMatrix: ViewTransformationMatrix,
                samplerState: SamplerState.PointClamp
            );
        }

        public void RenderWorldObjectsOnScreen(TextureManager textureManager)
        {
            Rendering.DrawGameObjects(textureManager, this, ObjectsOnScreen);
        }

        public void EndWorldDrawing(SpriteBatch spriteBatch, TextureManager textureManager)
        {
            DebugSystem.TestSpatialHashing(textureManager, this);
            spriteBatch.End();
        }
    }
}
