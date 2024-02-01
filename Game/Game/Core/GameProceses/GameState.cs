// GameState.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.Debugger;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using StellarLiberation.Game.Layers.GameLayers;
using StellarLiberation.Game.Layers.MenueLayers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses
{
    [Serializable]
    public class GameState : Layer
    {
        [JsonIgnore] public readonly DebugSystem DebugSystem = new();
        [JsonIgnore] private readonly LinkedList<Layer> mLayers = new();
        [JsonProperty] public PlanetSystem CurrentSystem { get; set; }
        [JsonProperty] public readonly HashSet<PlanetSystem> PlanetSystems = new();
        [JsonProperty] public readonly Player Player = new();
        [JsonProperty] public readonly Inventory Inventory = new();
        [JsonProperty] public readonly Wallet Wallet = new();

        public GameState() : base(false)
        {
            MapFactory.Generate(out PlanetSystems);
            CurrentSystem = PlanetSystems.First();
            Player.Position = Vector2.Zero;
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, PersistanceManager persistanceManager, GameSettings gameSettings, ResolutionManager resolutionManager)
        {
            base.Initialize(game1, layerManager, graphicsDevice, persistanceManager, gameSettings, resolutionManager);
            AddLayer(new PlanetSystemLayer(this, 1));
        }

        public void AddLayer(Layer layer)
        {
            mLayers.AddLast(layer);
            layer.Initialize(Game1, LayerManager, GraphicsDevice, PersistanceManager, GameSettings, ResolutionManager);
        }

        public void PopLayer()
        {
            mLayers.Last.Value.Destroy();
            mLayers.RemoveLast();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, () => LayerManager.AddLayer(new PauseLayer(this)));
            mLayers.Last.Value.Update(gameTime, inputState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.Black);
            mLayers.Last.Value.Draw(spriteBatch);
        }

        public override void Destroy()
        {
        }

        public override void OnResolutionChanged()
        {
        }
    }
}
