// GameLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.MapSystem;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.Game.Layers.Scenes;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Layers
{
    [Serializable]
    public class GameLayer : SceneManagerLayer
    {
        [JsonProperty] public readonly HashSet<PlanetSystem> PlanetSystems;
        [JsonProperty] public readonly Map Map;
        [JsonProperty] public readonly Player Player;

        public GameLayer() : base()
        {
            // Build and place player to startsystem
            Player = new();

            // Build and generate map
            Map = new();
            Map.Generate(Player, out PlanetSystems);

            // Play bg music
            // SoundManager.Instance.PlaySound(ContentRegistry.bgMusicGame, 1.2f, false, true, true);

            // Add Main Scene
            AddScene(new PlanetSystemScene(PlanetSystems.First()));
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);

            // ESC for Pause
            inputState.DoAction(ActionType.Back, () => mLayerManager.AddLayer(new PauseLayer()));
        }

        public override void Destroy() { }

        public override void OnResolutionChanged() { }
    }
}
