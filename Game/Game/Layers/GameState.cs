// GameState.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.Debugger;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using StellarLiberation.Game.Layers.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Layers
{
    [Serializable]
    public class GameState
    {
        [JsonIgnore] private LayerManager mLayermanager;
        [JsonIgnore] public readonly DebugSystem DebugSystem = new();
        [JsonProperty] public readonly HashSet<PlanetSystem> PlanetSystems = new();
        [JsonProperty] public readonly Player Player = new();
        [JsonProperty] public readonly Inventory Inventory = new();
        [JsonProperty] public readonly Wallet Wallet = new();
        [JsonProperty] public PlanetSystem CurrentSystem { get; set; }

        [JsonIgnore] private PlanetSystemScene mPlanetSystemScene;

        public GameState() : base()
        {
            MapFactory.Generate(out PlanetSystems);

            // Add Main Scene
            CurrentSystem = PlanetSystems.First();
            Player.Position = Vector2.Zero;
        }

        public void Initialize(LayerManager layerManager)
        {
            mPlanetSystemScene = new(this, CurrentSystem.GetInstance(), 1);
            layerManager.AddLayer(mPlanetSystemScene);
            mLayermanager = layerManager;
        }

        public void Update(InputState inputState)
        {
            if (Player.DefenseSystem.HullForce == 0) mLayermanager.AddLayer(new PauseLayer());
        }

        public void LoadMap()
        {
            //AddScene(new MapScene(this, PlanetSystems.ToList(), CurrentSystem));    
        }

        public void ChangePlanetSystem(PlanetSystem planetSystem)
        {
            // CurrentSystem.ClearInstance();
            // CurrentSystem = planetSystem;
            // var zoom = mPlanetSystemScene.Camera2D.Zoom;
            // RemoveScene(mPlanetSystemScene);
            // mPlanetSystemScene = new PlanetSystemScene(this, CurrentSystem.GetInstance(), zoom);
            // AddScene(mPlanetSystemScene);
        }
    }
}
