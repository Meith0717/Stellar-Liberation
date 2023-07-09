using GalaxyExplovive.Core.GameEngine.Content_Management;
using GalaxyExplovive.Core.GameEngine.GameObjects;
using GalaxyExplovive.Core.GameEngine.InputManagement;
using GalaxyExplovive.Core.GameEngine.Position_Management;
using GalaxyExplovive.Core.GameEngine.Utility;
using GalaxyExplovive.Core.GameObject;
using GalaxyExplovive.Core.UserInterface.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GalaxyExplovive.Core.GameEngine
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

        private readonly CrossHair mSelectObjCrossHair;
        private Matrix mViewTransformationMatrix;

        public GameEngine(SoundManager soundManager, MyUiMessageManager messageManager)
        {
            SoundManager = soundManager;
            MessageManager = messageManager;
            mSelectObjCrossHair = new(CrossHair.CrossHairType.Select);
        }

        public void UpdateEngine(GameTime time, InputState input, GraphicsDevice graphicsDevice)
        {
            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHeight = graphicsDevice.Viewport.Height;

            DebugSystem.Update(time, input);

            GameTime += time.ElapsedGameTime.Milliseconds;
            mViewTransformationMatrix = Transformations.CreateViewTransformationMatrix(Camera.Position, Camera.Zoom, screenWidth, screenHeight);
            ViewMousePosition = input.mMousePosition.ToVector2();
            WorldMousePosition = Transformations.ScreenToWorld(mViewTransformationMatrix, ViewMousePosition);

            Camera.Update(time, input, WorldMousePosition);
            FrustumCuller.Update(screenWidth, screenHeight, mViewTransformationMatrix);

            if (SelectObject == null)
            {
                mSelectObjCrossHair.Update(null, 0, Color.Transparent, false);
                return;
            }
            SelectObject.SelectActions(input, this);
            mSelectObjCrossHair.Update(SelectObject.Position, SelectObject.TextureScale * 20, Color.OrangeRed, false);
        }

        public void UpdateGameObject<T>(GameTime time, InputState input, T obj) where T : GameObjects.GameObject
        {
            obj.UpdateLogic(time, input, this);
        }

        public void UpdateGameObjects<T>(GameTime time, InputState input, List<T> objects) where T : GameObjects.GameObject
        {
            foreach (T obj in objects)
            {
                obj.UpdateLogic(time, input, this);
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
                transformMatrix: mViewTransformationMatrix,
                samplerState: SamplerState.PointClamp
            );

            DebugSystem.TestSpatialHashing(textureManager, this);
            Rendering.RenderObjectsOnScreen(textureManager, this);
            mSelectObjCrossHair.Draw(textureManager, this);
        }

#pragma warning disable CA1822 // Mark members as static
        public void EndWorldDrawing(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
        }
#pragma warning restore CA1822 // Mark members as static
    }
}
