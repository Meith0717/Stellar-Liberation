using Galaxy_Explovive.Core.Debug;
using Galaxy_Explovive.Core.Effects;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.Map;
using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Core.Rendering;
using Galaxy_Explovive.Core.UserInterface.Messages;
using Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips;
using Galaxy_Explovive.Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.Utility;

namespace Galaxy_Explovive.Game
{
    public class Game
    {  
        // Public GameObject Classes
        public float GameTime = 0;
        public Vector2 mWorldMousePosition = Vector2.Zero;
        public Vector2 mViewMousePosition = Vector2.Zero;
        public InteractiveObject SelectObject = null;
        public MyUiMessageManager mMessageManager;
        public readonly SpatialHashing<GameObject> mSpatialHashing;
        public readonly FrustumCuller mFrustumCuller;
        public readonly Camera2d mCamera;
        public readonly DebugSystem mDebugSystem;
        public readonly SoundManager mSoundManager;
        public readonly TextureManager mTextureManager;
        public readonly GraphicsDevice mGraphicsDevice;
        public readonly Map mMap;

        // Recources
        public double mAlloys;
        public double mEnergy;
        public double mCrystals;

        // Local Private Classes
        private List<Cargo> mShips = new();
        private readonly ParllaxManager mParllaxManager;

        public Game(GraphicsDevice graphicsDevice, TextureManager textureManager, 
            MyUiMessageManager messageManager, SoundManager soundManager)
        {
            mSpatialHashing = new(1000);
            mFrustumCuller = new();
            mCamera = new(graphicsDevice);
            mDebugSystem = new();
            mMessageManager = messageManager;
            mSoundManager = soundManager;
            mTextureManager = textureManager;
            mGraphicsDevice = graphicsDevice;
            mParllaxManager = new();
            mMap = new(this, 100, 5000, 100000, 100000);
        }

        public void Initialize()
        {
            mParllaxManager.Add(new("gameBackground", 0.1f));
            mParllaxManager.Add(new("gameBackgroundParlax2", 0.25f));
            mParllaxManager.Add(new("gameBackgroundParlax1", 0.5f));
            mShips.Add(new(this, MyUtility.GetRandomVector2(Vector2.Zero, 0)));
            mMap.Generate();
        }

        public void Update(InputState inputState, GameTime gameTime) 
        {
            mViewMousePosition = inputState.mMousePosition.ToVector2();
            mWorldMousePosition = mCamera.ViewToWorld(mViewMousePosition);
            mFrustumCuller.Update(mGraphicsDevice, mCamera.ViewToWorld);
            mTextureManager.Update(mCamera.Zoom);
            GameTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;

            foreach (Cargo c in mShips) { c.UpdateLogik(gameTime, inputState); }
            if (inputState.mActionList.Contains(ActionType.ToggleRayTracing))
            {
                Globals.mRayTracing = !Globals.mRayTracing;
            }
            if (SelectObject != null) { SelectObject.SelectActions(inputState); }
            mMap.Update(gameTime, inputState);
            mParllaxManager.Update(mCamera.Movement, mCamera.Zoom);
            mDebugSystem.Update(gameTime, inputState);
            mCamera.Update(gameTime, inputState);
        }
        public void Draw(SpriteBatch spriteBatch) 
        {
            mDebugSystem.UpdateFrameCounting();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw(mTextureManager);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, 
                transformMatrix: mCamera.GetViewTransformationMatrix(), 
                samplerState: SamplerState.PointClamp);

            mMap.Draw();
            foreach (Cargo c in mShips)
            {
                c.Draw();
            }
            mDebugSystem.TestSpatialHashing(mTextureManager, mSpatialHashing, mWorldMousePosition);
            spriteBatch.End();
        }

        public void ApplyResolution()
        {
            mParllaxManager.OnResolutionChanged(mGraphicsDevice);
        }
    }
}
