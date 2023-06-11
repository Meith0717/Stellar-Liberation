﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.Effects;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Game.GameObjects;
using System.Collections.Generic;
using Galaxy_Explovive.Core.Map;
using Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Core.Rendering;
using Galaxy_Explovive.Menue.Layers;
using Galaxy_Explovive.Core.Debug;
using Galaxy_Explovive.Core.UserInterface.Messages;

namespace Galaxy_Explovive.Game.Layers
{
    public class GameLayer : Layer
    {
        const int mapWidth = 3000000;
        const int mapHeight = 2000000;

        // Public GameObject Classes
        public float GameTime { get; set; } = 0;
        public InteractiveObject SelectObject { get; set; }
        public SpatialHashing<GameObject> mSpatialHashing;
        public FrustumCuller mFrustumCuller;
        public Camera2d mCamera;
        public DebugSystem mDebugSystem;
        public Vector2 mWorldMousePosition;
        public MyUiMessageManager mMessageManager;

        // Recources
        public double mAlloys;
        public double mEnergy;
        public double mCrystals;

        // Local Private Classes
        private List<Cargo> mShips = new();
        private List<PlanetSystem> mPlanetSystemList = new();
        private RectangleF mMapSize = new(mapWidth, mapHeight, mapWidth * 2, mapHeight * 2);
        private PlanetSystem mHomeSystem;
        private ParllaxManager mParllaxManager;

        // Layer Stuff _____________________________________
        public GameLayer(Game1 game)
            : base(game)
        {
            mCamera = new(mGraphicsDevice);
            mDebugSystem = new();
            mSpatialHashing = new SpatialHashing<GameObject>(10000);
            mFrustumCuller = new();
            mPlanetSystemList = Map.Generate(this, 25000, new Vector2(mapWidth, mapHeight));
            mHomeSystem = MyUtility.GetRandomElement(mPlanetSystemList);
            mCamera.TargetPosition = mHomeSystem.Position;
            mCamera.SetZoom(0.25f);
            mParllaxManager = new();
            mParllaxManager.Add(new("gameBackground", 0.1f));
            mParllaxManager.Add(new("gameBackgroundParlax2", 0.25f));
            mParllaxManager.Add(new("gameBackgroundParlax1", 0.5f));
            mShips.Add(new(this, MyUtility.GetRandomVector2(mHomeSystem.Position, 1000)));
            mShips.Add(new(this, MyUtility.GetRandomVector2(mHomeSystem.Position, 1000)));
            OnResolutionChanged();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mWorldMousePosition = mCamera.ViewToWorld(inputState.mMousePosition.ToVector2());
            mFrustumCuller.Update(mGraphicsDevice, mCamera.ViewToWorld);
            mTextureManager.SetCamZoom(mCamera.Zoom);
            if (inputState.mMouseActionType != MouseActionType.None &&
                !mFrustumCuller.IsVectorOnScreenView(inputState.mMousePosition.ToVector2()))
            {
                mLayerManager.AddLayer(new PauseLayer(mGame));
                return;
            }
            GameTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            UpdateSystems(gameTime, inputState);
            foreach (Cargo c in mShips)
            {
                c.UpdateLogik(gameTime, inputState);
            }
            if (inputState.mActionList.Contains(ActionType.ToggleRayTracing))
            {
                Globals.mRayTracing = !Globals.mRayTracing;
            }
            if (SelectObject != null) { SelectObject.UpdateInputs(inputState); }
            mParllaxManager.Update(mCamera.Movement, mCamera.Zoom);
            mDebugSystem.Update(gameTime, inputState);
            mCamera.Update(gameTime, inputState);
            System.Diagnostics.Debug.WriteLineIf(SelectObject != null, SelectObject);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mDebugSystem.UpdateFrameCounting();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw(mTextureManager);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack,
            transformMatrix: mCamera.GetViewTransformationMatrix(),
            samplerState: SamplerState.PointClamp);
            DrawSystems();
            Map.DrawGrid(mMapSize.ToRectangle(), mTextureManager);
            foreach (Cargo c in mShips)
            {
                c.Draw();
            }
            mDebugSystem.TestSpatialHashing(mTextureManager, mSpatialHashing, mWorldMousePosition);
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mParllaxManager.OnResolutionChanged(mGraphicsDevice);
        }

        public override void Destroy() { }

        // Update Stuff _____________________________________
        private void UpdateSystems(GameTime gameTime, InputState inputState)
        {
            foreach (PlanetSystem planetSystem in mPlanetSystemList)
            {
                planetSystem.UpdateLogik(gameTime, inputState);
            }
        }

        // Draw Stuff _____________________________________
        private void DrawSystems()
        {
            var counter = 0;
            foreach (PlanetSystem planetSystem in mPlanetSystemList)
            {
                planetSystem.Draw();
                counter++;
            }
        }
    }
}