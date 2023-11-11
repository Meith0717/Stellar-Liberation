// GameLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.ItemManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.MapSystem;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem;
using StellarLiberation.Game.GameObjects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.Layers.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Layers
{
    [Serializable]
    public class GameLayer : SceneManagerLayer
    {
        [JsonProperty] public readonly HashSet<PlanetSystem> PlanetSystems = new();
        [JsonProperty] public readonly Player Player = new();
        [JsonProperty] public readonly ProjectileManager ProjectileManager = new();
        [JsonProperty] public readonly ItemManager ItemManager = new();

        [JsonIgnore] public PlanetSystem CurrentSystem { get; set; }
        [JsonIgnore] private readonly MainGameScene mMainGameScene;

        public GameLayer() : base()
        {
            MapFactory.Generate(out PlanetSystems);

            // Play bg music
            SoundManager.Instance.PlaySound(MusicRegistries.bgMusicGame, 1.2f, false, true, true);

            // Add Main Scene
            CurrentSystem = PlanetSystems.First();
            Player.Position = CurrentSystem.QuantumGate.Position;
            mMainGameScene = new(this);
            AddScene(mMainGameScene);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            // Check if pause is pressed
            inputState.DoAction(ActionType.ESC, () => mLayerManager.AddLayer(new PauseLayer()));

            // Check if mouse clicks outside window
            if (!mGraphicsDevice.Viewport.Bounds.Contains(inputState.mMousePosition) && (inputState.HasMouseAction(MouseActionType.LeftClick) || inputState.HasMouseAction(MouseActionType.RightClick))) mLayerManager.AddLayer(new PauseLayer());

            System.Diagnostics.Debug.WriteLine(Player.Position);

            // Update Top Scene
            base.Update(gameTime, inputState);
        }

        public override void Destroy() { }

        public override void OnResolutionChanged() { }

        public void LoadMap() => AddScene(new MapScene(this, PlanetSystems.ToList(), CurrentSystem));
    }
}
