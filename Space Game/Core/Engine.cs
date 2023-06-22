using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.GameObjects;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Core.Rendering.Renderer;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core
{
    public class Engine
    {
        private List<GameObject.GameObject> mGameObjects = new();
        private Renderer mRenderer = new();
        private SpatialHashing<GameObject.GameObject> mSpatialHashing = new(1000);

        public Camera2d mCamera2d = new();
        public SelectableObject SelectedObject;
        public Matrix ViewTransformation;
        public Vector2 MouseWorldPosition;
        public Vector2 MouseViewPosition;
        public float GameTime;

        public Engine(SoundManager soundManager, TextureManager textureManager) { }

        public void Update(GraphicsDevice graphicsDevice, GameTime gameTime, InputState inputState)
        {
            GameTime += gameTime.ElapsedGameTime.Milliseconds;
            MouseViewPosition = inputState.mMousePosition.ToVector2();
            MouseWorldPosition = ViewToWorldTransform(MouseViewPosition);

            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHight = graphicsDevice.Viewport.Height;
            ViewTransformation = mCamera2d.GetViewTransformation(screenWidth, screenHight);
            mRenderer.Update(screenWidth, screenHight, ViewTransformation);

            foreach (var obj in mGameObjects)
            {
                mSpatialHashing.RemoveObject(obj, obj.Position);
                obj.UpdateLogik(gameTime, inputState, this);
                mSpatialHashing.InsertObject(obj, obj.Position);
            }

            if (SelectedObject != null)
            {
                SelectedObject.SelectActions(inputState, this);
            }
        }

        public void Draw(SpriteBatch spriteBatch, TextureManager textureManager)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: ViewTransformation, samplerState: SamplerState.PointClamp);
            mRenderer.RenderGameObjectList(textureManager, mGameObjects);
            spriteBatch.End();
        }

        public void AddGameObject(GameObject.GameObject obj)
        {
            mGameObjects.Add(obj);
        }

        public void AddGameObjects(List<GameObject.GameObject> objects)
        {
            foreach (var obj in objects)
            {
                mGameObjects.Add(obj);
            }
        }

        public Vector2 ViewToWorldTransform(Vector2 position)
        {
            return Vector2.Transform(position, ViewTransformation);
        }
    }
}
