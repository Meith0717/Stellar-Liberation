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
        private ParallaxController mParallaxController;
        private PlanetSystem mCurrentSystem;

        public MapScene(GameLayer gameLayer, List<PlanetSystem> planetSystems, PlanetSystem currentSystem)
            : base(gameLayer, 100, 1f, 3, false)
        {
            mCurrentSystem = currentSystem;
            Camera2D.SetPosition(mCurrentSystem.Position);
            mPlanetSystems = planetSystems;
            foreach (var system in mPlanetSystems) SpatialHashing.InsertObject(system, (int)system.Position.X, (int)system.Position.Y);
            mParallaxController = new();
            mParallaxController.Add(new(TextureRegistries.gameBackgroundParlax1, .1f));
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => GameLayer.PopScene());
            mParallaxController.Update(Camera2D.Movement, Camera2D.Zoom);

            foreach (var item in mPlanetSystems) item.Update(gameTime, inputState, this);
        }

        public override void DrawOnScreen() => mParallaxController.Draw();

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
