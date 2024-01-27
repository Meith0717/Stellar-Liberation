// PlanetSystemScene.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.GridSystem;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;

namespace StellarLiberation.Game.Layers.Scenes
{
    internal class PlanetSystemScene : Scene
    {
        private readonly UiFrame mBackgroundLayer;

        private readonly PlanetSystemInstance mPlanetSystem;
        private readonly Grid mGrid;

        public PlanetSystemScene(GameLayer gameLayer, PlanetSystemInstance currentPlanetSystem, float camZoom) : base(gameLayer, 50000)
        {
            mBackgroundLayer = new() { Color = Color.Black, Anchor = Anchor.Center, FillScale = FillScale.FillIn };
            mBackgroundLayer.AddChild(new UiSprite(GameSpriteRegistries.gameBackground) { Anchor = Anchor.Center, FillScale = FillScale.FillIn });

            Camera2D.Zoom = camZoom;
            mPlanetSystem = currentPlanetSystem;
            mGrid = new(10000);
        }

        public override void UpdateObj(GameTime gameTime, InputState inputState)
        {
            mBackgroundLayer.Update(inputState, mGraphicsDevice.Viewport.Bounds, 1);
            inputState.DoAction(ActionType.ToggleHyperMap, () => GameLayer.LoadMap());

            mPlanetSystem.UpdateObjects(gameTime, inputState, this);
            GameLayer.Player.Update(gameTime, inputState, this);
            GameLayer.DebugSystem.CheckForSpawn(GameLayer.CurrentSystem.GetInstance(), this);

            Camera2D.Position = GameLayer.Player.Position;
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, .001f, 1);
        }

        public override void DrawOnScreenView(SceneManagerLayer sceneManagerLayer, SpriteBatch spriteBatch)
        {
            base.DrawOnScreenView(sceneManagerLayer, spriteBatch);
            mBackgroundLayer.Draw();
        }

        public override void DrawOnWorldView(SceneManagerLayer sceneManagerLayer, SpriteBatch spriteBatch)
        {
            mPlanetSystem.Draw();
            mGrid.Draw(this);
            base.DrawOnWorldView(sceneManagerLayer, spriteBatch);
        }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }
    }
}
