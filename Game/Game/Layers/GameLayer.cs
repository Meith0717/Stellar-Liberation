// GameLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.ItemManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.MapSystem;
using StellarLiberation.Game.Core.Persistance;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem;
using StellarLiberation.Game.Core.Utilitys;
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
        [JsonIgnore] public readonly HudLayer HudLayer;

        public GameLayer() : base()
        {
            MapFactory.Generate(out PlanetSystems);

            MusicManager.Instance.PlayMusic(MusicRegistries.bgMusicGame);

            // Add Main Scene
            CurrentSystem = PlanetSystems.First();
            Player.Position = ExtendetRandom.NextVectorInCircle(CurrentSystem.SystemBounding);
            mMainGameScene = new(this);
            HudLayer = new(mMainGameScene);
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            AddScene(mMainGameScene);
            mLayerManager.AddLayer(HudLayer);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            // Check if mouse clicks outside window
            if (!mGraphicsDevice.Viewport.Bounds.Contains(inputState.mMousePosition) && (inputState.HasAction(ActionType.LeftClick) || inputState.HasAction(ActionType.RightClick))) mLayerManager.AddLayer(new PauseLayer());

            // Update Top Scene
            base.Update(gameTime, inputState);

            // Check if pause is pressed
            inputState.DoAction(ActionType.ESC, () => mLayerManager.AddLayer(new PauseLayer()));
        }

        public override void Destroy()
        {
            SoundEffectManager.Instance.StopAllSounds();
            MusicManager.Instance.StopAllMusics();
        }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }

        public void LoadMap() => AddScene(new MapScene(this, PlanetSystems.ToList(), CurrentSystem));
    }
}
