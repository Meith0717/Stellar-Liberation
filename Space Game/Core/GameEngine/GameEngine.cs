using Galaxy_Explovive.Core.GameEngine.GameObjects;
using Galaxy_Explovive.Core.GameEngine.InputManagement;
using Galaxy_Explovive.Core.GameEngine.Rendering;
using Galaxy_Explovive.Core.GameEngine.Utility;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.UserInterface.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.GameEngine
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
            mViewTransformationMatrix = MyUtility.GetViewTransforationMatrix(Camera.Position, Camera.Zoom, screenWidth, screenHeight);
            ViewMousePosition = input.mMousePosition.ToVector2();
            WorldMousePosition = MyUtility.ScreenToWorldProjection(mViewTransformationMatrix, ViewMousePosition);

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

        public void DrawGameObject<T>(TextureManager textureManager, T obj) where T : GameObjects.GameObject
        {
            if (obj.BoundedBox.Radius == 0) throw new System.Exception($"BoundedBox Radius is Zero {obj}");
            if (FrustumCuller.CircleOnWorldView(obj.BoundedBox))
            {
                obj.Draw(textureManager, this);
            }
        }

        public void DrawGameObjects<T>(TextureManager textureManager, List<T> objects) where T : GameObjects.GameObject
        {
            foreach (T obj in objects)
            {
                if (obj.BoundedBox.Radius == 0) throw new System.Exception($"BoundedBox Radius is Zero {obj}");
                if (FrustumCuller.CircleOnWorldView(obj.BoundedBox))
                {
                    obj.Draw(textureManager, this);
                }
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
            DebugSystem.TestSpatialHashing(textureManager, SpatialHashing, WorldMousePosition);
            mSelectObjCrossHair.Draw(textureManager, this);
        }

        public void DrawObjectsOnScreen(TextureManager textureManager)
        {
            var objects = SpatialHashing.GetObjectsInSpace(FrustumCuller.WorldFrustum.ToRectangle());
            foreach (GameObjects.GameObject obj in objects)
            {
                DrawGameObject(textureManager, obj);
            }
        }

#pragma warning disable CA1822 // Mark members as static
        public void EndWorldDrawing(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
        }
#pragma warning restore CA1822 // Mark members as static
    }
}
