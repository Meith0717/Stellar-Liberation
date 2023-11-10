// MapScene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.Parallax;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using System.Collections.Generic;

namespace StellarLiberation.Game.Layers.Scenes
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
            mParlaxManager.Add(new(TextureRegistries.gameBackgroundParlax1, .1f));
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
            TextureManager.Instance.Draw(TextureRegistries.mapCrosshair, mCurrentSystem.Position, 0.02f, 0, 1, Color.YellowGreen);
            var targetSystem = mCurrentSystem.Player.HyperDrive.TargetPlanetSystem;
            if (targetSystem is null) return;
            TextureManager.Instance.Draw(TextureRegistries.mapCrosshair, targetSystem.Position, 0.02f, 0, 1, Color.LightBlue);
        }

        public override void OnResolutionChanged() {; }
    }
}
