﻿// PlanetSystemScene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Layers.Scenes
{
    internal class PlanetSystemScene : Scene
    {
        private List<PlanetSystem> mPlanetSystems;
        private PlanetSystem mPlanetSystem;
        private ParllaxManager mParlaxManager;

        public PlanetSystemScene(PlanetSystem planetSystem, List<PlanetSystem> planetSystems) : base(50000, 0.001f, 1, false)
        {
            mPlanetSystems = planetSystems;

            mPlanetSystem = planetSystem;
            mPlanetSystem.Player.ActualPlanetSystem = mPlanetSystem;
            mPlanetSystem.Player.SpawnInNewPlanetSystem(Vector2.Zero);

            mParlaxManager = new();
            mParlaxManager.Add(new(ContentRegistry.gameBackgroundParlax1, .1f));
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleMap, OpenMap);

            mParlaxManager.Update(Camera.Movement, Camera.Zoom);

            mSceneManagerLayer.DebugSystem.CheckForSpawn(mPlanetSystem);
            mPlanetSystem.UpdateObjects(gameTime, inputState, mSceneManagerLayer, this);
        }

        public override void DrawOnScreen() => mParlaxManager.Draw();

        public override void DrawOnWorld() {; }

        public override void OnResolutionChanged() { }

        public void OpenMap() => mSceneManagerLayer.AddScene(new MapScene(mPlanetSystems, mPlanetSystem.Position));
    }
}
