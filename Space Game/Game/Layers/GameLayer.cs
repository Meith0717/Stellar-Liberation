using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;
using Space_Game.Core;
using Space_Game.Core.Effects;
using Space_Game.Core.GameObject;
using Space_Game.Core.InputManagement;
using Space_Game.Core.LayerManagement;
using Space_Game.Core.Maths;
using Space_Game.Core.Menu;
using Space_Game.Core.PositionManagement;
using Space_Game.Core.TextureManagement;
using Space_Game.Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Space_Game.Game.Layers
{
    [Serializable]
    public class GameLayer : Layer
    {
        const int mapWidth = 300000;
        const int mapHeight = 300000;
        const int systemAmount = 1500;

        [JsonIgnore] public RectangleF mMapSize = new RectangleF(new Vector2(mapWidth, mapHeight), new Vector2(mapWidth, mapHeight) * 2);

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
                new Vector2(Globals.mRandom.Next(-500, 500), Globals.mRandom.Next(-500, 500)), true));

        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            mPassedSeconds += gameTime.ElapsedGameTime.Milliseconds / 1000d;
            mParllaxManager.Update();
            mSelectionRectangle.Update(gameTime, inputState);
            Globals.mCamera2d.Update(gameTime, inputState);
            UpdateSystems(gameTime, inputState);
            UpdateShips(gameTime, inputState);
            ManageTimeWarp(gameTime, inputState);
            TabToGoHome(gameTime, inputState);
            Globals.mWeaponManager.Update(gameTime);
        }
        public override void Draw()
        {
            mSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mBackground.Render();
            //mParllaxManager.Draw();
            mSpriteBatch.End();

            mSpriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: Globals.mCamera2d.GetViewTransformationMatrix(), samplerState: SamplerState.PointClamp);
            Globals.mWeaponManager.Draw();
            mSelectionRectangle.Draw();
            DrawSystems();
            DrawShips();
            DrawGrid();
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
            int i = 0;
            while (i < systemAmount)
            {
                Vector2 position = new Vector2(Globals.mRandom.Next(-mapWidth, mapWidth), Globals.mRandom.Next(-mapHeight, mapHeight));
                List<PlanetSystem> neighbourSystem = GetObjectsInRadius(position, 10000).OfType<PlanetSystem>().ToList();
                if (neighbourSystem.Count > 0)
                {
                    continue;
                }

                PlanetSystem system = new PlanetSystem(position);
                mPlanetSystemList.Add(system);
                mSpatialHashing.InsertObject(system, (int)position.X, (int)position.Y);
                i++;
            }
            mHomeSystem = mPlanetSystemList[Globals.mRandom.Next(mPlanetSystemList.Count)];
            Globals.mCamera2d.mTargetPosition = mHomeSystem.Position;
        }
        private void InitializeGlobals()
        {
            Globals.mCamera2d = new(mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            Globals.mGameLayer = this;
            Globals.mWeaponManager = new WeaponManager();
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
                if (Globals.mTimeWarp <= 1) { return; }
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
            for (float x = -mMapSize.X; x <= mMapSize.Width / 2; x += 10000)
            {
                TextureManager.GetInstance().GetSpriteBatch().DrawLine(new Vector2(x, - mMapSize.Width / 1.9f),
                    new Vector2(x, mMapSize.Width / 1.9f), new Color(25, 25, 25, 25), 1f / Globals.mCamera2d.mZoom);
            }
            for (float y = -mMapSize.Y; y <= mMapSize.Height / 2; y += 10000)
            {
                TextureManager.GetInstance().GetSpriteBatch().DrawLine(new Vector2(-mMapSize.Width / 1.9f, y),
                    new Vector2(mMapSize.Width / 1.9f, y), new Color(25, 25, 25, 25), 1f / Globals.mCamera2d.mZoom);
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
