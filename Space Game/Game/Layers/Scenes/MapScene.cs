// MapScene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Layers.Scenes
{
    internal class MapScene : Scene
    {

        private List<PlanetSystem> mPlanetSystems;
        private ParllaxManager mParlaxManager;
        private GameLayer mGameLayer;
        private PlanetSystem mCurrentSystem;

        public MapScene(GameLayer gameLayer, List<PlanetSystem> planetSystems, PlanetSystem currentSystem) 
            : base(100, 1f, 3, false)
        {
            mCurrentSystem = currentSystem;
            mGameLayer = gameLayer;
            Camera.SetPosition(mCurrentSystem.Position);
            mPlanetSystems = planetSystems;
            foreach (var system in mPlanetSystems) SpatialHashing.InsertObject(system, (int)system.Position.X, (int)system.Position.Y);
            mParlaxManager = new();
            mParlaxManager.Add(new(ContentRegistry.gameBackgroundParlax1, .1f));
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => mGameLayer.PopScene());
            mParlaxManager.Update(Camera.Movement, Camera.Zoom);

            foreach (var item in mPlanetSystems) item.Update(gameTime, inputState, mGameLayer, this);
        }

        public override void DrawOnScreen() => mParlaxManager.Draw();

        public override void DrawOnWorld() 
        {
            TextureManager.Instance.Draw(ContentRegistry.mapCrosshair, mCurrentSystem.Position, 0.02f, 0, 1, Color.YellowGreen);
            var targetSystem = mCurrentSystem.Player.HyperDrive.TargetPlanetSystem;
            if (targetSystem is null) return;
            TextureManager.Instance.Draw(ContentRegistry.mapCrosshair, targetSystem.Position, 0.02f, 0, 1, Color.LightBlue);
        }

        public override void OnResolutionChanged() {; }
    }
}
