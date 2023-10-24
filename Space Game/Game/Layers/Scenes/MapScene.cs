// MapScene.cs 
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
    internal class MapScene : Scene
    {

        private List<PlanetSystem> mPlanetSystems;
        private ParllaxManager mParlaxManager;
        private GameLayer mGameLayer;

        public MapScene(GameLayer gameLayer, List<PlanetSystem> planetSystems, Vector2 mapPosition) 
            : base(100, 0.1f, 1, false)
        {
            mGameLayer = gameLayer;
            Camera.SetPosition(mapPosition);
            mPlanetSystems = planetSystems;
            foreach (var system in mPlanetSystems) SpatialHashing.InsertObject(system, (int)system.Position.X, (int)system.Position.Y);
            mParlaxManager = new();
            mParlaxManager.Add(new(ContentRegistry.gameBackgroundParlax1, .1f));
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleMap, () => mGameLayer.PopScene());
            mParlaxManager.Update(Camera.Movement, Camera.Zoom);

            foreach (var item in mPlanetSystems) item.Update(gameTime, inputState, mGameLayer, this);
        }

        public override void DrawOnScreen() => mParlaxManager.Draw();

        public override void DrawOnWorld() {; }

        public override void OnResolutionChanged() {; }
    }
}
