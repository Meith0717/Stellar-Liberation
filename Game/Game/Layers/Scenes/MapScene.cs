// MapScene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using System.Collections.Generic;

namespace StellarLiberation.Game.Layers.Scenes
{
    internal class MapScene : Scene
    {

        private List<PlanetSystem> mPlanetSystems;
        private PlanetSystem mCurrentSystem;

        public MapScene(GameLayer gameLayer, List<PlanetSystem> planetSystems, PlanetSystem currentSystem)
            : base(gameLayer, 100, 1f, 3, true, .9f, .9f)
        {
            mCurrentSystem = currentSystem;
            Camera2D.SetPosition(mCurrentSystem.Position);
            mPlanetSystems = planetSystems;
            foreach (var system in mPlanetSystems) SpatialHashing.InsertObject(system, (int)system.Position.X, (int)system.Position.Y);
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => GameLayer.PopScene());

            foreach (var item in mPlanetSystems) item.Update(gameTime, inputState, this);
        }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }
    }
}
