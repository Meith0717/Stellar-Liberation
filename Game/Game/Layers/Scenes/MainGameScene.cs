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
    internal class MainGameScene : Scene
    {
        private ParallaxController mParlaxManager;

        public MainGameScene(GameLayer gameLayer) : base(gameLayer, 50000, 0.00001f, 1, false)
        {
            mParlaxManager = new();
            mParlaxManager.Add(new(TextureRegistries.gameBackgroundParlax1, .1f));
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => GameLayer.LoadMap());

            mParlaxManager.Update(Camera2D.Movement, Camera2D.Zoom);
            GameLayer.Player.Update(gameTime, inputState, this);
            GameLayer.ItemManager.Update(gameTime, inputState, this);
            GameLayer.ProjectileManager.Update(gameTime, inputState, this);
            GameLayer.DebugSystem.CheckForSpawn(GameLayer.CurrentSystem);
            foreach (PlanetSystem planetSystem in GameLayer.PlanetSystems)
            {
                if (!ViewFrustumFilter.WorldFrustum.Intersects(planetSystem.SystemBounding)) continue;
                planetSystem.UpdateObjects(gameTime, inputState, this);
                GameLayer.CurrentSystem = planetSystem;
                break;
            }
        }

        public override void DrawOnScreen() => mParlaxManager.Draw();

        public override void DrawOnWorld() {; }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }
    }
}
