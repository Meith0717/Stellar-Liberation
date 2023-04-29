using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.Debug;
using Galaxy_Explovive.Core.Effects;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.Menu;
using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Core.Rendering;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Game.GameObjects;
using System;
using System.Collections.Generic;
using Galaxy_Explovive.Game.GameObjects.Spacecraft;
using System.Diagnostics;
using Galaxy_Explovive.Core.Map;

namespace Galaxy_Explovive.Game.Layers
{
    [Serializable]
    public class GameLayer : Layer
    {
        const int mapWidth = 4000000;
        const int mapHeight = 2000000;

        [JsonIgnore] public RectangleF mMapSize = new RectangleF(new Vector2(mapWidth, mapHeight), new Vector2(mapWidth, mapHeight) * 2);

        [JsonProperty] public double mPassedSeconds = 0;
        [JsonProperty] private PlanetSystem mHomeSystem;
        [JsonProperty] private List<PlanetSystem> mPlanetSystemList = new List<PlanetSystem>();

        // Recources
        [JsonProperty] public double mAlloys;
        [JsonProperty] public double mEnergy;
        [JsonProperty] public double mCrystals;

        [JsonIgnore] public SpatialHashing<GameObject> mSpatialHashing;

        private UiElementSprite mBackground;
        private FrustumCuller mFrustumCuller;
        private ParllaxManager mParllaxManager;
        private Cargo mShipTest;

        // Layer Stuff _____________________________________
        public GameLayer() : base()
        {
            InitializeGlobals();
            mBackground = new UiElementSprite("gameBackground");
            mBackground.mSpriteFit = UiElementSprite.SpriteFit.Cover;
            mSpatialHashing = new SpatialHashing<GameObject>(2000);
            mFrustumCuller = new FrustumCuller();
            SpawnSystemsAndGetHome();
            mParllaxManager = new ParllaxManager();
            OnResolutionChanged();
            Globals.mTimeWarp = 1;
            InitializeParllax();
            mShipTest = new Cargo(mHomeSystem.Position);
        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            mPassedSeconds += gameTime.ElapsedGameTime.Milliseconds / 1000d;
            mParllaxManager.Update();
            mFrustumCuller.Update();
            Globals.mDebugSystem.Update(gameTime, inputState);
            Globals.mCamera2d.Update(gameTime, inputState);
            UpdateSystems(gameTime, inputState);
            ManageTimeWarp(gameTime, inputState);
            mShipTest.Update(gameTime, inputState);
        }
        public override void Draw()
        {
            Globals.mDebugSystem.UpdateFrameCounting();

            mSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Globals.mDebugSystem.ShowRenderInfo(new Vector2(2, 2));
            mSpriteBatch.End();

            mSpriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: Globals.mCamera2d.GetViewTransformationMatrix(), samplerState: SamplerState.PointClamp);
            DrawSystems();
            DrawGrid();
            mShipTest.Draw();
            mSpriteBatch.End();
        }
        public override void OnResolutionChanged()
        {
            Globals.mCamera2d.SetResolution(mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            mBackground.Update(new Rectangle(0, 0, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height));
            mParllaxManager.OnResolutionChanged();
        }
        public override void Destroy() { }

        // Constructor Stuff _____________________________________
        private void SpawnSystemsAndGetHome()
        {
            mPlanetSystemList = MapBuilder.Instance.Generate(25000, new Vector2(mapWidth, mapHeight));
            mHomeSystem = mPlanetSystemList[Globals.mRandom.Next(mPlanetSystemList.Count)];
            Globals.mCamera2d.mTargetPosition = mHomeSystem.Position;
        }
        private void InitializeGlobals()
        {
            Globals.mCamera2d = new(mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            Globals.mGameLayer = this;
            Globals.mDebugSystem = new DebugSystem();
        }
        private void InitializeParllax()
        {
            mParllaxManager.Add(new ParllaxBackground("gameBackgroundParlax", 0, 0.05f));
        }

        // Update Stuff _____________________________________
        private void UpdateSystems(GameTime gameTime, InputState inputState)
        {
            foreach (PlanetSystem planetSystem in mPlanetSystemList)
            {
                planetSystem.Update(gameTime, inputState);
            }
        }
        private void ManageTimeWarp(GameTime gameTime, InputState inputState)
        {
            if (inputState.mActionList.Contains(ActionType.AccelerateTime))
            {
                if (Globals.mTimeWarp >= 64) { return; }
                Globals.mTimeWarp *= 2;
                return;
            }
            if (inputState.mActionList.Contains(ActionType.DeaccelerateTime))
            {
                if (Globals.mTimeWarp <= 1) { return; }
                Globals.mTimeWarp /= 2;
                return;
            }
        }

        // Draw Stuff _____________________________________
        private void DrawSystems()
        {
            var counter = 0;
            foreach (PlanetSystem planetSystem in mPlanetSystemList)
            {
                if (!mFrustumCuller.IsOnScreen(planetSystem)) { continue; }
                planetSystem.Draw();
                counter++;
            }
            Debug.WriteLine($"Counted Objects: {counter}");
        }
        private void DrawGrid()
        {
            int ColorAplpha = 5;
            for (float x = -mMapSize.X; x <= mMapSize.Width / 2; x += 10000)
            {
                TextureManager.Instance.DrawLine(new Vector2(x, -mMapSize.Width / 1.9f),
                    new Vector2(x, mMapSize.Width / 1.9f), new Color(ColorAplpha, ColorAplpha, ColorAplpha, ColorAplpha), 1, 0);
            }
            for (float y = -mMapSize.Y; y <= mMapSize.Height / 2; y += 10000)
            {
                TextureManager.Instance.DrawLine(new Vector2(-mMapSize.Width / 1.9f, y),
                    new Vector2(mMapSize.Width / 1.9f, y), new Color(ColorAplpha, ColorAplpha, ColorAplpha, ColorAplpha), 1, 0);
            }
        }        
    }
}
