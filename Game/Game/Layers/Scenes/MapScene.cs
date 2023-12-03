// MapScene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
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
            gameLayer.HudLayer.Hide = true;
            mCurrentSystem = currentSystem;
            Camera2D.SetPosition(mCurrentSystem.MapObj.Position);
            mPlanetSystems = planetSystems;
            foreach (var system in mPlanetSystems)
            {
                var mapObj = system.MapObj;
                SpatialHashing.InsertObject(mapObj, (int)mapObj.Position.X, (int)mapObj.Position.Y);
            }
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => { GameLayer.HudLayer.Hide = false; GameLayer.PopScene(); });

            foreach (var system in mPlanetSystems)
            {
                var mapObj = system.MapObj;
                mapObj.Update(gameTime, inputState, this);
            }
        }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }
    }
}
