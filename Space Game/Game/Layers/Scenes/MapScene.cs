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

        public MapScene(List<PlanetSystem> planetSystems, Vector2 mapPosition) 
            : base(100, 0.1f, 1, true)
        {
            Camera.SetPosition(mapPosition);
            mPlanetSystems = planetSystems;
            foreach (var system in mPlanetSystems) SpatialHashing.InsertObject(system, (int)system.Position.X, (int)system.Position.Y);
            mParlaxManager = new();
            mParlaxManager.Add(new(ContentRegistry.gameBackgroundParlax1, .1f));
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleMap, () => mSceneManagerLayer.PopScene());
            mParlaxManager.Update(Camera.Movement, Camera.Zoom);

            foreach (var item in mPlanetSystems)
            {
                item.Update(gameTime, inputState, mSceneManagerLayer, this);
                if (!item.LeftPressed) continue;
                mSceneManagerLayer.PopScene();
                mSceneManagerLayer.PopScene();
                mSceneManagerLayer.AddScene(new PlanetSystemScene(item, mPlanetSystems)); 
            }
        }

        public override void DrawOnScreen() => mParlaxManager.Draw();

        public override void DrawOnWorld() {; }

        public override void OnResolutionChanged() {; }
    }
}
