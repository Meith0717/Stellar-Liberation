// PlanetSystemScene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.UserInterface;
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
        private readonly UiSprite mBackground;

        public PlanetSystemScene(GameLayer gameLayer, PlanetSystem currentPlanetSystem, float camZoom) : base(gameLayer, 50000, 0.001f, 1, false)
        {
            Camera2D.Zoom = camZoom;
            mPlanetSystem = currentPlanetSystem;
            mParlaxManager = new();
            mBackground = new(GameSpriteRegistries.gameBackground) { Anchor = Anchor.Center, FillScale = FillScale.FillIn };
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            mBackground.Update(inputState, mGraphicsDevice.Viewport.Bounds, 1);
            inputState.DoAction(ActionType.ToggleHyperMap, () => GameLayer.LoadMap());

            GameLayer.Player.Update(gameTime, inputState, this);
            mPlanetSystem.UpdateObjects(gameTime, inputState, this);
            GameLayer.DebugSystem.CheckForSpawn(GameLayer.CurrentSystem, this);

            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, 0.001f, 1);
            Camera2DController.Manage(GameLayer.Player, inputState, Camera2D);
            mParlaxManager.Update(Vector2.Zero, Camera2D.Zoom);
        }

        public override void DrawOnScreenView(SceneManagerLayer sceneManagerLayer, SpriteBatch spriteBatch)
        {
            base.DrawOnScreenView(sceneManagerLayer, spriteBatch);
            mBackground.Draw();
        }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }
    }
}
