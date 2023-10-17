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

        public PlanetSystemScene(PlanetSystem planetSystem) : base(10000, 0.001f, 1, false)
        {
            mPlanetSystem = planetSystem;
            mPlanetSystem.Player.ActualPlanetSystem = mPlanetSystem;

            mParlaxManager = new();
            mParlaxManager.Add(new(ContentRegistry.gameBackgroundParlax1, .1f));
        }

        public override void Update(GameTime gameTime, InputState inputState, int screenWidth, int screenHeight)
        {
            base.Update(gameTime, inputState, screenWidth, screenHeight);

            mParlaxManager.Update(Camera.Movement, Camera.Zoom);

            mSceneManagerLayer.DebugSystem.CheckForSpawn(mPlanetSystem);
            mPlanetSystem.UpdateObjects(gameTime, inputState, mSceneManagerLayer, this);
        }

        public override void DrawOnScreen()
        {
            mParlaxManager.Draw();
        }

        public override void DrawOnWorld() => mPlanetSystem.DrawObjects(mSceneManagerLayer, this);

        public override void OnResolutionChanged() { }
    }
}
