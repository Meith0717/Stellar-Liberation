// PlanetSystemScene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParallaxSystem;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;

namespace StellarLiberation.Game.Layers.Scenes
{
    internal class PlanetSystemScene : Scene
    {

        private readonly ParallaxController mParlaxManager;
        private readonly PlanetSystem mPlanetSystem;

        public PlanetSystemScene(GameLayer gameLayer, PlanetSystem currentPlanetSystem, float camZoom) : base(gameLayer, 50000, 0.001f, 1, false)
        {
            Camera2D.Zoom = camZoom;
            mPlanetSystem = currentPlanetSystem;
            mParlaxManager = new();
            mParlaxManager.Add(new(TextureRegistries.gameBackground, 0));
            mParlaxManager.Add(new(TextureRegistries.gameBackgroundParlax4, .1f));
            mParlaxManager.Add(new(TextureRegistries.gameBackgroundParlax3, .05f));
            mParlaxManager.Add(new(TextureRegistries.gameBackgroundParlax2, .01f));
            mParlaxManager.Add(new(TextureRegistries.gameBackgroundParlax1, .001f));
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => GameLayer.LoadMap());

            mParlaxManager.Update(Camera2D.Movement, Camera2D.Zoom);
            GameLayer.Player.Update(gameTime, inputState, this);
            mPlanetSystem.UpdateObjects(gameTime, inputState, this);
            GameLayer.DebugSystem.CheckForSpawn(GameLayer.CurrentSystem, this);
            Camera2DController.Track(GameLayer.Player, this);
        }

        public override void DrawOnScreenView(SceneManagerLayer sceneManagerLayer, SpriteBatch spriteBatch)
        {
            base.DrawOnScreenView(sceneManagerLayer, spriteBatch);
            mParlaxManager.Draw();
        }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }
    }
}
