// GameLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.MapSystem;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips.Allies;
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
        public readonly Inventory Inventory = new();
        [JsonProperty]
        public readonly Wallet Wallet = new();
        [JsonProperty]
        public PlanetSystem CurrentSystem { get; set; }

        [JsonIgnore]
        private PlanetSystemScene mPlanetSystemScene;
        [JsonIgnore]
        public readonly HudLayer HudLayer;

        public GameLayer() : base()
        {
            MapFactory.Generate(out PlanetSystems);

            // Add Main Scene
            CurrentSystem = PlanetSystems.First();
            Player.Position = ExtendetRandom.NextVectorInCircle(CurrentSystem.SystemBounding);
            mPlanetSystemScene = new(this, CurrentSystem, 1);
            HudLayer = new(mPlanetSystemScene);
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, PersistanceManager persistanceManager)
        {
            base.Initialize(game1, layerManager, graphicsDevice, persistanceManager);
            LayerManager.AddLayer(HudLayer);
            AddScene(mPlanetSystemScene);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {

            // Check if mouse clicks outside window
            if (!mGraphicsDevice.Viewport.Bounds.Contains(inputState.mMousePosition) && (inputState.HasAction(ActionType.LeftClick) || inputState.HasAction(ActionType.RightClick))) LayerManager.AddLayer(new PauseLayer(this));

            if (Player.DefenseSystem.HullForce == 0) LayerManager.PopLayer();

            // Update Top Scene
            base.Update(gameTime, inputState);

            // Check if pause is pressed
            inputState.DoAction(ActionType.Inventar, () => LayerManager.AddLayer(new InventoryLayer(Inventory)));
            inputState.DoAction(ActionType.ESC, () => LayerManager.AddLayer(new PauseLayer(this)));
        }

        public override void Destroy()
        {
            SoundEffectManager.Instance.StopAllSounds();
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
            Player.Position = ExtendetRandom.NextVectorInCircle(CurrentSystem.SystemBounding);
        }
    }
}
