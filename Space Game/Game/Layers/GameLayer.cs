using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;
using Galaxy_Explovive.Core;
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
using Microsoft.Xna.Framework.Content;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.Rendering;

namespace Galaxy_Explovive.Game.Layers
{
    public class GameLayer : Layer
    {
        const int mapWidth = 3000000;
        const int mapHeight = 2000000;

        // Public GameObject Classes
        public float GameTime { get; set; }
        public SpatialHashing<GameObject> mSpatialHashing;
        public FrustumCuller mFrustumCuller;

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
        public GameLayer(LayerManager layerManager, SoundManager soundManager) : base(layerManager, soundManager)
        {
            InitializeGlobals();
            mSpatialHashing = new SpatialHashing<GameObject>(10000);
            mFrustumCuller = new();
            mPlanetSystemList = Map.Generate(this, 25000, new Vector2(mapWidth, mapHeight));
            mHomeSystem = MyUtility.GetRandomElement(mPlanetSystemList);
            Globals.Camera2d.mTargetPosition = mHomeSystem.Position;
            mParllaxManager = new();
            mParllaxManager.Add(new("gameBackground", 0, 0.1f));
            mParllaxManager.Add(new("gameBackgroundParlax2", 1, 0.25f));
            mParllaxManager.Add(new("gameBackgroundParlax1", 2, 0.5f));
            mShips.Add(new(this, MyUtility.GetRandomVector2(mHomeSystem.Position, 1000)));
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
            mFrustumCuller.Update();
            mParllaxManager.Update();
            Globals.DebugSystem.Update(gameTime, inputState);
            Globals.Camera2d.Update(gameTime, inputState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Globals.DebugSystem.UpdateFrameCounting();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw();
            Globals.DebugSystem.ShowRenderInfo(new Vector2(2, 2));
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, 
                transformMatrix: Globals.Camera2d.GetViewTransformationMatrix(), 
                samplerState: SamplerState.PointClamp);
            DrawSystems();
            Map.DrawGrid(mMapSize.ToRectangle());
            foreach(Cargo c in mShips)
            {
                c.Draw();
            }
            Globals.DebugSystem.DrawNearMousObjects();
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
            Globals.Camera2d = new();
            Globals.GameLayer = this;
            Globals.DebugSystem = new();
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
    }
}
