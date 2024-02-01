// PlanetSystemLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GridSystem;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class PlanetSystemLayer : GameLayer
    {
        private readonly UiFrame mBackgroundLayer;

        private readonly PlanetSystemInstance mPlanetSystem;
        private readonly Grid mGrid;

        public PlanetSystemLayer(GameStateLayer gameLayer, float camZoom) : base(gameLayer, 50000)
        {
            HUDLayer = new HudLayer(this);
            mBackgroundLayer = new() { Color = Color.Black, Anchor = Anchor.Center, FillScale = FillScale.FillIn };
            mBackgroundLayer.AddChild(new UiSprite(GameSpriteRegistries.gameBackground) { Anchor = Anchor.Center, FillScale = FillScale.FillIn });

            Camera2D.Zoom = camZoom;
            mPlanetSystem = gameLayer.CurrentSystem.GetInstance();
            mGrid = new(10000);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => GameState.AddLayer(new MapLayer(GameState)));
            inputState.DoAction(ActionType.Inventar, () => LayerManager.AddLayer(new InventoryLayer(GameState.Inventory, GameState.Wallet)));

            mBackgroundLayer.Update(inputState, GraphicsDevice.Viewport.Bounds, 1);

            mPlanetSystem.UpdateObjects(gameTime, inputState, this);
            GameState.Player.Update(gameTime, inputState, this);
            GameState.DebugSystem.CheckForSpawn(GameState.CurrentSystem.GetInstance(), this);

            Camera2D.Position = GameState.Player.Position;
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, .001f, 1);
            base.Update(gameTime, inputState);
        }

        public override void DrawOnScreenView(GameStateLayer gameState, SpriteBatch spriteBatch)
        {
            mBackgroundLayer.Draw();
        }

        public override void DrawOnWorldView(GameStateLayer gameState, SpriteBatch spriteBatch)
        {
            mPlanetSystem.Draw();
            mGrid.Draw(this);
        }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }

        public override void Destroy() {; }
    }
}
