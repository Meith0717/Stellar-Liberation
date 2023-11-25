// PlanetSystemScene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.Camera;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.Parallax;
using StellarLiberation.Game.Core.PositionManagement;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;

namespace StellarLiberation.Game.Layers.Scenes
{
    internal class MainGameScene : Scene
    {
        private ParallaxController mParlaxManager;
        private Compass mCompass = new();

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
            Camer2DController.Track(GameLayer.Player, this);
            mCompass.Update(Camera2D.Position, ViewFrustumFilter, GameLayer.Player.SensorArray.ShipsInDistance);
        }

        public override void DrawOnScreenView(SceneManagerLayer sceneManagerLayer, SpriteBatch spriteBatch)
        {
            base.DrawOnScreenView(sceneManagerLayer, spriteBatch);
            mParlaxManager.Draw();
            mCompass.Draw();
        }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }
    }
}
