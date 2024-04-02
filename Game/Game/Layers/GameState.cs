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
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
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
        [JsonIgnore] private readonly LinkedList<Layer> mLayers = new();
        [JsonIgnore] private readonly FrameCounter mFrameCounter = new(200);
        [JsonIgnore] public readonly GameObjectsInteractor GameObjectsInteractor = new();
        [JsonIgnore] public List<PlanetSystem> PlanetSystems;
        [JsonProperty] private readonly List<PlanetsystemState> PlanetSystemStates;
        [JsonProperty] private readonly MapConfig mMapConfig;

        public GameState(Game1 game1) : base(game1, false)
        {
            mMapConfig = new(25, 25, 42);
            PlanetSystemStates = MapFactory.Generate(mMapConfig);
            PlanetSystemStates.First().GameObjects.Add(SpaceshipFactory.Get(Vector2.Zero, ShipID.Destroyer, Fractions.Allied));
        }

        public override void Initialize()
        {
            PlanetSystems = MapFactory.GetPlanetSystems(PlanetSystemStates);
            foreach (var planetsystemState in PlanetSystemStates)
                planetsystemState.Initialize();
            AddLayer(new PlanetSystemLayer(this, PlanetSystemStates.First(), Game1));
        }

        public void AddLayer(Layer layer)
        {
            layer.ApplyResolution();
            mLayers.AddLast(layer);
        }

        public void PopLayer()
        {
            if (mLayers.Count == 0) return;
            mLayers.Last.Value.Destroy();
            mLayers.RemoveLast();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        { 
            inputState.DoAction(ActionType.ESC, () => LayerManager.AddLayer(new PauseLayer(this, Game1)));
            mFrameCounter.Update(gameTime);
            DebugSystem.Update(inputState);
            foreach (var planetsystemState in PlanetSystemStates)
                planetsystemState.Update(gameTime, inputState, this);
            mLayers.Last?.Value.Update(gameTime, inputState);
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

        public override void ApplyResolution()
        {
            foreach (var layer in mLayers)
                layer.ApplyResolution();
        }
    }
}
