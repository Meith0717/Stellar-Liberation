// GameState.cs 
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
using StellarLiberation.Game.GameObjects.SpaceCrafts;
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
        [JsonIgnore] private readonly FrameCounter mFrameCounter = new(200);
        [JsonIgnore] public readonly GameObjectsInteractor GameObjectsInteractor = new();
        [JsonProperty] public readonly MapState MapState = new();
        [JsonProperty] private readonly MapConfig mMapConfig;
        [JsonProperty] private readonly List<PlanetsystemState> PlanetSystemStates;

        public GameState(Game1 game1) : base(game1, false)
        {
            mMapConfig = new(5, 5, 0);
            PlanetSystemStates = MapFactory.Generate(mMapConfig);
            for (int i = 0; i < 1; i++)
                PlanetSystemStates.First().GameObjects.Add(SpacecraftFactory.GetFlagship(Vector2.Zero, Fractions.Allied));
        }

        public override void Initialize()
        {
            MapState.Initialize(PlanetSystemStates);
            foreach (var planetsystemState in PlanetSystemStates)
                planetsystemState.Initialize();
            AddLayer(new PlanetsystemLayer(this, PlanetSystemStates.First(), Game1));
        }

        public void AddLayer(GameLayer layer)
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
            MapState.Update(gameTime);
            foreach (var planetsystemState in PlanetSystemStates)
                planetsystemState.Update(gameTime, this);
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
