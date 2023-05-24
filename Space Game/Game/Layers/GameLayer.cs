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
using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Game.GameObjects;
using System;
using System.Collections.Generic;
using Galaxy_Explovive.Core.Map;
using Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips;
using System.Diagnostics;
using Galaxy_Explovive.Core.Utility;

namespace Galaxy_Explovive.Game.Layers
{
    [Serializable]
    public class GameLayer : Layer
    {
        const int mapWidth = 3000000;
        const int mapHeight = 2000000;

        [JsonIgnore] public RectangleF mMapSize = new RectangleF(new Vector2(mapWidth, mapHeight), new Vector2(mapWidth, mapHeight) * 2);

        [JsonProperty] private PlanetSystem mHomeSystem;
        [JsonProperty] private List<PlanetSystem> mPlanetSystemList = new List<PlanetSystem>();
        [JsonProperty] public float GameTime { get; set; }

        // Recources
        [JsonProperty] public double mAlloys;
        [JsonProperty] public double mEnergy;
        [JsonProperty] public double mCrystals;

        [JsonIgnore] public SpatialHashing<GameObject> mSpatialHashing;

        private ParllaxManager mParllaxManager;
        private List<Cargo> mShips = new();

        // Layer Stuff _____________________________________
        public GameLayer() : base()
        {
            InitializeGlobals();
            mSpatialHashing = new SpatialHashing<GameObject>(10000);
            mPlanetSystemList = MapBuilder.Instance.Generate(25000, new Vector2(mapWidth, mapHeight));
            mHomeSystem = MyUtility.GetRandomElement(mPlanetSystemList);
            Globals.mCamera2d.mTargetPosition = mHomeSystem.Position;
            mParllaxManager = new();
            mParllaxManager.Add(new("gameBackground", 0, 0.1f));
            mParllaxManager.Add(new("gameBackgroundParlax2", 1, 0.25f));
            mParllaxManager.Add(new("gameBackgroundParlax1", 2, 0.5f));
            mShips.Add(new(MyUtility.GetRandomVector2(mHomeSystem.Position, 1000)));
            OnResolutionChanged();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            GameTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            Debug.WriteLine(GameTime);
            UpdateSystems(gameTime, inputState);
            foreach (Cargo c in mShips)
            {
                c.Update(gameTime, inputState);
            }
            if (inputState.mActionList.Contains(ActionType.ToggleRayTracing))
            {
                Globals.mRayTracing = !Globals.mRayTracing;
            }
            Globals.mFrustumCuller.Update();
            mParllaxManager.Update();
            Globals.mDebugSystem.Update(gameTime, inputState);
            Globals.mCamera2d.Update(gameTime, inputState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Globals.mDebugSystem.UpdateFrameCounting();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw();
            Globals.mDebugSystem.ShowRenderInfo(new Vector2(2, 2));
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, 
                transformMatrix: Globals.mCamera2d.GetViewTransformationMatrix(), 
                samplerState: SamplerState.PointClamp);
            DrawSystems();
            DrawGrid();
            foreach(Cargo c in mShips)
            {
                c.Draw();
            }
            Globals.mDebugSystem.DrawNearMousObjects();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mParllaxManager.OnResolutionChanged();
        }

        public override void Destroy() { }

        // Constructor Stuff _____________________________________
        private void InitializeGlobals()
        {
            Globals.mCamera2d = new();
            Globals.mGameLayer = this;
            Globals.mDebugSystem = new();
            Globals.mFrustumCuller = new();
        }

        // Update Stuff _____________________________________
        private void UpdateSystems(GameTime gameTime, InputState inputState)
        {
            foreach (PlanetSystem planetSystem in mPlanetSystemList)
            {
                planetSystem.Update(gameTime, inputState);
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
        private void DrawGrid()
        {
            int ColorAplpha = 20;

            for (float x = -mMapSize.X; x <= mMapSize.Width / 2; x += 10000)
            {
                TextureManager.Instance.DrawAdaptiveLine(new Vector2(x, -mMapSize.Height / 2f - 10000),
                    new Vector2(x, mMapSize.Height / 2f + 10000), new Color(ColorAplpha, ColorAplpha, ColorAplpha, ColorAplpha), 1, 0);
            }

            for (float y = -mMapSize.Y; y <= mMapSize.Height / 2; y += 10000)
            {
                TextureManager.Instance.DrawAdaptiveLine(new Vector2(-mMapSize.Width / 2f - 10000, y),
                    new Vector2(mMapSize.Width / 2f + 10000, y), new Color(ColorAplpha, ColorAplpha, ColorAplpha, ColorAplpha), 1, 0);
            }
        }        
    }
}
