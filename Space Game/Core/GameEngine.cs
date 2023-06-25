using Galaxy_Explovive.Core.Debug;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Core.Rendering;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.UserInterface.Messages;
using Galaxy_Explovive.Game.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core
{
    public class GameEngine
    {
        public Camera2d Camera { get; private set; } = new();
        public SpatialHashing<GameObject.GameObject> SpatialHashing { get; private set; } = new(1000);
        public FrustumCuller FrustumCuller { get; private set; } = new();
        public DebugSystem DebugSystem { get; private set; } = new();

        public float GameTime { get; private set; } = 0;
        public Vector2 WorldMousePosition { get; private set; } = Vector2.Zero;
        public Vector2 ViewMousePosition { get; private set; } = Vector2.Zero;
        public InteractiveObject SelectObject { get; set; } = null;

        public SoundManager SoundManager { get; private set; }
        public MyUiMessageManager MessageManager { get; private set; }

        private Matrix mViewTransformationMatrix;

        public GameEngine(SoundManager soundManager, MyUiMessageManager messageManager)
        {
            SoundManager = soundManager;
            MessageManager = messageManager;
        }

        public void UpdateEngine(GameTime time, InputState input, GraphicsDevice graphicsDevice)
        {
            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHeight = graphicsDevice.Viewport.Height;

            Camera.Update(time, input);
            DebugSystem.Update(time, input);

            GameTime += time.ElapsedGameTime.Milliseconds;
            mViewTransformationMatrix = Camera.GetViewTransformationMatrix(screenWidth, screenHeight);
            ViewMousePosition = input.mMousePosition.ToVector2();
            WorldMousePosition = Vector2.Transform(ViewMousePosition, Matrix.Invert(mViewTransformationMatrix));

            FrustumCuller.Update(screenWidth, screenHeight, mViewTransformationMatrix);

            if (SelectObject == null) { return; }
            SelectObject.SelectActions(input, this);
        }

        public void UpdateGameObjects<T>(GameTime time, InputState input, List<T> objects) where T : GameObject.GameObject
        {
             foreach(T obj in objects)
            {
                SpatialHashing.RemoveObject(obj, (int)obj.Position.X, (int)obj.Position.Y);
                obj.UpdateLogik(time, input, this);
                if (FrustumCuller.CircleOnWorldView(obj.BoundedBox))
                {
                    SpatialHashing.InsertObject(obj, (int)obj.Position.X, (int)obj.Position.Y);
                }
            }
        }

        public void DrawGameObjects<T>(TextureManager textureManager, List<T> objects) where T : GameObject.GameObject
        { 
            foreach (T obj in objects)
            {
                if (FrustumCuller.CircleOnWorldView(obj.BoundedBox))
                {
                    obj.Draw(textureManager, this);
                }
            }
        }

        public void BeginWorldDrawing(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                transformMatrix:mViewTransformationMatrix,
                samplerState: SamplerState.PointClamp
            );
        }
    }
}
