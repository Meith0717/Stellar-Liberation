// GameLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Position_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.MapSystem;
using CelestialOdyssey.Game.GameObjects;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.Layers.Scenes;
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
        [JsonIgnore] private readonly FrustumCuller mFrustumCuller;

        public GameLayer() : base()
        {
            mFrustumCuller = new();

            // Build and place player to startsystem
            Player = new();

            // Build and generate map
            Map = new();
            Map.Generate(Player, out PlanetSystems);

            // Play bg music
            // SoundManager.Instance.PlaySound(ContentRegistry.bgMusicGame, 1.2f, false, true, true);

            // Add Main Scene
            AddScene(new PlanetSystemScene(PlanetSystems.First(), PlanetSystems.ToList()));
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            // Check if pause is pressed
            inputState.DoAction(ActionType.ESC, () => mLayerManager.AddLayer(new PauseLayer()));

            // Check if mouse clicks outside window
            mFrustumCuller.Update(mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height, Matrix.Identity);
            if (!mFrustumCuller.VectorOnScreenView(inputState.mMousePosition) && (inputState.HasMouseAction(MouseActionType.LeftClick) || inputState.HasMouseAction(MouseActionType.RightClick))) mLayerManager.AddLayer(new PauseLayer());

            // Update Top Scene
            base.Update(gameTime, inputState);
        }

        public override void Destroy() { }

        public override void OnResolutionChanged() { }
    }
}
