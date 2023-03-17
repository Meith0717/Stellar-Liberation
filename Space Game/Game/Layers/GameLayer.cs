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
        const int systemAmount = 1;

        [JsonIgnore] public RectangleF mMapSize = new RectangleF(new Vector2(mapWidth, mapHeight), new Vector2(mapWidth, mapHeight) * 2);

        [JsonProperty] public double mPassedSeconds = 0;
        [JsonProperty] private PlanetSystem mHomeSystem;
        [JsonProperty] private List<PlanetSystem> mPlanetSystemList = new List<PlanetSystem>();

        // Recources
        [JsonProperty] public double mAlloys;
        [JsonProperty] public double mEnergy;
        [JsonProperty] public double mCrystals;

        [JsonIgnore] public SpatialHashing<GameObject> mSpatialHashing;

        private int mSpatialHashingCellSize = 2000;
        private UiElementSprite mBackground;
        private ParllaxManager mParllaxManager;

        // Layer Stuff _____________________________________
        public GameLayer() : base()
        {
            InitializeGlobals();
            mBackground = new UiElementSprite("gameBackground");
            mBackground.mSpriteFit = UiElementSprite.SpriteFit.Cover;
            mSpatialHashing = new SpatialHashing<GameObject>(mSpatialHashingCellSize);
            SpawnSystemsAndGetHome();
            mParllaxManager = new ParllaxManager();
            OnResolutionChanged();
            Globals.mTimeWarp = 1;
            InitializeParllax();
        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            mPassedSeconds += gameTime.ElapsedGameTime.Milliseconds / 1000d;
            mParllaxManager.Update();
            Globals.mCamera2d.Update(gameTime, inputState);
            UpdateSystems(gameTime, inputState);
            ManageTimeWarp(gameTime, inputState);
            TabToGoHome(gameTime, inputState);
        }
        public override void Draw()
        {
            mSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            // mBackground.Render();
            // mParllaxManager.Draw();
            mSpriteBatch.End();

            mSpriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: Globals.mCamera2d.GetViewTransformationMatrix(), samplerState: SamplerState.PointClamp);
            DrawSystems();
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
                mPlanetSystemList.Add(new PlanetSystem(position));
                i++;
            }
            mHomeSystem = mPlanetSystemList[Globals.mRandom.Next(mPlanetSystemList.Count)];
            Globals.mCamera2d.mTargetPosition = mHomeSystem.mPosition;
        }
        private void InitializeGlobals()
        {
            Globals.mCamera2d = new(mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            Globals.mGameLayer = this;
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
        private void TabToGoHome(GameTime gameTime, InputState inputState)
        {
            if (inputState.mActionList.Contains(ActionType.GoHome))
            {
                Globals.mCamera2d.mTargetPosition = mHomeSystem.mPosition;
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

        private void DrawGrid()
        {
            for (float x = -mMapSize.X; x <= mMapSize.Width / 2; x += 10000)
            {
                TextureManager.GetInstance().DrawLine(new Vector2(x, -mMapSize.Width / 1.9f),
                    new Vector2(x, mMapSize.Width / 1.9f), new Color(25, 25, 25, 25), 2, 0);
            }
            for (float y = -mMapSize.Y; y <= mMapSize.Height / 2; y += 10000)
            {
                TextureManager.GetInstance().DrawLine(new Vector2(-mMapSize.Width / 1.9f, y),
                    new Vector2(mMapSize.Width / 1.9f, y), new Color(25, 25, 25, 25), 2, 0);
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
