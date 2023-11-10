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
        private ParllaxManager mParlaxManager;
        private GameLayer mGameLayer;

        public PlanetSystemScene(GameLayer gameLayer, PlanetSystem planetSystem) : base(50000, 0.001f, 1, false)
        {
            mGameLayer = gameLayer;
            mPlanetSystem = planetSystem;
            mPlanetSystem.Player.ActualPlanetSystem = mPlanetSystem;
            mPlanetSystem.Player.SpawnInNewPlanetSystem(Vector2.Zero);

            mParlaxManager = new();
            mParlaxManager.Add(new(TextureRegistries.gameBackgroundParlax1, .1f));
            mPlanetSystem.SpawnShip();
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => mGameLayer.LoadMap());

            mParlaxManager.Update(Camera.Movement, Camera.Zoom);

            mGameLayer.DebugSystem.CheckForSpawn(mPlanetSystem);
            mPlanetSystem.UpdateObjects(gameTime, inputState, mGameLayer, this);
        }

        public override void DrawOnScreen() => mParlaxManager.Draw();

        public override void DrawOnWorld() {; }

        public override void OnResolutionChanged() { }
    }
}
