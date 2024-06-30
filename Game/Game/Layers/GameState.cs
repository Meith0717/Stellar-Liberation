// GameState.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Penumbra;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.Debugging;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using StellarLiberation.Game.Layers.GameLayers;
using StellarLiberation.Game.Layers.MenueLayers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Layers
{
    [Serializable]
    public class GameState : Layer
    {

        [JsonIgnore] public readonly DebugSystem DebugSystem = new();
        [JsonIgnore] private readonly LinkedList<GameLayer> mLayers = new();
        [JsonIgnore] public readonly GameObjectsInteractor GameObjectsInteractor = new();
         [JsonProperty] public readonly MapState MapState = new();
        [JsonProperty] private readonly MapConfig mMapConfig;
        [JsonProperty] public readonly List<PlanetsystemState> PlanetSystemStates;

        [JsonIgnore] public PlanetsystemState ActualPlanetSystem { get; private set; }

        public GameState(Game1 game1) : base(game1, false, false)
        {
            mMapConfig = new(3, 3, 0);
            PlanetSystemStates = MapFactory.Generate(mMapConfig);
            for (var _ = 0; _ < 5; _++)
                PlanetSystemStates.First().GameObjects.Add(FlagshipFactory.Instance.GetFlagship("MKI", ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 20000)), Fraction.Allied));
        }

        public override void Initialize()
        {
            MapState.Initialize(PlanetSystemStates);
            foreach (var planetsystemState in PlanetSystemStates)
                planetsystemState.Initialize();
            ActualPlanetSystem = PlanetSystemStates.First();
            AddLayer(new PlanetsystemLayer(this, ActualPlanetSystem, Game1));
        }

        public void OpenMap() => AddLayer(new MapLayer(this, MapState, ActualPlanetSystem.MapPosition, Game1));

        public void CloseMap() => PopLayer();

        public void OpenPlanetSystem(PlanetsystemState planetsystemState)
        {
            ActualPlanetSystem = planetsystemState;
            PopLayer(); PopLayer();
            AddLayer(new PlanetsystemLayer(this, planetsystemState, Game1));
        }

        private void AddLayer(GameLayer layer)
        {
            layer.ApplyResolution();
            layer.Initialize();
            mLayers.AddLast(layer);
        }

        private void PopLayer()
        {
            if (mLayers.Count == 0) return;
            mLayers.Last.Value.Destroy();
            mLayers.RemoveLast();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, () => LayerManager.AddLayer(new PauseLayer(this, Game1)));
            DebugSystem.Update(inputState);
            MapState.Update(gameTime);
            foreach (var planetsystemState in PlanetSystemStates)
                planetsystemState.Update(gameTime, this);
            mLayers.Last?.Value.Update(gameTime, inputState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.Black);
            mLayers.Last?.Value.Draw(spriteBatch);
            spriteBatch.Begin();
            DebugSystem.ShowInfo(new(10, 10));
            spriteBatch.End();
            base.Draw(spriteBatch);
        }

        public override void ApplyResolution()
        {
            foreach (var layer in mLayers)
                layer.ApplyResolution();
            base.ApplyResolution();
        }
    }
}
