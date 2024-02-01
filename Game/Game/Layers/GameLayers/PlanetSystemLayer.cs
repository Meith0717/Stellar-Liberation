// PlanetSystemLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.GridSystem;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.Layers.MenueLayers;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class PlanetSystemLayer : GameLayer
    {
        private readonly UiFrame mBackgroundLayer;

        private readonly PlanetSystemInstance mPlanetSystem;
        private readonly Grid mGrid;

        public PlanetSystemLayer(GameState gameLayer, PlanetSystemInstance currentPlanetSystem, float camZoom) : base(gameLayer, 50000)
        {
            HUDLayer = new HudLayer(this);
            mBackgroundLayer = new() { Color = Color.Black, Anchor = Anchor.Center, FillScale = FillScale.FillIn };
            mBackgroundLayer.AddChild(new UiSprite(GameSpriteRegistries.gameBackground) { Anchor = Anchor.Center, FillScale = FillScale.FillIn });

            Camera2D.Zoom = camZoom;
            mPlanetSystem = currentPlanetSystem;
            mGrid = new(10000);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, () => LayerManager.AddLayer(new PauseLayer()));

            mBackgroundLayer.Update(inputState, mGraphicsDevice.Viewport.Bounds, 1);
            inputState.DoAction(ActionType.ToggleHyperMap, () => GameState.LoadMap());

            mPlanetSystem.UpdateObjects(gameTime, inputState, this);
            GameState.Player.Update(gameTime, inputState, this);
            GameState.DebugSystem.CheckForSpawn(GameState.CurrentSystem.GetInstance(), this);

            Camera2D.Position = GameState.Player.Position;
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, .001f, 1);
            base.Update(gameTime, inputState);
        }

        public override void DrawOnScreenView(GameState gameState, SpriteBatch spriteBatch)
        {
            mBackgroundLayer.Draw();
        }

        public override void DrawOnWorldView(GameState gameState, SpriteBatch spriteBatch)
        {
            mPlanetSystem.Draw();
            mGrid.Draw(this);
        }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }

        public override void Destroy() {; }
    }
}
