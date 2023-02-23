using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.Menu;
using rache_der_reti.Core.PositionManagement;
using rache_der_reti.Core.TextureManagement;
using rache_der_reti.Game.GameObjects;
using rache_der_reti.Game.Layers;
using Space_Game.Core;
using Space_Game.Core.Effects;
using Space_Game.Core.GameObject;
using Space_Game.Core.LayerManagement;
using Space_Game.Core.Maths;
using Space_Game.Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Space_Game.Game.Layers
{
    [Serializable]
    public class GameLayer : Layer
    {

        [JsonIgnore] public RectangleF mMapSize = new RectangleF(new Vector2(200000, 200000), new Vector2(200000, 200000) * 2);
        
        [JsonProperty] public double mPassedSeconds = 0;
        [JsonProperty] private PlanetSystem mHomeSystem;
        [JsonProperty] private List<PlanetSystem> mPlanetSystemList = new List<PlanetSystem>();
        [JsonProperty] private List<Ship> mShipList = new();
        
        // Recources
        [JsonProperty] public double mAlloys;
        [JsonProperty] public double mEnergy;
        [JsonProperty] public double mCrystals;
        
        [JsonIgnore] public SpatialHashing<GameObject> mSpatialHashing;
        [JsonIgnore] public SelectionRectangle mSelectionRectangle;

        private int mSpatialHashingCellSize = 2000;
        private HudLayer mHudLayer = new();
        private UiElementSprite mBackground;
        private ParllaxManager mParllaxManager;

        // Layer Stuff _____________________________________
        public GameLayer() : base()
        {
            InitializeGlobals();
            mBackground = new UiElementSprite("gameBackground");
            mBackground.mSpriteFit = UiElementSprite.SpriteFit.Cover;
            mSpatialHashing = new SpatialHashing<GameObject>(mSpatialHashingCellSize);
            mSelectionRectangle = new SelectionRectangle(Globals.mCamera2d);
            SpawnSystemsAndGetHome();
            mParllaxManager = new ParllaxManager();
            OnResolutionChanged();
            Globals.mTimeWarp = 1;
            InitializeParllax();
            // For Testing ____
            mShipList.Add(new Ship(mHomeSystem.Position + 
                new Vector2(Globals.mRandom.Next(-500, 500), Globals.mRandom.Next(-500, 500))));
            mShipList.Add(new Ship(mHomeSystem.Position +
                new Vector2(Globals.mRandom.Next(-500, 500), Globals.mRandom.Next(-500, 500))));

        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            mPassedSeconds += gameTime.ElapsedGameTime.Milliseconds / 1000d;
            mParllaxManager.Update();
            mSelectionRectangle.Update(gameTime, inputState);
            //mHudLayer.Update(gameTime, inputState);
            Globals.mCamera2d.Update(gameTime, inputState);
            UpdateSystems(gameTime, inputState);
            UpdateShips(gameTime, inputState);
            ManageTimeWarp(gameTime, inputState);
            TabToGoHome(gameTime, inputState);
        }
        public override void Draw()
        {
            mSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mBackground.Render();
            mSpriteBatch.End();
            mParllaxManager.Draw();
            mSpriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: Globals.mCamera2d.GetViewTransformationMatrix(), samplerState: SamplerState.PointClamp);
            mSelectionRectangle.Draw();
            DrawSystems();
            DrawShips();
            DrawGrid();
            mSpriteBatch.End();

            //mHudLayer.Draw();
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
            var startRadius = 0;
            var radAmount = 55;
            var radSteps = 3000;
            var probability = 0.5;

            for (int radius = startRadius + 0; radius <= startRadius + radAmount * radSteps; radius += radSteps)
            {
                float scope = 2 * MathF.PI * radius;
                float distribution = scope / 4200 * 2;
                float steps = MathF.PI * 2 / distribution;
                for (float angle = 0; angle < (MathF.PI * 2) - steps; angle += steps)
                {
                    if (Globals.mRandom.NextDouble() <= probability)
                    {
                        Vector2 position = MyMathF.GetInstance().GetCirclePosition(radius, angle, radSteps / 2);
                        PlanetSystem system = new PlanetSystem(position);
                        mPlanetSystemList.Add(system);
                    }
                }
                if (radius > radSteps * 30) { probability -= 0.015; }
            }
            int random = Globals.mRandom.Next(mPlanetSystemList.Count);
            mHomeSystem = mPlanetSystemList[random];
            Globals.mCamera2d.mTargetPosition = mHomeSystem.Position;
        }
        private void InitializeGlobals()
        {
            Globals.mCamera2d = new (mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            Globals.mGameLayer = this;
        }
        private void InitializeParllax()
        {
            mParllaxManager.Add(new ParllaxBackground("gameBackgroundParlax", 0, 0.05f));
            mParllaxManager.Add(new ParllaxBackground("gameBackgroundParlax", 0, 0.15f));
        }

        // Update Stuff _____________________________________
        private void UpdateSystems(GameTime gameTime, InputState inputState) 
        {
            foreach (PlanetSystem planetSystem in mPlanetSystemList)
            {
                planetSystem.Update(gameTime, inputState);
            }
        }
        private void UpdateShips(GameTime gameTime, InputState inputState)
        {
            foreach (Ship ship in mShipList)
            {
                ship.Update(gameTime, inputState);
            }
        }
        private void TabToGoHome(GameTime gameTime, InputState inputState)
        {
            if (inputState.mActionList.Contains(ActionType.GoHome))
            {
                Globals.mCamera2d.mTargetPosition = mHomeSystem.Position;
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
                if (Globals.mTimeWarp <=1) { return; }
                Globals.mTimeWarp /= 2;
                return;
            }
        }

        // Draw Stuff _____________________________________
        private void DrawSystems()
        {
            foreach (PlanetSystem planetSystem in mPlanetSystemList)
            {
                planetSystem.Draw();
            }
        }
        private void DrawShips()
        {
            foreach (Ship ship in mShipList)
            {
                ship.Draw();
            }
        }
        private void DrawGrid()
        {
            for (float x = -mMapSize.X; x <= mMapSize.Width / 2; x += 2000)
            {
                TextureManager.GetInstance().GetSpriteBatch().DrawLine(new Vector2(x, -mMapSize.Width / 2),
                    new Vector2(x, mMapSize.Width / 2), new Color(25, 25, 25, 25), 1f / Globals.mCamera2d.mZoom);
            }
            for (float y = -mMapSize.Y; y <= mMapSize.Height / 2; y += 2000)
            {
                TextureManager.GetInstance().GetSpriteBatch().DrawLine(new Vector2(-mMapSize.Width / 2, y),
                    new Vector2(mMapSize.Width / 2, y), new Color(25, 25, 25, 25), 1f / Globals.mCamera2d.mZoom);
            }
        }

        // Layer Logik _____________________________________
        public List<GameObject> GetObjectsInRadius(Vector2 positionVector2, int radius)
        {
            var objectsInRadius = new List<GameObject>();
            var maxRadius = radius + mSpatialHashingCellSize;
            for (var i = -radius; i <= maxRadius; i += mSpatialHashingCellSize)
            {
                for (var j = -radius; j <= maxRadius; j += mSpatialHashingCellSize)
                {
                    var objectsInBucket = mSpatialHashing.GetObjectsInBucket((int)(positionVector2.X + i), (int)(positionVector2.Y + j));
                    foreach (var gameObject in objectsInBucket)
                    {
                        var position = gameObject.Position;
                        var distance = Vector2.Distance(positionVector2, position);
                        if (distance <= radius)
                        {
                            objectsInRadius.Add(gameObject);
                        }
                    }
                }

            }
            return objectsInRadius;
        }
    }
}
