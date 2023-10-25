// PlanetSystemScene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Layers.Scenes
{
    internal class PlanetSystemScene : Scene
    {
        private PlanetSystem mPlanetSystem;
        private ParllaxManager mParlaxManager;
        private GameLayer mGameLayer;

        public PlanetSystemScene(GameLayer gameLayer, PlanetSystem planetSystem) : base(50000, 0.001f, 1, false)
        {
            mGameLayer = gameLayer;
            mPlanetSystem = planetSystem;
            mPlanetSystem.Player.ActualPlanetSystem = mPlanetSystem;
            mPlanetSystem.Player.SpawnInNewPlanetSystem(Vector2.Zero);

            mParlaxManager = new();
            mParlaxManager.Add(new(ContentRegistry.gameBackgroundParlax1, .1f));
            mPlanetSystem.SpawnShip();
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => mGameLayer.LoadMap(mPlanetSystem.Position));

            mParlaxManager.Update(Camera.Movement, Camera.Zoom);

            mGameLayer.DebugSystem.CheckForSpawn(mPlanetSystem);
            mPlanetSystem.UpdateObjects(gameTime, inputState, mGameLayer, this);
        }

        public override void DrawOnScreen() => mParlaxManager.Draw();

        public override void DrawOnWorld() {; }

        public override void OnResolutionChanged() { }
    }
}
