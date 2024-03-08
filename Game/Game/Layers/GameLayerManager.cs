// GameLayerManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.Debugging;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using StellarLiberation.Game.Layers.GameLayers;
using StellarLiberation.Game.Layers.MenueLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace StellarLiberation.Game.Layers
{
    [Serializable]
    public class GameLayerManager : Layer
    {
        [JsonIgnore] private readonly FrameCounter mFrameCounter = new(1000);
        [JsonIgnore] public readonly DebugSystem DebugSystem = new();
        [JsonIgnore] private readonly LinkedList<Layer> mLayers = new();
        [JsonIgnore] private PlanetSystemLayer mMainLayer;
        [JsonIgnore] public List<PlanetSystem> mPlanetSystems { get; private set; }
        [JsonProperty] public readonly PlanetsystemSpaceshipTracer mSpaceshipPSManager = new();
        [JsonProperty] private readonly MapConfig mMapConfig;
        [JsonProperty] public readonly Spaceship Player;
        [JsonProperty] public readonly Wallet Wallet = new();

        public GameLayerManager() : base(false)
        {
            mMapConfig = new(3, 3, 0);
            Player = SpaceshipFactory.Get(Vector2.Zero, ShipID.Destroyer, Fractions.Allied);
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, PersistanceManager persistanceManager, GameSettings gameSettings, ResolutionManager resolutionManager)
        {
            mPlanetSystems = MapFactory.Generate(mMapConfig);
            if (mSpaceshipPSManager.LocateSpaceShip(Player) is null)
                mSpaceshipPSManager.AddSpaceShip(mPlanetSystems.First(), Player);
            base.Initialize(game1, layerManager, graphicsDevice, persistanceManager, gameSettings, resolutionManager);
        }

        public void AddLayer(Layer layer)
        {
            mLayers.AddLast(layer);
            layer.Initialize(Game1, LayerManager, GraphicsDevice, PersistanceManager, GameSettings, ResolutionManager);
        }

        public void PopLayer()
        {
            if (mLayers.Count == 0) return;
            mLayers.Last.Value.Destroy();
            mLayers.RemoveLast();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mFrameCounter.Update(gameTime);
            DebugSystem.Update(inputState);
            if (Player.IsDisposed) LayerManager.PopLayer();
            inputState.DoAction(ActionType.ESC, () => LayerManager.AddLayer(new PauseLayer(this)));
            mLayers.Last?.Value.Update(gameTime, inputState);
            if (mMainLayer?.PlanetSystem == mSpaceshipPSManager.LocateSpaceShip(Player)) return;
            mMainLayer = new(this, mSpaceshipPSManager.LocateSpaceShip(Player), mMainLayer?.Camera2D.Zoom is null ? 1 : mMainLayer.Camera2D.Zoom);
            PopLayer();
            AddLayer(mMainLayer);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mFrameCounter.UpdateFrameCouning();
             GraphicsDevice.Clear(Color.Black);
            mLayers.Last?.Value.Draw(spriteBatch);
            spriteBatch.Begin();
            DebugSystem.ShowInfo(new(10, 10));
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(1, 1), $"{MathF.Round(mFrameCounter.CurrentFramesPerSecond)} fps", 0.75f, Color.White);
            spriteBatch.End();
        }

        public override void Destroy() { }

        public override void OnResolutionChanged() { }
    }
}
