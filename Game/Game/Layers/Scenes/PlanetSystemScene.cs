// PlanetSystemScene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.Parallax;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;

namespace StellarLiberation.Game.Layers.Scenes
{
    internal class PlanetSystemScene : Scene
    {
        private PlanetSystem mPlanetSystem;
        private ParallaxController mParlaxManager;

        public PlanetSystemScene(GameLayer gameLayer, PlanetSystem planetSystem) : base(gameLayer, 50000, 0.001f, 1, false)
        {
            mPlanetSystem = planetSystem;
            mPlanetSystem.Player.ActualPlanetSystem = mPlanetSystem;
            mPlanetSystem.Player.SpawnInNewPlanetSystem(Vector2.Zero);

            mParlaxManager = new();
            mParlaxManager.Add(new(TextureRegistries.gameBackgroundParlax1, .1f));
            mPlanetSystem.SpawnShip();
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => GameLayer.LoadMap());

            mParlaxManager.Update(Camera2D.Movement, Camera2D.Zoom);

            GameLayer.DebugSystem.CheckForSpawn(mPlanetSystem);
            mPlanetSystem.UpdateObjects(gameTime, inputState, this);
        }

        public override void DrawOnScreen() => mParlaxManager.Draw();

        public override void DrawOnWorld() {; }

        public override void OnResolutionChanged() { }
    }
}
