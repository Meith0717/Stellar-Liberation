// GameLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.MapSystem;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.GameObjects.Recources.Items;
using StellarLiberation.Game.GameObjects.SpaceShipManagement;
using StellarLiberation.Game.Layers.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Layers
{
    [Serializable]
    public class GameLayer : SceneManagerLayer
    {
        [JsonProperty]
        public readonly HashSet<PlanetSystem> PlanetSystems = new();
        [JsonProperty]
        public readonly Player Player = new();
        [JsonProperty]
        public readonly Inventory Inventory = new(500);
        [JsonProperty]
        public readonly Wallet Wallet = new();

        [JsonIgnore]
        public PlanetSystem CurrentSystem { get; set; }
        [JsonIgnore]
        private PlanetSystemScene mPlanetSystemScene;
        [JsonIgnore]
        public readonly HudLayer HudLayer;

        public GameLayer() : base()
        {
            MapFactory.Generate(out PlanetSystems);

            MusicManager.Instance.PlayMusic(MusicRegistries.bgMusicGame);

            // Add Main Scene
            CurrentSystem = PlanetSystems.First();
            Player.Position = ExtendetRandom.NextVectorInCircle(CurrentSystem.SystemBounding);
            mPlanetSystemScene = new(this, CurrentSystem, 1);
            HudLayer = new(mPlanetSystemScene);
            Inventory.Collect(ItemFactory.Get(ItemID.Iron, Vector2.Zero, Vector2.Zero));
            Inventory.Collect(ItemFactory.Get(ItemID.Iron, Vector2.Zero, Vector2.Zero));
            Inventory.Collect(ItemFactory.Get(ItemID.Iron, Vector2.Zero, Vector2.Zero));
            Inventory.Collect(ItemFactory.Get(ItemID.Iron, Vector2.Zero, Vector2.Zero));
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            AddScene(mPlanetSystemScene);
            mLayerManager.AddLayer(HudLayer);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            // Check if mouse clicks outside window
            if (!mGraphicsDevice.Viewport.Bounds.Contains(inputState.mMousePosition) && (inputState.HasAction(ActionType.LeftClick) || inputState.HasAction(ActionType.RightClick))) mLayerManager.AddLayer(new PauseLayer());

            // Update Top Scene
            base.Update(gameTime, inputState);

            // Check if pause is pressed
            inputState.DoAction(ActionType.Inventar, () => mLayerManager.AddLayer(new InventoryLayer(Inventory)));
            inputState.DoAction(ActionType.ESC, () => mLayerManager.AddLayer(new PauseLayer()));
        }

        public override void Destroy()
        {
            SoundEffectManager.Instance.StopAllSounds();
            MusicManager.Instance.StopAllMusics();
        }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }

        public void LoadMap() => AddScene(new MapScene(this, PlanetSystems.ToList(), CurrentSystem));
        public void ChangePlanetSystem(PlanetSystem planetSystem)
        {
            CurrentSystem = planetSystem;
            var zoom = mPlanetSystemScene.Camera2D.Zoom;
            RemoveScene(mPlanetSystemScene);
            mPlanetSystemScene = new PlanetSystemScene(this, CurrentSystem, zoom);
            AddScene(mPlanetSystemScene);
        }
    }
}
